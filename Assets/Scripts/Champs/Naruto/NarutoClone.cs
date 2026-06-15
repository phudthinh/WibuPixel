using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class NarutoClone : MonoBehaviourPunCallbacks
{
    private const string HiddenDamageKey = "HiddenDamage";
    private Game _game;
    private PhotonView _photonView;
    private Rigidbody2D _rb;
    private string _ofPlayer;
    private float _timeHitbox;
    private float _speedFly;
    private int _direction;
    private float _partTimeHitbox;
    public Animator _animator;
    public TextMeshProUGUI _playerNameText;
    public TextMeshProUGUI _champNameText;

    // Public variable for Chump object
    public GameObject Skill02Chump;

    private float _health = 50f;
    private bool _isGrounded = true;

    private enum State
    {
        Stand = 0,
        Idle = 1,
        Run = 2,
        Jump = 3,
        JumpMove = 4,
        Fall = 5,
        Attack01Ground = 6,
        Attack01Sky = 7,
        Attack02Ground = 8,
        Attack02Sky = 9,
        Attack03Ground = 10,
        Attack03Sky = 11,
        Attack04Ground = 12,
        Attack04Sky = 13,
        Hit01 = 14,
        Hit02 = 15,
        Hit03 = 16,
        Dash = 17,
        Skill01Ground = 18,
        Skill01Sky = 19,
        Skill02 = 20,
        Skill03 = 21,
        Skill04 = 22,
        Skill02Destroy = 23
    }
    private State currentState = State.Run;

    private enum AIState
    {
        Stand = 0,
        Idle = 1,
        Run = 2,
        Jump = 3
    }
    private AIState _aiState = AIState.Run;

    public void SetAttackInfo(string ofPlayerValue, float timeHitboxValue, string playerName, string champName, int directionValue)
    {
        _ofPlayer = ofPlayerValue;
        _timeHitbox = timeHitboxValue;
        _playerNameText.text = playerName;
        _champNameText.text = champName;
        _direction = directionValue;
    }

    public float GetHealth()
    {
        return _health;
    }

    void Awake()
    {
        _game = GameObject.Find("GameScript").GetComponent<Game>();
        _photonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        _partTimeHitbox = _timeHitbox / 2;
        if (_ofPlayer == "Player01")
        {
            _speedFly = _game.GetSpeedPlayer01();
            gameObject.tag = "Player01";
            _game.activeClonesPlayer01.Add(this);
        }
        else
        {
            _speedFly = _game.GetSpeedPlayer02();
            gameObject.tag = "Player02";
            _game.activeClonesPlayer02.Add(this);
        }

        _rb = GetComponent<Rigidbody2D>();

        // Set opacity to 50% only on the owner player's screen
        if (_photonView.IsMine)
        {
            foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
            {
                Color c = sr.color;
                c.a = 0.5f;
                sr.color = c;
            }
        }

        // Start AI behavior loop
        StartCoroutine(CloneAILoop());
    }

    private IEnumerator CloneAILoop()
    {
        while (true)
        {
            float waitTime = UnityEngine.Random.Range(1.0f, 2.5f);
            yield return new WaitForSeconds(waitTime);

            if (PhotonNetwork.LocalPlayer.Equals(_photonView.Owner))
            {
                int randVal = UnityEngine.Random.Range(0, 100);
                if (randVal < 30)
                {
                    _aiState = AIState.Stand;
                }
                else if (randVal < 85)
                {
                    _aiState = AIState.Run;
                    // 20% chance to flip direction
                    if (UnityEngine.Random.Range(0, 100) < 20)
                    {
                        _direction = -_direction;
                    }
                }
                else
                {
                    _aiState = AIState.Jump;
                }
            }
        }
    }

    void Update()
    {
        if (_timeHitbox > 0)
        {
            _timeHitbox -= Time.deltaTime;
        }
        else
        {
            currentState = State.Skill02Destroy;
            StartCoroutine(DestroyThisRPC());
        }

        if (PhotonNetwork.LocalPlayer.Equals(_photonView.Owner))
        {
            if (_aiState == AIState.Stand)
            {
                _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
                currentState = State.Idle;
            }
            else if (_aiState == AIState.Run)
            {
                _rb.linearVelocity = new Vector2(_speedFly * _direction, _rb.linearVelocity.y);
                currentState = State.Run;
            }
            else if (_aiState == AIState.Jump)
            {
                if (_isGrounded)
                {
                    _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 7f);
                    _isGrounded = false;
                }
                _rb.linearVelocity = new Vector2(_speedFly * _direction, _rb.linearVelocity.y);
                currentState = State.Jump;
                _aiState = AIState.Run;
            }

            _photonView.RPC("SyncPlayerPosition", RpcTarget.All, transform.position);
            _photonView.RPC("SetState", RpcTarget.All, (int)currentState);
            _photonView.RPC("SyncPlayerScale", RpcTarget.All, transform.localScale);
        }

        // Align text scale locally to match direction without mirroring
        if (_direction == 1)
        {
            transform.localScale = new Vector2(1, 1);
            _playerNameText.transform.localScale = new Vector2(1, 1);
            _champNameText.transform.localScale = new Vector2(1, 1);
        }
        else
        {
            transform.localScale = new Vector2(-1, 1);
            _playerNameText.transform.localScale = new Vector2(-1, 1);
            _champNameText.transform.localScale = new Vector2(-1, 1);
        }
    }

    public void TakeDamage(float damage)
    {
        _photonView.RPC("ApplyDamageRPC", RpcTarget.All, damage);
    }

    [PunRPC]
    private void ApplyDamageRPC(float damage)
    {
        _health -= damage;

        // Apply fake damage in Game class so opponent's HUD updates
        if (gameObject.tag == "Player01")
        {
            _game.fakeDamagePlayer01 += damage;
        }
        else if (gameObject.tag == "Player02")
        {
            _game.fakeDamagePlayer02 += damage;
        }

        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (Skill02Chump != null)
        {
            GameObject chump = Instantiate(Skill02Chump, transform.position, Quaternion.identity);
            Destroy(chump, 0.5f);
        }

        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        if (_game != null)
        {
            if (gameObject.tag == "Player01")
            {
                _game.activeClonesPlayer01.Remove(this);
                // Reset fake damage when the last clone of Player01 is gone
                if (_game.activeClonesPlayer01.Count == 0)
                {
                    _game.fakeDamagePlayer01 = 0f;
                }
            }
            else if (gameObject.tag == "Player02")
            {
                _game.activeClonesPlayer02.Remove(this);
                // Reset fake damage when the last clone of Player02 is gone
                if (_game.activeClonesPlayer02.Count == 0)
                {
                    _game.fakeDamagePlayer02 = 0f;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Skill") || collision.gameObject.CompareTag("Attack") || collision.gameObject.name.Contains("Skill") || collision.gameObject.name.Contains("Attack") || collision.gameObject.name.Contains("Power"))
        {
            _game.RegisterCloneHit(this, tag);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
        if (collision.CompareTag("Skill") || collision.CompareTag("Attack") || collision.gameObject.name.Contains("Skill") || collision.gameObject.name.Contains("Attack") || collision.gameObject.name.Contains("Power"))
        {
            _game.RegisterCloneHit(this, tag);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }

    [PunRPC]
    private IEnumerator DestroyThisRPC()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

    [PunRPC]
    private void SyncPlayerPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    [PunRPC]
    private void SyncPlayerScale(Vector3 newScale)
    {
        transform.localScale = newScale;
        _playerNameText.transform.localScale = newScale;
        _champNameText.transform.localScale = newScale;
    }

    [PunRPC]
    private void SetState(int state)
    {
        currentState = (State)state;
        _animator.SetInteger("State", state);
    }
}
