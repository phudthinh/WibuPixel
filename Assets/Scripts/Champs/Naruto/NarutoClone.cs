using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Text;
using System.Linq;
using System;
using Random = System.Random;


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
    private enum State
    {
        Run,
        Skill02Destroy
    }
    private State currentState = State.Run;

    public void SetAttackInfo(string ofPlayerValue, float timeHitboxValue, string playerName, string champName, int directionValue)
    {
        _ofPlayer = ofPlayerValue;
        _timeHitbox = timeHitboxValue;
        _playerNameText.text = playerName;
        _champNameText.text = champName;
        _direction = directionValue;
    }
    void Awake()
    {
        _game = GameObject.Find("GameScript").GetComponent<Game>();
        _photonView = GetComponent<PhotonView>();
    }
    void Start()
    {
        _partTimeHitbox = _timeHitbox / 2;
        if(_ofPlayer == "Player01")
        {
            _speedFly = _game.GetSpeedPlayer01();
            gameObject.tag = "Player01";
        }
        else
        {
            _speedFly = _game.GetSpeedPlayer02();
            gameObject.tag = "Player02";
        }

        _rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (_timeHitbox > 0)
        {
            _timeHitbox -= Time.deltaTime;
            currentState = State.Run;
        }
        else
        {
            currentState = State.Skill02Destroy;
            StartCoroutine(DestroyThisRPC());
        }

        if(_direction == 1)
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

        _rb.linearVelocity = new Vector2(_speedFly * _direction, 0);
        if(PhotonNetwork.LocalPlayer.Equals(_photonView.Owner))
        {
            _photonView.RPC("SyncPlayerPosition", RpcTarget.All, transform.position);
            _photonView.RPC("SetState", RpcTarget.All, (int)currentState);
            _photonView.RPC("SyncPlayerScale", RpcTarget.All, transform.localScale);
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
