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
    private bool _isDestroying = false;
    private System.Random _random;

    // Public variable for IntrinsicSkill object
    public GameObject IntrinsicSkill;

    private NarutoPrefab _realNaruto;

    private enum State
    {
        Run = 0,
        Skill02Destroy = 1
    }
    private State currentState = State.Run;

    public void SetAttackInfo(string ofPlayerValue, float timeHitboxValue, string playerName, string champName, int directionValue)
    {
        _ofPlayer = ofPlayerValue;
        _timeHitbox = timeHitboxValue;
        _playerNameText.text = playerName;
        _champNameText.text = champName;
        _direction = directionValue;

        // Initialize System.Random with a seed synchronized across all clients
        int seed = (int)(transform.position.x * 100f) + (int)(transform.position.y * 100f) + _direction + (int)(_timeHitbox * 10f);
        _random = new System.Random(seed);
    }

    public float GetHealth()
    {
        return _health;
    }

    void Awake()
    {
        _game = GameObject.Find("GameScript").GetComponent<Game>();
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

        // Check if this clone belongs to the local player
        bool isMyClone = false;
        GameObject[] p1Objects = GameObject.FindGameObjectsWithTag("Player01");
        foreach (var p1 in p1Objects)
        {
            PhotonView pv = p1.GetComponent<PhotonView>();
            if (pv != null && pv.IsMine && _ofPlayer == "Player01")
            {
                isMyClone = true;
                break;
            }
        }
        GameObject[] p2Objects = GameObject.FindGameObjectsWithTag("Player02");
        foreach (var p2 in p2Objects)
        {
            PhotonView pv = p2.GetComponent<PhotonView>();
            if (pv != null && pv.IsMine && _ofPlayer == "Player02")
            {
                isMyClone = true;
                break;
            }
        }

        // Set opacity to 50% only on the owner player's screen
        if (isMyClone)
        {
            foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
            {
                Color c = sr.color;
                c.a = 0.5f;
                sr.color = c;
            }
        }

        // Start synchronized AI direction loop
        StartCoroutine(CloneAILoop());
    }

    private IEnumerator CloneAILoop()
    {
        while (true)
        {
            if (_random == null)
            {
                yield return null;
                continue;
            }

            // Random wait time between 2.0s and 5.0s
            float waitTime = (float)(2.0 + _random.NextDouble() * 3.0);
            yield return new WaitForSeconds(waitTime);

            if (_isDestroying)
            {
                break;
            }

            // 50% chance to reverse direction
            if (_random.Next(0, 100) < 50)
            {
                _direction = -_direction;
            }
        }
    }

    void Update()
    {
        if (_isDestroying)
        {
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
            return;
        }

        if (_timeHitbox > 0)
        {
            _timeHitbox -= Time.deltaTime;
            _rb.linearVelocity = new Vector2(_speedFly * _direction, _rb.linearVelocity.y);
            currentState = State.Run;
        }
        else
        {
            _isDestroying = true;
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
            currentState = State.Skill02Destroy;
            StartCoroutine(DestroyThisRPC());
        }

        if (_animator != null)
        {
            _animator.SetInteger("State", (int)currentState);
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

        // Match the active state of the real player's intrinsic prefab
        if (IntrinsicSkill != null)
        {
            if (_realNaruto == null)
            {
                GameObject realPlayer = GameObject.FindWithTag(_ofPlayer);
                if (realPlayer != null)
                {
                    _realNaruto = realPlayer.GetComponent<NarutoPrefab>();
                }
            }

            if (_realNaruto != null && _realNaruto._intrinsicPrefab != null)
            {
                IntrinsicSkill.SetActive(_realNaruto._intrinsicPrefab.activeSelf);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        photonView.RPC("ApplyDamageRPC", RpcTarget.All, damage);
    }

    [PunRPC]
    private void ApplyDamageRPC(float damage)
    {
        _health -= damage;

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
            }
            else if (gameObject.tag == "Player02")
            {
                _game.activeClonesPlayer02.Remove(this);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
        if (collision.gameObject.name.Contains("Skill") || collision.gameObject.name.Contains("Attack") || collision.gameObject.name.Contains("Power") || collision.gameObject.name.Contains("Kunai") || collision.gameObject.name.Contains("Intrinsic"))
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
        if (collision.gameObject.name.Contains("Skill") || collision.gameObject.name.Contains("Attack") || collision.gameObject.name.Contains("Power") || collision.gameObject.name.Contains("Kunai") || collision.gameObject.name.Contains("Intrinsic"))
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
