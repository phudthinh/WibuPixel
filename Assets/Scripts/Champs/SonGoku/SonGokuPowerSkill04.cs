using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Text;
using System.Linq;

public class SonGokuPowerSkill04 : MonoBehaviourPunCallbacks
{
    private const string HiddenDamageKey = "HiddenDamage";
    private Game _game;
    private PhotonView _photonView;
    private Rigidbody2D _rb;
    private string _ofPlayer;
    private float _timeHitbox;
    private bool _isOwner;

    public void SetAttackInfo(string ofPlayerValue, float timeHitboxValue, bool isOwnerValue)
    {
        _ofPlayer = ofPlayerValue;
        _timeHitbox = timeHitboxValue;
        _isOwner = isOwnerValue;
    }
    void Awake()
    {
        _game = GameObject.Find("GameScript").GetComponent<Game>();
        _photonView = photonView;
        _rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (_timeHitbox > 0)
        {
            _timeHitbox -= Time.deltaTime;
        }
        else
        {
            DestroyThisRPC();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(_ofPlayer == "Player01")
        {
            if (collision.gameObject.CompareTag("Player02"))
            {
                if (transform.position.x > collision.transform.position.x)
                {
                    collision.transform.position = new Vector2(collision.transform.position.x - 0.05f, collision.transform.position.y);
                }
                else
                {
                    collision.transform.position = new Vector2(collision.transform.position.x + 0.05f, collision.transform.position.y);
                }

                if(transform.localScale.x > 0)
                {
                    collision.transform.localScale = new Vector2(-1, 1);
                }
                else
                {
                    collision.transform.localScale = new Vector2(1, 1);
                }
                if(_isOwner)
                {
                    float _damage = PlayerPrefs.GetFloat("DamSkill04");
                    _damage = _damage / 100;
                    float _critical = _game.GetCriticalPlayer01();
                    float _criticalDamage = _game.GetCriticalDamagePlayer01();

                    float _defensePlayer02 = _game.GetDefensePlayer02();

                    float randomDamage = Random.Range(_damage - (_damage * 10 / 100), _damage + (_damage * 10 / 100));
                    float randomCritical = Random.Range(0, 100);

                    string typeOfAttack = "Damage";

                    float randomPositionX = Random.Range(collision.transform.position.x - 0.3f, collision.transform.position.x + 0.3f);
                    float randomPositionY = Random.Range(collision.transform.position.y - 0.5f, collision.transform.position.y + 0.5f);

                    if (randomCritical <= _critical)
                    {
                        randomDamage = randomDamage + (randomDamage * _criticalDamage / 100);
                        typeOfAttack = "CriticalDamage";
                    }

                    randomDamage = randomDamage * (100 / (100 + _defensePlayer02));

                    _game.TakeDamagePlayer02(randomDamage, 0.2f);

                    GameObject _valueTextPrefab = PhotonNetwork.Instantiate("ValueTextPrefab", new Vector2(randomPositionX, randomPositionY + 0.5f), Quaternion.identity);
                    _valueTextPrefab.GetComponent<PhotonView>().RPC("SetValueTextInfoRPC", RpcTarget.All, typeOfAttack, randomDamage);

                    if(_game.GetBloodsuckingPlayer01() > 0)
                    {
                        float bloodsucking = randomDamage * _game.GetBloodsuckingPlayer01() / 100;
                        string typeOfBloodsucking = "Bloodsucking";
                        _game.TakePlusHealthPlayer01(bloodsucking);
                        if(bloodsucking >= _game.GetHealthMaxPlayer01() * 5 / 100)
                        {
                            GameObject _valueTextPrefab2 = PhotonNetwork.Instantiate("ValueTextPrefab", new Vector2(this.transform.position.x, this.transform.position.y + 0.5f), Quaternion.identity);
                            _valueTextPrefab2.GetComponent<PhotonView>().RPC("SetValueTextInfoRPC", RpcTarget.All, typeOfBloodsucking, bloodsucking);
                        }
                    }
                }
            }
        }
        else if(_ofPlayer == "Player02")
        {
            if (collision.gameObject.CompareTag("Player01"))
            {
                if (transform.position.x > collision.transform.position.x)
                {
                    collision.transform.position = new Vector2(collision.transform.position.x - 0.05f, collision.transform.position.y);
                }
                else
                {
                    collision.transform.position = new Vector2(collision.transform.position.x + 0.05f, collision.transform.position.y);
                }

                if(transform.localScale.x > 0)
                {
                    collision.transform.localScale = new Vector2(-1, 1);
                }
                else
                {
                    collision.transform.localScale = new Vector2(1, 1);
                }
                if(_isOwner)
                {
                    float _damage = PlayerPrefs.GetFloat("DamSkill04");
                    _damage = _damage / 120;
                    float _critical = _game.GetCriticalPlayer02();
                    float _criticalDamage = _game.GetCriticalDamagePlayer02();

                    float _defensePlayer01 = _game.GetDefensePlayer01();

                    float randomDamage = Random.Range(_damage - (_damage * 10 / 100), _damage + (_damage * 10 / 100));
                    float randomCritical = Random.Range(0, 100);

                    string typeOfAttack = "Damage";

                    float randomPositionX = Random.Range(collision.transform.position.x - 0.3f, collision.transform.position.x + 0.3f);
                    float randomPositionY = Random.Range(collision.transform.position.y - 0.5f, collision.transform.position.y + 0.5f);

                    if (randomCritical <= _critical)
                    {
                        randomDamage = randomDamage + (randomDamage * _criticalDamage / 100);
                        typeOfAttack = "CriticalDamage";
                    }

                    randomDamage = randomDamage * (100 / (100 + _defensePlayer01));

                    _game.TakeDamagePlayer01(randomDamage, 0.2f);
                    
                    GameObject _valueTextPrefab = PhotonNetwork.Instantiate("ValueTextPrefab", new Vector2(randomPositionX, randomPositionY + 0.5f), Quaternion.identity);
                    _valueTextPrefab.GetComponent<PhotonView>().RPC("SetValueTextInfoRPC", RpcTarget.All, typeOfAttack, randomDamage);

                    if(_game.GetBloodsuckingPlayer02() > 0)
                    {
                        float bloodsucking = randomDamage * _game.GetBloodsuckingPlayer02() / 100;
                        string typeOfBloodsucking = "Bloodsucking";
                        _game.TakePlusHealthPlayer02(bloodsucking);
                        if(bloodsucking >= _game.GetHealthMaxPlayer02() * 5 / 100)
                        {
                            GameObject _valueTextPrefab2 = PhotonNetwork.Instantiate("ValueTextPrefab", new Vector2(this.transform.position.x, this.transform.position.y + 0.5f), Quaternion.identity);
                            _valueTextPrefab2.GetComponent<PhotonView>().RPC("SetValueTextInfoRPC", RpcTarget.All, typeOfBloodsucking, bloodsucking);
                        }
                    }
                }
            }
        }
    }


    [PunRPC]
    private void DestroyThisRPC()
    {
        Destroy(this.gameObject);
    }
}
