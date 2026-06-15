using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Text;
using System.Linq;

public class LuffyPrefab : MonoBehaviourPunCallbacks
{
    private const string HiddenDamageKey = "HiddenDamage";
    private float _damage;
    private float _defense;
    private float _healthMax;
    private float _health;
    private float _healing;
    private float _manaMax;
    private float _mana;
    private float _restoreMana;
    private float _critical;
    private float _criticalDamage;
    private float _dodge;
    private float _bloodsucking;
    private float _reducedHealing;
    private float _speed;
    private float _resistance;
    private float _penetrate;
    private float _attackSpeed;
    private float _reducedTime;
    private float _stun;

    private bool _intrinsic;
    private bool _skill01;
    private bool _skill02;
    private bool _skill03;
    private bool _skill04;

    private float valueIntrinsic;

    private float _timeCooldownIntrinsic = 0;
    private float _timeCooldownSkill01 = 0;
    private float _timeCooldownSkill02 = 0;
    private float _timeCooldownSkill03 = 0;
    private float _timeCooldownSkill04 = 0;
    private float _timeCooldownSpell = 0;

    private PhotonView _photonView;
    public Animator _animator;
    private Game _game;
    private Rigidbody2D _rb;
    public TextMeshProUGUI _playerNameText;
    public TextMeshProUGUI _champNameText;
    public CapsuleCollider2D _hitBoxGround;

    private bool canControl = false;
    private float idleTimer = 0f;
    private bool _isGrounded = true;
    private int jumpCount = 0;
    private float jumpForce = 7f;
    private int _attackCount = 0;
    private float _attackWaitTime = 0f;
    public GameObject _attackPrefab;
    public GameObject _intrinsicPrefab;
    public GameObject _skill01Prefab;
    public GameObject _skill02Prefab;
    public GameObject _skill03Prefab;
    public GameObject _skill04Prefab;

    private bool _isHit = false;
    private float _hitWaitTime = 0f;
    private int _numHit;

    private bool _dash = false;

    private bool _showDustMove = false;
    private bool _showDustOnGround = false;
    public GameObject _dustMovePrefab;

    private KeyCode _keyLeft;
    private KeyCode _keyRight;
    private KeyCode _keyJump;
    private KeyCode _keyFall;
    private KeyCode _keySkill01;
    private KeyCode _keySkill02;
    private KeyCode _keySkill03;
    private KeyCode _keySkill04;
    private KeyCode _keySpell;
    private KeyCode _keyIntrinsic;
    private KeyCode _keyAttack;

    private TMP_InputField _chatInput;

    private TextMeshProUGUI _textTitleIntrinsic;
    private TextMeshProUGUI _textTitleSkill01;
    private TextMeshProUGUI _textTitleSkill02;
    private TextMeshProUGUI _textTitleSkill03;
    private TextMeshProUGUI _textTitleSkill04;
    private TextMeshProUGUI _textTitleSpell;

    private TextMeshProUGUI _textDescriptionIntrinsic;
    private TextMeshProUGUI _textDescriptionSkill01;
    private TextMeshProUGUI _textDescriptionSkill02;
    private TextMeshProUGUI _textDescriptionSkill03;
    private TextMeshProUGUI _textDescriptionSkill04;
    private TextMeshProUGUI _textDescriptionSpell;

    private TextMeshProUGUI _textTimeCooldownIntrinsic;
    private TextMeshProUGUI _textTimeCooldownSkill01;
    private TextMeshProUGUI _textTimeCooldownSkill02;
    private TextMeshProUGUI _textTimeCooldownSkill03;
    private TextMeshProUGUI _textTimeCooldownSkill04;
    private TextMeshProUGUI _textTimeCooldownSpell;

    private TextMeshProUGUI _textShowTimeCooldownIntrinsic;
    private TextMeshProUGUI _textShowTimeCooldownSkill01;
    private TextMeshProUGUI _textShowTimeCooldownSkill02;
    private TextMeshProUGUI _textShowTimeCooldownSkill03;
    private TextMeshProUGUI _textShowTimeCooldownSkill04;
    private TextMeshProUGUI _textShowTimeCooldownSpell;

    private TextMeshProUGUI _textManaIntrinsic;
    private TextMeshProUGUI _textManaSkill01;
    private TextMeshProUGUI _textManaSkill02;
    private TextMeshProUGUI _textManaSkill03;
    private TextMeshProUGUI _textManaSkill04;

    private GameObject _objectTimeCooldownIntrinsic;
    private GameObject _objectTimeCooldownSkill01;
    private GameObject _objectTimeCooldownSkill02;
    private GameObject _objectTimeCooldownSkill03;
    private GameObject _objectTimeCooldownSkill04;
    private GameObject _objectTimeCooldownSpell;

    private string _currentSpell;

    private Image _imageSpell;
    private Image _imageIntrinsic;
    private Image _imageSkill01;
    private Image _imageSkill02;
    private Image _imageSkill03;
    private Image _imageSkill04;

    public GameObject _objectHealthEffect;
    public GameObject _objectManaEffect;
    public GameObject _objectResistance;
    public GameObject _objectShield;
    public GameObject _objectFinishingBlow;
    public GameObject _objectBloodThirsty;
    public GameObject _objectStandardDamage;

    private GameObject _UIIntrinsicFrame;
    private GameObject _UISkill01Frame;
    private GameObject _UISkill02Frame;
    private GameObject _UISkill03Frame;
    private GameObject _UISkill04Frame;
    private GameObject _UISpellFrame;
    private bool _endGame = false;
    private enum State
    {
        Stand,
        Idle,
        Run,
        Jump,
        JumpMove,
        Fall,
        Attack01Ground,
        Attack01Sky,
        Attack02Ground,
        Attack02Sky,
        Attack03Ground,
        Attack03Sky,
        Attack04Ground,
        Attack04Sky,
        Hit01,
        Hit02,
        Hit03,
        Dash,
        Intrinsic01Ground,
        Intrinsic01Sky,
        Intrinsic02Ground,
        Intrinsic02Sky,
        Skill01Ground,
        Skill01Sky,
        Skill02,
        Skill03,
        Skill04A,
        Skill04B,
    }

    private int randomVoice;


    private State currentState = State.Stand;
    private readonly Dictionary<string, AnimationClip> _animationClips = new Dictionary<string, AnimationClip>();

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
        _rb = GetComponent<Rigidbody2D>();
        _game = GameObject.Find("GameScript").GetComponent<Game>();
        _photonView.RPC("SetPlayerInfo", RpcTarget.All);
        StartCoroutine(StartPlayer());
        UpdateKeyBoard();
        StartProperties();
        _chatInput = GameObject.Find("ChatInput").GetComponent<TMP_InputField>();
        
        if (_animator != null && _animator.runtimeAnimatorController != null)
        {
            foreach (var c in _animator.runtimeAnimatorController.animationClips)
            {
                _animationClips[c.name] = c;
            }
        }

        StartCoroutine(AddContentSkill());
        StartCoroutine(TimeCooldownSkill());
        StartCoroutine(Healing());
    }
    private void Update()
    {
        if (canControl && !_endGame)
        {
            Stand();
            Idle();
            MoveLeft();
            MoveRight();
            Fall();
            Attack();
            Jump();
            CannotMove();
            Hit();
            Spell();
            Skill01();
            Skill02();
            Skill03();
            Skill04();
            UpdateProperties();
            _endGame = _game.GetEndGame();
        }
        if(PhotonNetwork.LocalPlayer.Equals(_photonView.Owner))
        {
            _photonView.RPC("SyncPlayerPosition", RpcTarget.All, transform.position);
            _photonView.RPC("SetState", RpcTarget.All, (int)currentState);
            _photonView.RPC("SyncPlayerScale", RpcTarget.All, transform.localScale);
            StartCoroutine(_game.UpdatePropertiesPlayer(this.gameObject.tag));
        }
    }
    private void StartProperties()
    {
        if (PhotonNetwork.LocalPlayer.Equals(photonView.Owner))
        {
            _damage = PlayerPrefs.GetFloat("Damage");
            _defense = PlayerPrefs.GetFloat("Defense");
            _healthMax = PlayerPrefs.GetFloat("Health");
            _health = _healthMax;
            _healing = PlayerPrefs.GetFloat("Healing");
            _manaMax = PlayerPrefs.GetFloat("Mana");
            _mana = _manaMax;
            _restoreMana = PlayerPrefs.GetFloat("RestoreMana");
            _critical = PlayerPrefs.GetFloat("Critical");
            _criticalDamage = PlayerPrefs.GetFloat("CriticalDamage");
            _dodge = PlayerPrefs.GetFloat("Dodge");
            _bloodsucking = PlayerPrefs.GetFloat("Bloodsucking");
            _reducedHealing = PlayerPrefs.GetFloat("ReducedHealing");
            _speed = PlayerPrefs.GetFloat("Speed");
            _resistance = PlayerPrefs.GetFloat("Resistance");
            _penetrate = PlayerPrefs.GetFloat("Penetrate");
            _attackSpeed = PlayerPrefs.GetFloat("AttackSpeed");
            _reducedTime = PlayerPrefs.GetFloat("ReducedTime");
            _stun = PlayerPrefs.GetFloat("Stun");
            if(this.gameObject.tag == "Player01")
            {
                StartCoroutine(_game.TakePropertiesPlayer01(_damage, _defense, _healthMax, _health, _healing, _manaMax, _mana, _restoreMana, _critical, _criticalDamage, _dodge, _bloodsucking, _reducedHealing, _speed, _resistance, _penetrate, _attackSpeed, _reducedTime, _stun));
            }
            else if (this.gameObject.tag == "Player02")
            {
                StartCoroutine(_game.TakePropertiesPlayer02(_damage, _defense, _healthMax, _health, _healing, _manaMax, _mana, _restoreMana, _critical, _criticalDamage, _dodge, _bloodsucking, _reducedHealing, _speed, _resistance, _penetrate, _attackSpeed, _reducedTime, _stun));
            }
        }
    }
    private void UpdateProperties()
    {
        if(_timeCooldownIntrinsic <= 0)
        {
            _objectTimeCooldownIntrinsic.SetActive(false);
            _textShowTimeCooldownIntrinsic.text = "";
            _timeCooldownIntrinsic = 0;
        }
        else
        {
            _objectTimeCooldownIntrinsic.SetActive(true);
        }

        if(_timeCooldownSkill01 <= 0)
        {
            _objectTimeCooldownSkill01.SetActive(false);
            _textShowTimeCooldownSkill01.text = "";
            _timeCooldownSkill01 = 0;
        }
        else
        {
            _objectTimeCooldownSkill01.SetActive(true);
        }

        if(_timeCooldownSkill02 <= 0)
        {
            _objectTimeCooldownSkill02.SetActive(false);
            _textShowTimeCooldownSkill02.text = "";
            _timeCooldownSkill02 = 0;
        }
        else
        {
            _objectTimeCooldownSkill02.SetActive(true);
        }

        if(_timeCooldownSkill03 <= 0)
        {
            _objectTimeCooldownSkill03.SetActive(false);
            _textShowTimeCooldownSkill03.text = "";
            _timeCooldownSkill03 = 0;
        }
        else
        {
            _objectTimeCooldownSkill03.SetActive(true);
        }

        if(_timeCooldownSkill04 <= 0)
        {
            _objectTimeCooldownSkill04.SetActive(false);
            _textShowTimeCooldownSkill04.text = "";
            _timeCooldownSkill04 = 0;
        }
        else
        {
            _objectTimeCooldownSkill04.SetActive(true);
        }

        if(_timeCooldownSpell <= 0)
        {
            _objectTimeCooldownSpell.SetActive(false);
            _textShowTimeCooldownSpell.text = "";
            _timeCooldownSpell = 0;
        }
        else
        {
            _objectTimeCooldownSpell.SetActive(true);
        }
    }
    private void UpdateKeyBoard()
    {
        _keyLeft = (KeyCode)PlayerPrefs.GetInt("_left");
        _keyRight = (KeyCode)PlayerPrefs.GetInt("_right");
        _keyJump = (KeyCode)PlayerPrefs.GetInt("_jump");
        _keyFall = (KeyCode)PlayerPrefs.GetInt("_fall");
        _keySkill01 = (KeyCode)PlayerPrefs.GetInt("_skill01");
        _keySkill02 = (KeyCode)PlayerPrefs.GetInt("_skill02");
        _keySkill03 = (KeyCode)PlayerPrefs.GetInt("_skill03");
        _keySkill04 = (KeyCode)PlayerPrefs.GetInt("_skill04");
        _keySpell = (KeyCode)PlayerPrefs.GetInt("_spell");
        _keyIntrinsic = (KeyCode)PlayerPrefs.GetInt("_intrinsic");
        _keyAttack = (KeyCode)PlayerPrefs.GetInt("_attack");
    }
    private IEnumerator StartPlayer()
    {
        yield return new WaitForSeconds(4.0f);
        if (PhotonNetwork.LocalPlayer.Equals(photonView.Owner))
        {
            canControl = true;
        }
    }
    private void CannotMove()
    {
        if(this.transform.position.x < -14.4f)
        {
            this.transform.position = new Vector2(-14.4f, this.transform.position.y);
        }
        if (this.transform.position.x > 14.4f)
        {
            this.transform.position = new Vector2(14.4f, this.transform.position.y);
        }
    }
    private void Idle()
    {
        if(_attackWaitTime <= 0 && _hitWaitTime <= 0 && idleTimer < 3.0f && !_dash && !_skill01 && !_skill02 && !_skill03 && !_skill04)
        {
            currentState = _isGrounded ? State.Idle : State.Jump;
            _animator.speed = 1.0f;
        
            if (Input.GetKey(_keyLeft) && Input.GetKey(_keyRight))
            {
                currentState = _isGrounded ? State.Idle : State.Jump;
                _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
            }
        }
    }
    private void Stand()
    {
        if (Input.anyKey)
        {
            idleTimer = 0f;
        }
        else
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= 3.0f && _attackWaitTime <= 0 && _hitWaitTime <= 0 && !_dash && !_skill01 && !_skill02 && !_skill03 && !_skill04)
            {
                currentState = State.Stand;
            }
            if (idleTimer >= 0.5f)
            {
                _showDustMove = true;
            }
        }
    }
    private void MoveLeft()
    {
        if( _chatInput != null || !_dash || !_skill01 || !_skill02 || !_skill03 || !_skill04 )
        {
            if(_chatInput.isFocused)
            {
                return;
            }
        }
        if (Input.GetKey(_keyLeft) && !Input.GetKey(_keyRight)  && _hitWaitTime <= 0 && !_dash && !_skill01 && !_skill02 && !_skill03 && !_skill04)
        {
            if(_attackWaitTime <= 0 && _hitWaitTime <= 0)
            {
                currentState = State.Run;
                Vector2 movement = new Vector2(-1, 0);
                _rb.linearVelocity = new Vector2(movement.x * _speed, _rb.linearVelocity.y);
                transform.localScale = new Vector2(-1, 1);
                _playerNameText.transform.localScale = new Vector2(-1, 1);
                _champNameText.transform.localScale = new Vector2(-1, 1);
            }
            if(_showDustMove && _isGrounded)
            {
                _photonView.RPC("SetDustMove", RpcTarget.All, new Vector2(transform.position.x + 0.3f, transform.position.y + 0.05f), new Vector2(-0.5f, 0.5f));
                _showDustMove = false;
            }
            
        }
        else if (!Input.GetKey(_keyLeft) && !Input.GetKey(_keyRight))
        {
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
        }
    }
    private void MoveRight()
    {
        if( _chatInput != null || !_dash || !_skill01 || !_skill02 || !_skill03 || !_skill04)
        {
            if(_chatInput.isFocused)
            {
                return;
            }
        }
        if (Input.GetKey(_keyRight) && !Input.GetKey(_keyLeft)  && _hitWaitTime <= 0 && !_dash && !_skill01 && !_skill02 && !_skill03 && !_skill04)
        {
            if(_attackWaitTime <= 0 && _hitWaitTime <= 0)
            {
                currentState = State.Run;
                Vector2 movement = new Vector2(1, 0);
                _rb.linearVelocity = new Vector2(movement.x * _speed, _rb.linearVelocity.y);
                transform.localScale = new Vector2(1, 1);
                _playerNameText.transform.localScale = new Vector2(1, 1);
                _champNameText.transform.localScale = new Vector2(1, 1);
            }
            if(_showDustMove && _isGrounded)
            {
                _photonView.RPC("SetDustMove", RpcTarget.All, new Vector2(transform.position.x - 0.3f, transform.position.y + 0.05f), new Vector2(0.5f, 0.5f));
                _showDustMove = false;
            }
            
        }
        else if (!Input.GetKey(_keyLeft) && !Input.GetKey(_keyRight))
        {
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
        }
    }
        private void Jump()
    {
        if( _chatInput != null || !_dash || !_skill01 || !_skill02 || !_skill03 || !_skill04 || _intrinsic)
        {
            if(_chatInput.isFocused)
            {
                return;
            }
        }
        if (Input.GetKeyDown(_keyJump) && _hitWaitTime <= 0 && !_skill01 && !_skill02 && !_skill03 && !_skill04 && !_intrinsic)
        {
            if (_isGrounded)
            {
                jumpCount = 1;
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
                _isGrounded = false;
                _showDustOnGround = true;
                randomVoice = Random.Range(1, 3);
                if(randomVoice == 1)
                {
                    GameObject _voice = PhotonNetwork.Instantiate("SkillLuffy/VoiceJump", transform.position, Quaternion.identity);
                    StartCoroutine(DetroyObject(_voice, 5.0f));
                }
                GameObject _sound = PhotonNetwork.Instantiate("SkillLuffy/SoundJump", transform.position, Quaternion.identity);
                StartCoroutine(DetroyObject(_sound, 5.0f));
            }
            else if (jumpCount == 1)
            {
                jumpCount = 2;
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce / 1.2f);
                PhotonNetwork.Instantiate("DustDoubleJumpPrefab", transform.position, Quaternion.identity);
                randomVoice = Random.Range(1, 3);
                if(randomVoice == 1)
                {
                    GameObject _voice = PhotonNetwork.Instantiate("SkillLuffy/VoiceJump", transform.position, Quaternion.identity);
                    StartCoroutine(DetroyObject(_voice, 5.0f));
                }
                GameObject _sound = PhotonNetwork.Instantiate("SkillLuffy/SoundJump", transform.position, Quaternion.identity);
                StartCoroutine(DetroyObject(_sound, 5.0f));
            }
            else if (jumpCount == 2 && _currentSpell == "TripleJump")
            {
                jumpCount = 3;
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce / 1.2f);
                PhotonNetwork.Instantiate("DustTripleJumpPrefab", transform.position, Quaternion.identity);
                randomVoice = Random.Range(1, 3);
                if(randomVoice == 1)
                {
                    GameObject _voice = PhotonNetwork.Instantiate("SkillLuffy/VoiceJump", transform.position, Quaternion.identity);
                    StartCoroutine(DetroyObject(_voice, 5.0f));
                }
                GameObject _sound = PhotonNetwork.Instantiate("SkillLuffy/SoundJump", transform.position, Quaternion.identity);
                StartCoroutine(DetroyObject(_sound, 5.0f));
            }
        }
        if (Input.GetKey(_keyFall) && _hitWaitTime <= 0 && !_skill01 && !_skill02 && !_skill03 && !_skill04 && !_intrinsic)
        {
            if(!_isGrounded)
                _rb.gravityScale = 3f;
        }
        else
        {
            if(!_dash)
            {
                _rb.gravityScale = 1f;
            }
        }
    }
    private void Fall()
    {
        if(!_isGrounded)
        {
            if(_attackWaitTime <= 0 && _hitWaitTime <= 0 && !_dash && !_skill01 && !_skill02 && !_skill03 && !_skill04)
            {
                if(_rb.linearVelocity.y <= 0.1)
                {
                    currentState = State.Fall;
                }
                else
                {
                    if(Input.GetKey(_keyLeft) || Input.GetKey(_keyRight))
                        currentState = State.JumpMove;
                    else
                        currentState = State.Jump;
                }
            }
        }
    }
    private void Attack()
    {
        if( _chatInput != null || !_dash || _skill01 || _skill02 || _skill03 || _skill04)
        {
            if(_chatInput.isFocused)
            {
                return;
            }
        }
        AnimationClip clip;
        if(Input.GetKeyDown(_keyAttack) && _attackWaitTime <= 0 && _hitWaitTime <= 0 && !_dash && !_skill01 && !_skill02 && !_skill03 && !_skill04)
        {
            _attackCount++;
            _animator.speed = _attackSpeed;
            if(transform.localScale.x == 1)
                _rb.linearVelocity = new Vector2(_speed / 5, _rb.linearVelocity.y);
            else
                _rb.linearVelocity = new Vector2(-_speed / 5, _rb.linearVelocity.y);
            if(_attackCount > 4)
            {
                _attackCount = 1;
            }
            if(_timeCooldownIntrinsic != 0)
            {
                if(_isGrounded)
                {
                    _animationClips.TryGetValue("Attack0" + _attackCount.ToString() + "Ground", out clip);
                    if(_attackCount == 1)
                    {
                        randomVoice = Random.Range(1, 3);
                        GameObject _sound = PhotonNetwork.Instantiate("SkillLuffy/SoundAttack01", transform.position, Quaternion.identity);
                        StartCoroutine(DetroyObject(_sound, 5.0f));
                        if(randomVoice == 1)
                        {
                            GameObject _voice = PhotonNetwork.Instantiate("SkillLuffy/VoiceAttack01", transform.position, Quaternion.identity);
                            StartCoroutine(DetroyObject(_voice, 5.0f));
                        }
                        currentState = State.Attack01Ground;
                    }
                    else if(_attackCount == 2)
                    {
                        randomVoice = Random.Range(1, 3);
                        GameObject _sound = PhotonNetwork.Instantiate("SkillLuffy/SoundAttack02", transform.position, Quaternion.identity);
                        StartCoroutine(DetroyObject(_sound, 5.0f));
                        if(randomVoice == 1)
                        {
                            GameObject _voice = PhotonNetwork.Instantiate("SkillLuffy/VoiceAttack02", transform.position, Quaternion.identity);
                            StartCoroutine(DetroyObject(_voice, 5.0f));
                        }
                        currentState = State.Attack02Ground;
                    }
                    else if(_attackCount == 3)
                    {
                        randomVoice = Random.Range(1, 3);
                        GameObject _sound = PhotonNetwork.Instantiate("SkillLuffy/SoundAttack03", transform.position, Quaternion.identity);
                        StartCoroutine(DetroyObject(_sound, 5.0f));
                        if(randomVoice == 1)
                        {
                            GameObject _voice = PhotonNetwork.Instantiate("SkillLuffy/VoiceAttack03", transform.position, Quaternion.identity);
                            StartCoroutine(DetroyObject(_voice, 5.0f));
                        }
                        currentState = State.Attack03Ground;
                    }
                    else if(_attackCount == 4)
                    {
                        randomVoice = Random.Range(1, 3);
                        GameObject _sound = PhotonNetwork.Instantiate("SkillLuffy/SoundAttack04", transform.position, Quaternion.identity);
                        StartCoroutine(DetroyObject(_sound, 5.0f));
                        if(randomVoice == 1)
                        {
                            GameObject _voice = PhotonNetwork.Instantiate("SkillLuffy/VoiceAttack04", transform.position, Quaternion.identity);
                            StartCoroutine(DetroyObject(_voice, 5.0f));
                        }
                        currentState = State.Attack04Ground;
                    }
                }
                else
                {
                    _animationClips.TryGetValue("Attack0" + _attackCount.ToString() + "Sky", out clip);
                    if(_attackCount == 1)
                    {
                        randomVoice = Random.Range(1, 3);
                        GameObject _sound = PhotonNetwork.Instantiate("SkillLuffy/SoundAttack01", transform.position, Quaternion.identity);
                        StartCoroutine(DetroyObject(_sound, 5.0f));
                        if(randomVoice == 1)
                        {
                            GameObject _voice = PhotonNetwork.Instantiate("SkillLuffy/VoiceAttack01", transform.position, Quaternion.identity);
                            StartCoroutine(DetroyObject(_voice, 5.0f));
                        }
                        currentState = State.Attack01Sky;
                    }
                    else if(_attackCount == 2)
                    {
                        randomVoice = Random.Range(1, 3);
                        GameObject _sound = PhotonNetwork.Instantiate("SkillLuffy/SoundAttack02", transform.position, Quaternion.identity);
                        StartCoroutine(DetroyObject(_sound, 5.0f));
                        if(randomVoice == 1)
                        {
                            GameObject _voice = PhotonNetwork.Instantiate("SkillLuffy/VoiceAttack02", transform.position, Quaternion.identity);
                            StartCoroutine(DetroyObject(_voice, 5.0f));
                        }
                        currentState = State.Attack02Sky;
                    }
                    else if(_attackCount == 3)
                    {
                        randomVoice = Random.Range(1, 3);
                        GameObject _sound = PhotonNetwork.Instantiate("SkillLuffy/SoundAttack03", transform.position, Quaternion.identity);
                        StartCoroutine(DetroyObject(_sound, 5.0f));
                        if(randomVoice == 1)
                        {
                            GameObject _voice = PhotonNetwork.Instantiate("SkillLuffy/VoiceAttack03", transform.position, Quaternion.identity);
                            StartCoroutine(DetroyObject(_voice, 5.0f));
                        }
                        currentState = State.Attack03Sky;
                    }
                    else if(_attackCount == 4)
                    {
                        randomVoice = Random.Range(1, 3);
                        GameObject _sound = PhotonNetwork.Instantiate("SkillLuffy/SoundAttack04", transform.position, Quaternion.identity);
                        StartCoroutine(DetroyObject(_sound, 5.0f));
                        if(randomVoice == 1)
                        {
                            GameObject _voice = PhotonNetwork.Instantiate("SkillLuffy/VoiceAttack04", transform.position, Quaternion.identity);
                            StartCoroutine(DetroyObject(_voice, 5.0f));
                        }
                        currentState = State.Attack04Sky;
                    }
                }
            }
            else
            {
                if(_isGrounded)
                {
                    _animationClips.TryGetValue("Intrinsic01Ground", out clip);
                    if(_attackCount == 1 || _attackCount == 3)
                    {
                        randomVoice = Random.Range(1, 3);
                        GameObject _sound = PhotonNetwork.Instantiate("SkillLuffy/SoundAttack01", transform.position, Quaternion.identity);
                        StartCoroutine(DetroyObject(_sound, 5.0f));
                        if(randomVoice == 1)
                        {
                            GameObject _voice = PhotonNetwork.Instantiate("SkillLuffy/VoiceAttack01", transform.position, Quaternion.identity);
                            StartCoroutine(DetroyObject(_voice, 5.0f));
                        }
                        currentState = State.Intrinsic01Ground;
                    }
                    else if(_attackCount == 2 || _attackCount == 4)
                    {
                        randomVoice = Random.Range(1, 3);
                        GameObject _sound = PhotonNetwork.Instantiate("SkillLuffy/SoundAttack02", transform.position, Quaternion.identity);
                        StartCoroutine(DetroyObject(_sound, 5.0f));
                        if(randomVoice == 1)
                        {
                            GameObject _voice = PhotonNetwork.Instantiate("SkillLuffy/VoiceAttack02", transform.position, Quaternion.identity);
                            StartCoroutine(DetroyObject(_voice, 5.0f));
                        }
                        currentState = State.Intrinsic02Ground;
                    }
                    else
                    {
                        currentState = State.Intrinsic01Ground;
                    }
                }
                else
                {
                    _animationClips.TryGetValue("Intrinsic01Sky", out clip);
                    if(_attackCount == 1 || _attackCount == 3)
                    {
                        randomVoice = Random.Range(1, 3);
                        GameObject _sound = PhotonNetwork.Instantiate("SkillLuffy/SoundAttack01", transform.position, Quaternion.identity);
                        StartCoroutine(DetroyObject(_sound, 5.0f));
                        if(randomVoice == 1)
                        {
                            GameObject _voice = PhotonNetwork.Instantiate("SkillLuffy/VoiceAttack01", transform.position, Quaternion.identity);
                            StartCoroutine(DetroyObject(_voice, 5.0f));
                        }
                        currentState = State.Intrinsic01Sky;
                    }
                    else if(_attackCount == 2 || _attackCount == 4)
                    {
                        randomVoice = Random.Range(1, 3);
                        GameObject _sound = PhotonNetwork.Instantiate("SkillLuffy/SoundAttack02", transform.position, Quaternion.identity);
                        StartCoroutine(DetroyObject(_sound, 5.0f));
                        if(randomVoice == 1)
                        {
                            GameObject _voice = PhotonNetwork.Instantiate("SkillLuffy/VoiceAttack02", transform.position, Quaternion.identity);
                            StartCoroutine(DetroyObject(_voice, 5.0f));
                        }
                        currentState = State.Intrinsic02Sky;
                    }
                    else
                    {
                        currentState = State.Intrinsic01Sky;
                    }
                }
            }
            _attackWaitTime = clip.length / _animator.speed;
            if(_timeCooldownIntrinsic != 0)
                {
                if(transform.localScale.x == 1)
                    _photonView.RPC("ShowHitboxAttack", RpcTarget.All, new Vector2(transform.position.x, transform.position.y), new Vector2(0.5f, 0.5f), _attackWaitTime);
                else
                    _photonView.RPC("ShowHitboxAttack", RpcTarget.All, new Vector2(transform.position.x, transform.position.y), new Vector2(-0.5f, 0.5f), _attackWaitTime);
            }
            else
            {
                if(transform.localScale.x == 1)
                    _photonView.RPC("ShowHitboxIntrinsic", RpcTarget.All, new Vector2(transform.position.x, transform.position.y), new Vector2(0.5f, 0.5f), _attackWaitTime);
                else
                    _photonView.RPC("ShowHitboxIntrinsic", RpcTarget.All, new Vector2(transform.position.x, transform.position.y), new Vector2(-0.5f, 0.5f), _attackWaitTime);

                if(this.gameObject.tag == "Player01")
                {
                    _timeCooldownIntrinsic = PlayerPrefs.GetFloat("CooldownIntrinsic") - (PlayerPrefs.GetFloat("CooldownIntrinsic") * _game.GetReducedTimePlayer01() / 100);
                    _game.TakeMinusManaPlayer01(PlayerPrefs.GetFloat("ManaIntrinsic"));
                }
                else if(this.gameObject.tag == "Player02")
                {
                    _timeCooldownIntrinsic = PlayerPrefs.GetFloat("CooldownIntrinsic") - (PlayerPrefs.GetFloat("CooldownIntrinsic") * _game.GetReducedTimePlayer02() / 100);
                    _game.TakeMinusManaPlayer02(PlayerPrefs.GetFloat("ManaIntrinsic"));
                }
            }
        }
        _attackWaitTime -= Time.deltaTime;
    }
    public void Hit()
    {
        if(!_dash)
        {
            if (this.gameObject.tag == "Player01")
                _hitWaitTime = _game.GetHitTimeWaitPlayer01();
            else if (this.gameObject.tag == "Player02")
                _hitWaitTime = _game.GetHitTimeWaitPlayer02();

            if (_hitWaitTime > 0 && !_isHit && !_skill01 && !_skill02 && !_skill03 && !_skill04)
            {
                _numHit++;
                if(_numHit > 3)
                {
                    _numHit = 1;
                }
                if (_numHit == 1 && currentState != State.Hit01)
                currentState = State.Hit01;
                else if (_numHit == 2 && currentState != State.Hit02)
                    currentState = State.Hit02;
                else if (_numHit == 3 && currentState != State.Hit03)
                    currentState = State.Hit03;
                _isHit = true;
            }
            else if (_hitWaitTime <= 0 && _isHit)
            {
                _isHit = false;
            }
        }
    }
    private void Spell()
    {
        if (Input.GetKeyDown(_keySpell) && _timeCooldownSpell == 0 && !_skill01 && !_skill02 && !_skill03 && !_skill04)
        {
            _timeCooldownSpell = float.Parse(PlayerPrefs.GetString("ValueTimeCooldownSpell"));
            if(this.gameObject.tag == "Player01")
            {
                if(_currentSpell == "Health")
                {
                    float _plusHealth = float.Parse(PlayerPrefs.GetString("ValueSpell")) * _healthMax / 100;
                    _game.TakePlusHealthPlayer01(_plusHealth);
                    float randomPositionX = Random.Range(transform.position.x - 0.3f, transform.position.x + 0.3f);
                    float randomPositionY = Random.Range(transform.position.y - 0.5f, transform.position.y + 0.5f);
                    string typeOfAttack = "Healing";
                    GameObject _valueTextPrefab = PhotonNetwork.Instantiate("ValueTextPrefab", new Vector2(randomPositionX, randomPositionY + 0.5f), Quaternion.identity);
                    _valueTextPrefab.GetComponent<PhotonView>().RPC("SetValueTextInfoRPC", RpcTarget.All, typeOfAttack, _plusHealth);
                }
                else if(_currentSpell == "Mana")
                {
                    float _plusMana = float.Parse(PlayerPrefs.GetString("ValueSpell")) * _manaMax / 100;
                    _game.TakePlusManaPlayer01(_plusMana);
                    float randomPositionX = Random.Range(transform.position.x - 0.3f, transform.position.x + 0.3f);
                    float randomPositionY = Random.Range(transform.position.y - 0.5f, transform.position.y + 0.5f);
                    string typeOfAttack = "RestoreMana";
                    GameObject _valueTextPrefab = PhotonNetwork.Instantiate("ValueTextPrefab", new Vector2(randomPositionX, randomPositionY + 0.5f), Quaternion.identity);
                    _valueTextPrefab.GetComponent<PhotonView>().RPC("SetValueTextInfoRPC", RpcTarget.All, typeOfAttack, _plusMana);
                }
                else if(_currentSpell == "Dash")
                {
                    PhotonNetwork.Instantiate("SpellGame/DashLuffy", new Vector2(transform.position.x, transform.position.y + 0.3f), Quaternion.identity);
                    _rb.gravityScale = 0;
                    _rb.linearVelocity = new Vector2(0, 0);
                    transform.position = new Vector2(transform.position.x + 3.0f * transform.localScale.x, transform.position.y);
                    currentState = State.Dash;
                    _dash = true;
                    StartCoroutine(UnDash());
                }
                else if(_currentSpell == "TripleJump")
                {
                }
                else if(_currentSpell == "Resistance")
                {
                }
                else if(_currentSpell == "Shield")
                {
                }
                else if(_currentSpell == "FinishingBlow")
                {
                }
                else if (_currentSpell == "BloodThirsty")
                {
                }
                else if (_currentSpell == "StandardDamage")
                {
                }
                _photonView.RPC("SetSpell", RpcTarget.All, _currentSpell);
                StartCoroutine(TimeUnSetSpell(_currentSpell, float.Parse(PlayerPrefs.GetString("TimeSpell"))));
            }
            else if(this.gameObject.tag == "Player02")
            {
                if(_currentSpell == "Health")
                {
                    float _plusHealth = float.Parse(PlayerPrefs.GetString("ValueSpell")) * _healthMax / 100;
                    _game.TakePlusHealthPlayer02(_plusHealth);
                    float randomPositionX = Random.Range(transform.position.x - 0.3f, transform.position.x + 0.3f);
                    float randomPositionY = Random.Range(transform.position.y - 0.5f, transform.position.y + 0.5f);
                    string typeOfAttack = "Healing";
                    GameObject _valueTextPrefab = PhotonNetwork.Instantiate("ValueTextPrefab", new Vector2(randomPositionX, randomPositionY + 0.5f), Quaternion.identity);
                    _valueTextPrefab.GetComponent<PhotonView>().RPC("SetValueTextInfoRPC", RpcTarget.All, typeOfAttack, _plusHealth);
                }
                else if(_currentSpell == "Mana")
                {
                    float _plusMana = float.Parse(PlayerPrefs.GetString("ValueSpell")) * _manaMax / 100;
                    _game.TakePlusManaPlayer02(_plusMana);
                    float randomPositionX = Random.Range(transform.position.x - 0.3f, transform.position.x + 0.3f);
                    float randomPositionY = Random.Range(transform.position.y - 0.5f, transform.position.y + 0.5f);
                    string typeOfAttack = "RestoreMana";
                    GameObject _valueTextPrefab = PhotonNetwork.Instantiate("ValueTextPrefab", new Vector2(randomPositionX, randomPositionY + 0.5f), Quaternion.identity);
                    _valueTextPrefab.GetComponent<PhotonView>().RPC("SetValueTextInfoRPC", RpcTarget.All, typeOfAttack, _plusMana);
                }
                else if(_currentSpell == "Dash")
                {
                    PhotonNetwork.Instantiate("SpellGame/DashLuffy", new Vector2(transform.position.x, transform.position.y + 0.3f), Quaternion.identity);
                    _rb.gravityScale = 0;
                    _rb.linearVelocity = new Vector2(0, 0);
                    transform.position = new Vector2(transform.position.x + 3.0f * transform.localScale.x, transform.position.y);
                    currentState = State.Dash;
                    _dash = true;
                    StartCoroutine(UnDash());
                }
                else if(_currentSpell == "TripleJump")
                {
                }
                else if(_currentSpell == "Resistance")
                {
                }
                else if(_currentSpell == "Shield")
                {
                }
                else if(_currentSpell == "FinishingBlow")
                {
                }
                else if (_currentSpell == "BloodThirsty")
                {
                }
                else if (_currentSpell == "StandardDamage")
                {
                }
                _photonView.RPC("SetSpell", RpcTarget.All, _currentSpell);
                StartCoroutine(TimeUnSetSpell(_currentSpell, float.Parse(PlayerPrefs.GetString("TimeSpell"))));
            }
        }
        else if (Input.GetKeyUp(_keySpell))
        {
        }
    }
    IEnumerator TimeUnSetSpell(string spell, float time)
    {
        yield return new WaitForSeconds(time);
        _photonView.RPC("UnSetSpell", RpcTarget.All, spell);
    }
    private void Skill01()
    {
        if( _chatInput != null || !_dash || _skill01 || _skill02 || _skill03 || _skill04 || _intrinsic)
        {
            if(_chatInput.isFocused)
            {
                return;
            }
        }
        if(Input.GetKeyDown(_keySkill01)  && _attackWaitTime <= 0 && _hitWaitTime <= 0 && !_dash && _timeCooldownSkill01 == 0 && !_skill01 && !_skill02 && !_skill03 && !_skill04)
        {
            if(this.gameObject.tag == "Player01")
            {
                if(_game.GetManaPlayer01() < PlayerPrefs.GetFloat("ManaSkill01"))
                {
                    return;
                }
                _timeCooldownSkill01 = PlayerPrefs.GetFloat("CooldownSkill01") - (PlayerPrefs.GetFloat("CooldownSkill01") * _game.GetReducedTimePlayer01() / 100);
                _game.TakeMinusManaPlayer01(PlayerPrefs.GetFloat("ManaSkill01"));
            }
            else if(this.gameObject.tag == "Player02")
            {
                if(_game.GetManaPlayer02() < PlayerPrefs.GetFloat("ManaSkill01"))
                {
                    return;
                }
                _timeCooldownSkill01 = PlayerPrefs.GetFloat("CooldownSkill01") - (PlayerPrefs.GetFloat("CooldownSkill01") * _game.GetReducedTimePlayer02() / 100);
                _game.TakeMinusManaPlayer02(PlayerPrefs.GetFloat("ManaSkill01"));
            }
            _skill01 = true;
            randomVoice = Random.Range(1, 3);
            GameObject _sound = PhotonNetwork.Instantiate("SkillLuffy/SoundSkill01", transform.position, Quaternion.identity);
            StartCoroutine(DetroyObject(_sound, 5.0f));
            if(randomVoice == 1)
            {
                GameObject _voice = PhotonNetwork.Instantiate("SkillLuffy/VoiceSkill01", transform.position, Quaternion.identity);
                StartCoroutine(DetroyObject(_voice, 5.0f));
            }
            _rb.gravityScale = 0;
            _rb.linearVelocity = new Vector2(0, 0);
            if(_isGrounded)
            {
                currentState = State.Skill01Ground;
            }
            else
            {
                currentState = State.Skill01Sky;
            }
            if(transform.localScale.x == 1)
                _photonView.RPC("ShowHitboxSkill01", RpcTarget.All, new Vector2(transform.position.x, transform.position.y), new Vector2(0.5f, 0.5f), 0.8f);  
            else
                _photonView.RPC("ShowHitboxSkill01", RpcTarget.All, new Vector2(transform.position.x, transform.position.y), new Vector2(-0.5f, 0.5f), 0.8f);            
            StartCoroutine(TimeUnSkill01(0.8f));
        }
    }
    IEnumerator TimeUnSkill01(float time)
    {
        yield return new WaitForSeconds(time);
        _skill01 = false;
    }
    private void Skill02()
    {
        if( _chatInput != null || !_dash || _skill01 || _skill02 || _skill03 || _skill04)
        {
            if(_chatInput.isFocused)
            {
                return;
            }
        }
        if(Input.GetKeyDown(_keySkill02)  && _attackWaitTime <= 0 && _hitWaitTime <= 0 && !_dash && _timeCooldownSkill02 == 0 && !_skill01 && !_skill02 && !_skill03 && !_skill04)
        {
            if(this.gameObject.tag == "Player01")
            {
                if(_game.GetManaPlayer01() < PlayerPrefs.GetFloat("ManaSkill02"))
                {
                    return;
                }
                _timeCooldownSkill02 = PlayerPrefs.GetFloat("CooldownSkill02") - (PlayerPrefs.GetFloat("CooldownSkill02") * _game.GetReducedTimePlayer01() / 100);
                _game.TakeMinusManaPlayer01(PlayerPrefs.GetFloat("ManaSkill02"));
            }
            else if(this.gameObject.tag == "Player02")
            {
                if(_game.GetManaPlayer02() < PlayerPrefs.GetFloat("ManaSkill02"))
                {
                    return;
                }
                _timeCooldownSkill02 = PlayerPrefs.GetFloat("CooldownSkill02") - (PlayerPrefs.GetFloat("CooldownSkill02") * _game.GetReducedTimePlayer02() / 100);
                _game.TakeMinusManaPlayer02(PlayerPrefs.GetFloat("ManaSkill02"));
            }
            _skill02 = true;
            randomVoice = Random.Range(1, 3);
            GameObject _sound = PhotonNetwork.Instantiate("SkillLuffy/SoundSkill02", transform.position, Quaternion.identity);
            StartCoroutine(DetroyObject(_sound, 5.0f));
            if(randomVoice == 1)
            {
                GameObject _voice = PhotonNetwork.Instantiate("SkillLuffy/VoiceSkill02", transform.position, Quaternion.identity);
                StartCoroutine(DetroyObject(_voice, 5.0f));
            }
            _rb.linearVelocity = new Vector2(0, 0);
            _rb.gravityScale = 0;
            currentState = State.Skill02;
            if(transform.localScale.x == 1)
                _photonView.RPC("ShowHitboxSkill02", RpcTarget.All, new Vector2(transform.position.x, transform.position.y), new Vector2(0.5f, 0.5f), 0.8f);  
            else
                _photonView.RPC("ShowHitboxSkill02", RpcTarget.All, new Vector2(transform.position.x, transform.position.y), new Vector2(-0.5f, 0.5f), 0.8f); 
            StartCoroutine(TimeUnSkill02(0.8f));
        }
    }
    IEnumerator TimeUnSkill02(float time)
    {
        yield return new WaitForSeconds(time);
        _skill02 = false;
    }
    private void Skill03()
    {
        if( _chatInput != null || !_dash || _skill01 || _skill02 || _skill03 || _skill04)
        {
            if(_chatInput.isFocused)
            {
                return;
            }
        }
        if(Input.GetKeyDown(_keySkill03)  && _attackWaitTime <= 0 && _hitWaitTime <= 0 && !_dash && _timeCooldownSkill03 == 0 && !_skill01 && !_skill02 && !_skill03 && !_skill04)
        {
            if(this.gameObject.tag == "Player01")
            {
                if(_game.GetManaPlayer01() < PlayerPrefs.GetFloat("ManaSkill03"))
                {
                    return;
                }
                _timeCooldownSkill03 = PlayerPrefs.GetFloat("CooldownSkill03") - (PlayerPrefs.GetFloat("CooldownSkill03") * _game.GetReducedTimePlayer01() / 100);
                _game.TakeMinusManaPlayer01(PlayerPrefs.GetFloat("ManaSkill03"));
            }
            else if(this.gameObject.tag == "Player02")
            {
                if(_game.GetManaPlayer02() < PlayerPrefs.GetFloat("ManaSkill03"))
                {
                    return;
                }
                _timeCooldownSkill03 = PlayerPrefs.GetFloat("CooldownSkill03") - (PlayerPrefs.GetFloat("CooldownSkill03") * _game.GetReducedTimePlayer02() / 100);
                _game.TakeMinusManaPlayer02(PlayerPrefs.GetFloat("ManaSkill03"));
            }
            _skill03 = true;
            randomVoice = Random.Range(1, 3);
            GameObject _sound = PhotonNetwork.Instantiate("SkillLuffy/SoundSkill03", transform.position, Quaternion.identity);
            StartCoroutine(DetroyObject(_sound, 5.0f));
            if(randomVoice == 1)
            {
                GameObject _voice = PhotonNetwork.Instantiate("SkillLuffy/VoiceSkill03", transform.position, Quaternion.identity);
                StartCoroutine(DetroyObject(_voice, 5.0f));
            }
            _rb.linearVelocity = new Vector2(0, 0);
            _rb.gravityScale = 0;
            currentState = State.Skill03;
            if(transform.localScale.x == 1)
                _photonView.RPC("ShowHitboxSkill03", RpcTarget.All, new Vector2(transform.position.x, transform.position.y), new Vector2(0.5f, 0.5f), 1.5f);  
            else
                _photonView.RPC("ShowHitboxSkill03", RpcTarget.All, new Vector2(transform.position.x, transform.position.y), new Vector2(-0.5f, 0.5f), 1.5f); 
            StartCoroutine(TimeUnSkill03(1.5f));
        }
    }
    private IEnumerator TimeUnSkill03(float time)
    {
        yield return new WaitForSeconds(time);
        _skill03 = false;
    }
    private void Skill04()
    {
        if( _chatInput != null || !_dash || _skill01 || _skill02 || _skill03 || _skill04 || _intrinsic)
        {
            if(_chatInput.isFocused)
            {
                return;
            }
        }
        bool _directionSkill04 = Random.Range(0, 2) == 0;
        if(Input.GetKeyDown(_keySkill04)  && _attackWaitTime <= 0 && _hitWaitTime <= 0 && !_dash && _timeCooldownSkill04 == 0 && !_skill01 && !_skill02 && !_skill03 && !_skill04)
        {
            if(this.gameObject.tag == "Player01")
            {
                if(_game.GetManaPlayer01() < PlayerPrefs.GetFloat("ManaSkill04"))
                {
                    return;
                }
                _timeCooldownSkill04 = PlayerPrefs.GetFloat("CooldownSkill04") - (PlayerPrefs.GetFloat("CooldownSkill04") * _game.GetReducedTimePlayer01() / 100);
                _game.TakeMinusManaPlayer01(PlayerPrefs.GetFloat("ManaSkill04"));
            }
            else if(this.gameObject.tag == "Player02")
            {
                if(_game.GetManaPlayer02() < PlayerPrefs.GetFloat("ManaSkill04"))
                {
                    return;
                }
                _timeCooldownSkill04 = PlayerPrefs.GetFloat("CooldownSkill04") - (PlayerPrefs.GetFloat("CooldownSkill04") * _game.GetReducedTimePlayer02() / 100);
                _game.TakeMinusManaPlayer02(PlayerPrefs.GetFloat("ManaSkill04"));
            }
            _skill04 = true;
            randomVoice = Random.Range(1, 3);
            GameObject _sound = PhotonNetwork.Instantiate("SkillLuffy/SoundSkill04", transform.position, Quaternion.identity);
            StartCoroutine(DetroyObject(_sound, 5.0f));
            if(randomVoice == 1)
            {
                GameObject _voice = PhotonNetwork.Instantiate("SkillLuffy/VoiceSkill04", transform.position, Quaternion.identity);
                StartCoroutine(DetroyObject(_voice, 5.0f));
            }
            _rb.gravityScale = 0;
            _rb.linearVelocity = new Vector2(0, 0);
            if(_directionSkill04)
            {
                currentState = State.Skill04A;
            }
            else
            {
                currentState = State.Skill04B;
            }
            _directionSkill04 = !_directionSkill04;
            if(transform.localScale.x == 1)
                _photonView.RPC("ShowHitboxSkill04", RpcTarget.All, new Vector2(transform.position.x, transform.position.y), new Vector2(0.5f, 0.5f), 0.7f);  
            else
                _photonView.RPC("ShowHitboxSkill04", RpcTarget.All, new Vector2(transform.position.x, transform.position.y), new Vector2(-0.5f, 0.5f), 0.7f); 
            StartCoroutine(TimeUnSkill04(0.7f));
        }
    }
    IEnumerator TimeUnSkill04(float time)
    {
        yield return new WaitForSeconds(time);
        _skill04 = false;
    }

    private IEnumerator UnDash()
    {
        yield return new WaitForSeconds(0.3f);
        _dash = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if(_showDustOnGround)
            {
                PhotonNetwork.Instantiate("DustOnGroundPrefab", new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                _showDustOnGround = false;
            }
            jumpCount = 0;
            _isGrounded = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }
    private IEnumerator AddContentSkill()
    {
        _textTitleIntrinsic = GameObject.Find("TextTitleIntrinsic").GetComponent<TextMeshProUGUI>();
        _textTitleSkill01 = GameObject.Find("TextTitleSkill01").GetComponent<TextMeshProUGUI>();
        _textTitleSkill02 = GameObject.Find("TextTitleSkill02").GetComponent<TextMeshProUGUI>();
        _textTitleSkill03 = GameObject.Find("TextTitleSkill03").GetComponent<TextMeshProUGUI>();
        _textTitleSkill04 = GameObject.Find("TextTitleSkill04").GetComponent<TextMeshProUGUI>();
        _textTitleSpell = GameObject.Find("TextTitleSpell").GetComponent<TextMeshProUGUI>();

        _textDescriptionIntrinsic = GameObject.Find("TextDescriptionIntrinsic").GetComponent<TextMeshProUGUI>();
        _textDescriptionSkill01 = GameObject.Find("TextDescriptionSkill01").GetComponent<TextMeshProUGUI>();
        _textDescriptionSkill02 = GameObject.Find("TextDescriptionSkill02").GetComponent<TextMeshProUGUI>();
        _textDescriptionSkill03 = GameObject.Find("TextDescriptionSkill03").GetComponent<TextMeshProUGUI>();
        _textDescriptionSkill04 = GameObject.Find("TextDescriptionSkill04").GetComponent<TextMeshProUGUI>();
        _textDescriptionSpell = GameObject.Find("TextDescriptionSpell").GetComponent<TextMeshProUGUI>();

        _textManaIntrinsic = GameObject.Find("TextManaIntrinsic").GetComponent<TextMeshProUGUI>();
        _textManaSkill01 = GameObject.Find("TextManaSkill01").GetComponent<TextMeshProUGUI>();
        _textManaSkill02 = GameObject.Find("TextManaSkill02").GetComponent<TextMeshProUGUI>();
        _textManaSkill03 = GameObject.Find("TextManaSkill03").GetComponent<TextMeshProUGUI>();
        _textManaSkill04 = GameObject.Find("TextManaSkill04").GetComponent<TextMeshProUGUI>();

        _textTimeCooldownIntrinsic = GameObject.Find("TextTimeCooldownIntrinsic").GetComponent<TextMeshProUGUI>();
        _textTimeCooldownSkill01 = GameObject.Find("TextTimeCooldownSkill01").GetComponent<TextMeshProUGUI>();
        _textTimeCooldownSkill02 = GameObject.Find("TextTimeCooldownSkill02").GetComponent<TextMeshProUGUI>();
        _textTimeCooldownSkill03 = GameObject.Find("TextTimeCooldownSkill03").GetComponent<TextMeshProUGUI>();
        _textTimeCooldownSkill04 = GameObject.Find("TextTimeCooldownSkill04").GetComponent<TextMeshProUGUI>();
        _textTimeCooldownSpell = GameObject.Find("TextTimeCooldownSpell").GetComponent<TextMeshProUGUI>();

        _UIIntrinsicFrame = GameObject.Find("UIIntrinsicFrame");
        _UISkill01Frame = GameObject.Find("UISkill01Frame");
        _UISkill02Frame = GameObject.Find("UISkill02Frame");
        _UISkill03Frame = GameObject.Find("UISkill03Frame");
        _UISkill04Frame = GameObject.Find("UISkill04Frame");
        _UISpellFrame = GameObject.Find("UISpellFrame");

        _UIIntrinsicFrame.SetActive(false);
        _UISkill01Frame.SetActive(false);
        _UISkill02Frame.SetActive(false);
        _UISkill03Frame.SetActive(false);
        _UISkill04Frame.SetActive(false);
        _UISpellFrame.SetActive(false);

        _textShowTimeCooldownIntrinsic = GameObject.Find("TextShowTimeCooldownIntrinsic").GetComponent<TextMeshProUGUI>();
        _textShowTimeCooldownSkill01 = GameObject.Find("TextShowTimeCooldownSkill01").GetComponent<TextMeshProUGUI>();
        _textShowTimeCooldownSkill02 = GameObject.Find("TextShowTimeCooldownSkill02").GetComponent<TextMeshProUGUI>();
        _textShowTimeCooldownSkill03 = GameObject.Find("TextShowTimeCooldownSkill03").GetComponent<TextMeshProUGUI>();
        _textShowTimeCooldownSkill04 = GameObject.Find("TextShowTimeCooldownSkill04").GetComponent<TextMeshProUGUI>();
        _textShowTimeCooldownSpell = GameObject.Find("TextShowTimeCooldownSpell").GetComponent<TextMeshProUGUI>();

        _objectTimeCooldownIntrinsic = GameObject.Find("ObjectTimeCooldownIntrinsic");
        _objectTimeCooldownSkill01 = GameObject.Find("ObjectTimeCooldownSkill01");
        _objectTimeCooldownSkill02 = GameObject.Find("ObjectTimeCooldownSkill02");
        _objectTimeCooldownSkill03 = GameObject.Find("ObjectTimeCooldownSkill03");
        _objectTimeCooldownSkill04 = GameObject.Find("ObjectTimeCooldownSkill04");
        _objectTimeCooldownSpell = GameObject.Find("ObjectTimeCooldownSpell");

        _objectTimeCooldownIntrinsic.SetActive(false);
        _objectTimeCooldownSkill01.SetActive(false);
        _objectTimeCooldownSkill02.SetActive(false);
        _objectTimeCooldownSkill03.SetActive(false);
        _objectTimeCooldownSkill04.SetActive(false);
        _objectTimeCooldownSpell.SetActive(false);

        _currentSpell = PlayerPrefs.GetString("Spell");

        _imageSpell = GameObject.Find("SpellFrame").GetComponent<Image>();
        _imageIntrinsic = GameObject.Find("IntrinsicFrame").GetComponent<Image>();
        _imageSkill01 = GameObject.Find("Skill01Frame").GetComponent<Image>();
        _imageSkill02 = GameObject.Find("Skill02Frame").GetComponent<Image>();
        _imageSkill03 = GameObject.Find("Skill03Frame").GetComponent<Image>();
        _imageSkill04 = GameObject.Find("Skill04Frame").GetComponent<Image>();

        _textTitleIntrinsic.text = PlayerPrefs.GetString("TitleIntrinsic");
        _textTitleSkill01.text = PlayerPrefs.GetString("TitleSkill01");
        _textTitleSkill02.text = PlayerPrefs.GetString("TitleSkill02");
        _textTitleSkill03.text = PlayerPrefs.GetString("TitleSkill03");
        _textTitleSkill04.text = PlayerPrefs.GetString("TitleSkill04");
        _textTitleSpell.text = PlayerPrefs.GetString("TitleSpell");

        _textDescriptionIntrinsic.text = PlayerPrefs.GetString("DescriptionIntrinsic");
        _textDescriptionSkill01.text = PlayerPrefs.GetString("DescriptionSkill01");
        _textDescriptionSkill02.text = PlayerPrefs.GetString("DescriptionSkill02");
        _textDescriptionSkill03.text = PlayerPrefs.GetString("DescriptionSkill03");
        _textDescriptionSkill04.text = PlayerPrefs.GetString("DescriptionSkill04");
        _textDescriptionSpell.text = PlayerPrefs.GetString("DescriptionSpell");

        _textManaIntrinsic.text = PlayerPrefs.GetFloat("ManaIntrinsic").ToString() + " Mana";
        _textManaSkill01.text = PlayerPrefs.GetFloat("ManaSkill01").ToString() + " Mana";
        _textManaSkill02.text = PlayerPrefs.GetFloat("ManaSkill02").ToString() + " Mana";
        _textManaSkill03.text = PlayerPrefs.GetFloat("ManaSkill03").ToString() + " Mana";
        _textManaSkill04.text = PlayerPrefs.GetFloat("ManaSkill04").ToString() + " Mana";

        _imageIntrinsic.sprite = Resources.Load<Sprite>("SkillLuffy/Intrinsic");
        _imageSkill01.sprite = Resources.Load<Sprite>("SkillLuffy/Skill01");
        _imageSkill02.sprite = Resources.Load<Sprite>("SkillLuffy/Skill02");
        _imageSkill03.sprite = Resources.Load<Sprite>("SkillLuffy/Skill03");
        _imageSkill04.sprite = Resources.Load<Sprite>("SkillLuffy/Skill04");
        _imageSpell.sprite = Resources.Load<Sprite>("SpellIcon/" + _currentSpell);

        yield return new WaitForSeconds(4.0f);

        if(this.gameObject.tag == "Player01")
        {
            _textTimeCooldownIntrinsic.text = (PlayerPrefs.GetFloat("CooldownIntrinsic") - (PlayerPrefs.GetFloat("CooldownIntrinsic") * _game.GetReducedTimePlayer01() / 100)).ToString() + " giây";
            _textTimeCooldownSkill01.text = (PlayerPrefs.GetFloat("CooldownSkill01") - (PlayerPrefs.GetFloat("CooldownSkill01") * _game.GetReducedTimePlayer01() / 100)).ToString() + " giây";
            _textTimeCooldownSkill02.text = (PlayerPrefs.GetFloat("CooldownSkill02") - (PlayerPrefs.GetFloat("CooldownSkill02") * _game.GetReducedTimePlayer01() / 100)).ToString() + " giây";
            _textTimeCooldownSkill03.text = (PlayerPrefs.GetFloat("CooldownSkill03") - (PlayerPrefs.GetFloat("CooldownSkill03") * _game.GetReducedTimePlayer01() / 100)).ToString() + " giây";
            _textTimeCooldownSkill04.text = (PlayerPrefs.GetFloat("CooldownSkill04") - (PlayerPrefs.GetFloat("CooldownSkill04") * _game.GetReducedTimePlayer01() / 100)).ToString() + " giây";
        }
        else if(this.gameObject.tag == "Player02")
        {
            _textTimeCooldownIntrinsic.text = (PlayerPrefs.GetFloat("CooldownIntrinsic") - (PlayerPrefs.GetFloat("CooldownIntrinsic") * _game.GetReducedTimePlayer02() / 100)).ToString() + " giây";
            _textTimeCooldownSkill01.text = (PlayerPrefs.GetFloat("CooldownSkill01") - (PlayerPrefs.GetFloat("CooldownSkill01") * _game.GetReducedTimePlayer02() / 100)).ToString() + " giây";
            _textTimeCooldownSkill02.text = (PlayerPrefs.GetFloat("CooldownSkill02") - (PlayerPrefs.GetFloat("CooldownSkill02") * _game.GetReducedTimePlayer02() / 100)).ToString() + " giây";
            _textTimeCooldownSkill03.text = (PlayerPrefs.GetFloat("CooldownSkill03") - (PlayerPrefs.GetFloat("CooldownSkill03") * _game.GetReducedTimePlayer02() / 100)).ToString() + " giây";
            _textTimeCooldownSkill04.text = (PlayerPrefs.GetFloat("CooldownSkill04") - (PlayerPrefs.GetFloat("CooldownSkill04") * _game.GetReducedTimePlayer02() / 100)).ToString() + " giây";
        }
        _textTimeCooldownSpell.text = PlayerPrefs.GetString("TimeCooldownSpell");
    }
    private IEnumerator TimeCooldownSkill()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            _timeCooldownIntrinsic -= 1.0f;
            _timeCooldownSkill01 -= 1.0f;
            _timeCooldownSkill02 -= 1.0f;
            _timeCooldownSkill03 -= 1.0f;
            _timeCooldownSkill04 -= 1.0f;
            _timeCooldownSpell -= 1.0f;

            _textShowTimeCooldownIntrinsic.text = _timeCooldownIntrinsic.ToString("###.#");
            _textShowTimeCooldownSkill01.text = _timeCooldownSkill01.ToString("###.#");
            _textShowTimeCooldownSkill02.text = _timeCooldownSkill02.ToString("###.#");
            _textShowTimeCooldownSkill03.text = _timeCooldownSkill03.ToString("###.#");
            _textShowTimeCooldownSkill04.text = _timeCooldownSkill04.ToString("###.#");
            _textShowTimeCooldownSpell.text = _timeCooldownSpell.ToString("###.#");
        }
    }
    IEnumerator Healing()
    {
        if (PhotonNetwork.LocalPlayer.Equals(photonView.Owner))
        {
            while (!_endGame)
            {
                yield return new WaitForSeconds(1.0f);
                float randomPositionX = Random.Range(this.transform.position.x - 0.3f, this.transform.position.x + 0.3f);
                float randomPositionY = Random.Range(this.transform.position.y - 0.5f, this.transform.position.y + 0.5f);
                if(this.gameObject.tag == "Player01")
                {
                    float _plusHealth = _game.GetHealingPlayer01();
                    float _plusMana = _game.GetRestoreManaPlayer01();
                    _game.TakePlusHealthPlayer01(_plusHealth);
                    _game.TakePlusManaPlayer01(_plusMana);
                    if(_plusHealth >= _game.GetHealthMaxPlayer01() * 5 / 100)
                    {
                        GameObject _valueTextPrefab = PhotonNetwork.Instantiate("ValueTextPrefab", new Vector2(randomPositionX, randomPositionY), Quaternion.identity);
                        _valueTextPrefab.GetComponent<PhotonView>().RPC("SetValueTextInfoRPC", RpcTarget.All, "MiniHealing", _plusHealth);
                    }

                    if(_plusMana >= _game.GetManaMaxPlayer01() * 5 / 100)
                    {
                        GameObject _valueTextPrefab = PhotonNetwork.Instantiate("ValueTextPrefab", new Vector2(randomPositionX, randomPositionY), Quaternion.identity);
                        _valueTextPrefab.GetComponent<PhotonView>().RPC("SetValueTextInfoRPC", RpcTarget.All, "MiniMana", _plusMana);
                    }
                }
                else if(this.gameObject.tag == "Player02")
                {
                    float _plusHealth = _game.GetHealingPlayer02();
                    float _plusMana = _game.GetRestoreManaPlayer02();
                    _game.TakePlusHealthPlayer02(_plusHealth);
                    _game.TakePlusManaPlayer02(_plusMana);
                    if(_plusHealth >= _game.GetHealthMaxPlayer02() * 5 / 100)
                    {
                        GameObject _valueTextPrefab = PhotonNetwork.Instantiate("ValueTextPrefab", new Vector2(randomPositionX, randomPositionY), Quaternion.identity);
                        _valueTextPrefab.GetComponent<PhotonView>().RPC("SetValueTextInfoRPC", RpcTarget.All, "MiniHealing", _plusHealth);
                    }

                    if(_plusMana >= _game.GetManaMaxPlayer02() * 5 / 100)
                    {
                        GameObject _valueTextPrefab = PhotonNetwork.Instantiate("ValueTextPrefab", new Vector2(randomPositionX, randomPositionY), Quaternion.identity);
                        _valueTextPrefab.GetComponent<PhotonView>().RPC("SetValueTextInfoRPC", RpcTarget.All, "MiniMana", _plusMana);
                    }
                }
            }
        }
    }
    [PunRPC]
    private void SetPlayerInfo()
    {
        Player player = photonView.Owner;

        _playerNameText.text = player.NickName;
        if (player.CustomProperties.TryGetValue("SelectedChampName", out object selectedChampObj) && selectedChampObj is string selectedChampName)
        {
            _champNameText.text = selectedChampName;
        }
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

    [PunRPC]
    private void ShowHitboxAttack(Vector2 newPosition, Vector2 newScale, float timeHitbox)
    {
        GameObject _attack = Instantiate(_attackPrefab, newPosition, Quaternion.identity);
        _attack.transform.localScale = newScale;

        Attack attackComponent = _attack.GetComponent<Attack>();
        if (attackComponent != null)
        {
            attackComponent.SetAttackInfo(this.gameObject.tag, timeHitbox, PhotonNetwork.LocalPlayer.Equals(_photonView.Owner));
        }
    }

    [PunRPC]
    private void ShowHitboxIntrinsic(Vector2 newPosition, Vector2 newScale, float timeHitbox)
    {
        GameObject _intrinsic = Instantiate(_intrinsicPrefab, newPosition, Quaternion.identity);
        _intrinsic.transform.localScale = newScale;

        LuffyIntrinsic intrinsicComponent = _intrinsic.GetComponent<LuffyIntrinsic>();
        if (intrinsicComponent != null)
        {
            intrinsicComponent.SetAttackInfo(this.gameObject.tag, timeHitbox, PhotonNetwork.LocalPlayer.Equals(_photonView.Owner));
        }
    }


    [PunRPC]
    private void ShowHitboxSkill01(Vector2 newPosition, Vector2 newScale, float timeHitbox)
    {
        GameObject _powerSkill01 = Instantiate(_skill01Prefab, newPosition, Quaternion.identity);
        _powerSkill01.transform.localScale = newScale;

        LuffySkill01 powerSkill01Component = _powerSkill01.GetComponent<LuffySkill01>();
        if (powerSkill01Component != null)
        {
            powerSkill01Component.SetAttackInfo(this.gameObject.tag, timeHitbox, PhotonNetwork.LocalPlayer.Equals(_photonView.Owner));
        }
    }
    [PunRPC]
    private void ShowHitboxSkill02(Vector2 newPosition, Vector2 newScale, float timeHitbox)
    {
        GameObject _powerSkill02 = Instantiate(_skill02Prefab, newPosition, Quaternion.identity);
        _powerSkill02.transform.localScale = newScale;

        LuffySkill02 powerSkill02Component = _powerSkill02.GetComponent<LuffySkill02>();
        if (powerSkill02Component != null)
        {
            powerSkill02Component.SetAttackInfo(this.gameObject.tag, timeHitbox, PhotonNetwork.LocalPlayer.Equals(_photonView.Owner));
        }
    }
    [PunRPC]
    private void ShowHitboxSkill03(Vector2 newPosition, Vector2 newScale, float timeHitbox)
    {
        GameObject _powerSkill03 = Instantiate(_skill03Prefab, newPosition, Quaternion.identity);
        _powerSkill03.transform.localScale = newScale;

        LuffySkill03 powerSkill03Component = _powerSkill03.GetComponent<LuffySkill03>();
        if (powerSkill03Component != null)
        {
            powerSkill03Component.SetAttackInfo(this.gameObject.tag, timeHitbox, PhotonNetwork.LocalPlayer.Equals(_photonView.Owner));
        }
    }
    [PunRPC]
    private void ShowHitboxSkill04(Vector2 newPosition, Vector2 newScale, float timeHitbox)
    {
        GameObject _powerSkill04 = Instantiate(_skill04Prefab, newPosition, Quaternion.identity);
        _powerSkill04.transform.localScale = newScale;

        LuffySkill04 powerSkill04Component = _powerSkill04.GetComponent<LuffySkill04>();
        if (powerSkill04Component != null)
        {
            powerSkill04Component.SetAttackInfo(this.gameObject.tag, timeHitbox, PhotonNetwork.LocalPlayer.Equals(_photonView.Owner));
        }
    }
    [PunRPC]
    private void SetDustMove(Vector2 newPosition, Vector2 newScale)
    {
        GameObject _dustMove = Instantiate(_dustMovePrefab, newPosition, Quaternion.identity);
        _dustMove.transform.localScale = newScale;
    }

    [PunRPC]
    private void SetSpell(string spell)
    {
        if(spell == "Health")
        {
            _objectHealthEffect.SetActive(true);
        }
        else if(spell == "Mana")
        {
            _objectManaEffect.SetActive(true);
        }
        else if(spell == "Dash")
        {
        }
        else if(spell == "TripleJump")
        {
        }
        else if(spell == "Resistance")
        {
            _objectResistance.SetActive(true);
        }
        else if(spell == "Shield")
        {
            _objectShield.SetActive(true);
        }
        else if(spell == "FinishingBlow")
        {
            _objectFinishingBlow.SetActive(true);
        }
        else if(spell == "BloodThirsty")
        {
            _objectBloodThirsty.SetActive(true);
        }
        else if(spell == "StandardDamage")
        {
            _objectStandardDamage.SetActive(true);
        }
    }

    [PunRPC]
    private void UnSetSpell(string spell)
    {
        if(spell == "Health")
        {
            _objectHealthEffect.SetActive(false);
        }
        else if(spell == "Mana")
        {
            _objectManaEffect.SetActive(false);
        }
        else if(spell == "Dash")
        {
        }
        else if(spell == "TripleJump")
        {
        }
        else if(spell == "Resistance")
        {
            _objectResistance.SetActive(false);
        }
        else if(spell == "Shield")
        {
            _objectShield.SetActive(false);
        }
        else if(spell == "FinishingBlow")
        {
            _objectFinishingBlow.SetActive(false);
        }
        else if(spell == "BloodThirsty")
        {
            _objectBloodThirsty.SetActive(false);
        }
        else if(spell == "StandardDamage")
        {
            _objectStandardDamage.SetActive(false);
        }
    }

        
    [PunRPC]
    private IEnumerator DetroyObject(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj);
    }
}

