using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Text;
using System.Linq;

public class Game : MonoBehaviourPunCallbacks
{
    private const string ResolutionIndexKey = "ResolutionIndex";
    private const string FullscreenKey = "Fullscreen";
    private const string HiddenNameKey = "HiddenName";
    private const string HiddenPropertiesKey = "HiddenProperties";
    private const string HiddenNameChampKey = "HiddenNameChamp";
    private const string HiddenDamageKey = "HiddenDamage";
    private const string HiddenChatKey = "HiddenChat";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SoundVolumeKey = "SoundVolume";
    private const string VoiceVolumeKey = "VoiceVolume";

    private PhotonView _photonView;
    public static Game Instance;
    private GameObject localPlayer;
    public GameObject cameraPosition;
    public Transform[] backgroundLayers; 
    public float[] parallaxSpeeds; 
    private Transform playerTransform;
    private Vector3 previousCameraPosition;
    private int playerInRoom = 0;
    private bool canZoomCamera = true;

    private float maxX = 6.1f;
    private float minX = -6.1f;
    private float maxY = 1.5f;
    private float minY = -0.8f;
    private Dictionary<Player, string> playerChampNames = new Dictionary<Player, string>();

    public GameObject playerObjectPrefab;
    private Dictionary<Player, GameObject> playerObjectMap = new Dictionary<Player, GameObject>();
    private Vector2[] vector2Player;

    public TMP_InputField _chatInput;
    public Button _chatButton;
    public ScrollRect _chatListView;
    public Transform _chatListContent;
    public GameObject _contentChatPrefab;
    private List<GameObject> _chatMessages = new List<GameObject>();
    private Dictionary<Player, string> playerChampMap = new Dictionary<Player, string>();
    private int layerPlayer;

    private float _damagePlayer01;
    private float _defensePlayer01;
    private float _healthMaxPlayer01;
    private float _healthPlayer01;
    private float _healingPlayer01;
    private float _manaMaxPlayer01;
    private float _manaPlayer01;
    private float _restoreManaPlayer01;
    private float _criticalPlayer01;
    private float _criticalDamagePlayer01;
    private float _dodgePlayer01;
    private float _bloodsuckingPlayer01;
    private float _reducedHealingPlayer01;
    private float _speedPlayer01;
    private float _resistancePlayer01;
    private float _penetratePlayer01;
    private float _attackSpeedPlayer01;
    private float _reducedTimePlayer01;
    private float _stunPlayer01;

    private float _damagePlayer02;
    private float _defensePlayer02;
    private float _healthMaxPlayer02;
    private float _healthPlayer02;
    private float _healingPlayer02;
    private float _manaMaxPlayer02;
    private float _manaPlayer02;
    private float _restoreManaPlayer02;
    private float _criticalPlayer02;
    private float _criticalDamagePlayer02;
    private float _dodgePlayer02;
    private float _bloodsuckingPlayer02;
    private float _reducedHealingPlayer02;
    private float _speedPlayer02;
    private float _resistancePlayer02;
    private float _penetratePlayer02;
    private float _attackSpeedPlayer02;
    private float _reducedTimePlayer02;
    private float _stunPlayer02;

    private float _hitWaitTimePlayer01;
    private float _hitWaitTimePlayer02;    

    public TextMeshProUGUI _damagePlayer01Text;
    public TextMeshProUGUI _defensePlayer01Text;
    public TextMeshProUGUI _healthMaxPlayer01Text;
    public TextMeshProUGUI _healthPlayer01Text;
    public TextMeshProUGUI _healingPlayer01Text;
    public TextMeshProUGUI _manaMaxPlayer01Text;
    public TextMeshProUGUI _manaPlayer01Text;
    public TextMeshProUGUI _restoreManaPlayer01Text;
    public TextMeshProUGUI _criticalPlayer01Text;
    public TextMeshProUGUI _criticalDamagePlayer01Text;
    public TextMeshProUGUI _dodgePlayer01Text;
    public TextMeshProUGUI _bloodsuckingPlayer01Text;
    public TextMeshProUGUI _reducedHealingPlayer01Text;
    public TextMeshProUGUI _speedPlayer01Text;
    public TextMeshProUGUI _resistancePlayer01Text;
    public TextMeshProUGUI _penetratePlayer01Text;
    public TextMeshProUGUI _attackSpeedPlayer01Text;
    public TextMeshProUGUI _reducedTimePlayer01Text;
    public TextMeshProUGUI _stunPlayer01Text;

    public TextMeshProUGUI _damagePlayer02Text;
    public TextMeshProUGUI _defensePlayer02Text;
    public TextMeshProUGUI _healthMaxPlayer02Text;
    public TextMeshProUGUI _healthPlayer02Text;
    public TextMeshProUGUI _healingPlayer02Text;
    public TextMeshProUGUI _manaMaxPlayer02Text;
    public TextMeshProUGUI _manaPlayer02Text;
    public TextMeshProUGUI _restoreManaPlayer02Text;
    public TextMeshProUGUI _criticalPlayer02Text;
    public TextMeshProUGUI _criticalDamagePlayer02Text;
    public TextMeshProUGUI _dodgePlayer02Text;
    public TextMeshProUGUI _bloodsuckingPlayer02Text;
    public TextMeshProUGUI _reducedHealingPlayer02Text;
    public TextMeshProUGUI _speedPlayer02Text;
    public TextMeshProUGUI _resistancePlayer02Text;
    public TextMeshProUGUI _penetratePlayer02Text;
    public TextMeshProUGUI _attackSpeedPlayer02Text;
    public TextMeshProUGUI _reducedTimePlayer02Text;
    public TextMeshProUGUI _stunPlayer02Text;

    public Button _buttonSettting;
    public Button _buttonCloseSetting;
    public Button _buttonSaveSetting;
    public Button _buttonLeaveRoom;
    public GameObject _settingPanel;

    public TMP_Dropdown _dropdownResolution;
    public Toggle _toggleFullscreen;
    public Toggle _toggleHiddenName;
    public Toggle _toggleHiddenProperties;
    public Toggle _toggleHiddenNameChamp;
    public Toggle _toggleHiddenDamage;
    public Toggle _toggleHiddenChat;
    public Slider _sliderMusic;
    public Slider _sliderSound;
    public Slider _sliderVoice;
    public AudioSource _backgroundMusic;

    private Resolution[] _resolutions;

    private List<GameObject> namePlayerObjects;
    private List<GameObject> nameChampObjects;
    private List<GameObject> dameObjects;
    private List<GameObject> propertiesObjects;
    private List<GameObject> chatObjects;

    public HealthBar healthBarPlayer01;
    public DelayedHealthBar healthDelayBarPlayer01;
    public HealthBar healthBarPlayer02;
    public DelayedHealthBar healthDelayBarPlayer02;

    public HealthBar manaBarPlayer01;
    public DelayedHealthBar manaDelayBarPlayer01;
    public HealthBar manaBarPlayer02;
    public DelayedHealthBar manaDelayBarPlayer02;
    public Image _fillHealthBarPlayer01;
    public Image _fillGealthDelayBarPlayer01;
    public Image _fillHealthBarPlayer02;
    public Image _fillGealthDelayBarPlayer02;

    public Image _imageThumbnailPlayer01;
    public Image _imageThumbnailPlayer02;

    private TextMeshProUGUI _pingText;

    public List<NarutoClone> activeClonesPlayer01 = new List<NarutoClone>();
    public List<NarutoClone> activeClonesPlayer02 = new List<NarutoClone>();
    public float fakeDamagePlayer01 = 0f;
    public float fakeDamagePlayer02 = 0f;
    public NarutoClone _cloneHitPlayer01ThisFrame;
    public NarutoClone _cloneHitPlayer02ThisFrame;

    public void RegisterCloneHit(NarutoClone clone, string tag)
    {
        if (tag == "Player01")
        {
            _cloneHitPlayer01ThisFrame = clone;
        }
        else if (tag == "Player02")
        {
            _cloneHitPlayer02ThisFrame = clone;
        }
    }
    private TextMeshProUGUI _fpsText;
    private TextMeshProUGUI _timeText;
    private float _timeGame;
    private  bool _endGame = false;
    public Button _buttonLeaveGameWin;
    public Button _buttonLeaveGameLose;
    public GameObject _UIWin;
    public GameObject _UILose;

    public GameObject _dummy;

    private AudioSource[] _voiceSound; 
    private AudioSource[] _effectSound;

    private GameObject[] _voiceSoundGameObject; 
    private GameObject[] _effectSoundGameObject;

    private void Awake()
    {
        Instance = this;
    }

    public void SetLocalPlayer(GameObject player)
    {
        localPlayer = player;
    }

    public GameObject GetLocalPlayer()
    {
        return localPlayer;
    }

    void Start()
    {
        _photonView = photonView;
        localPlayer = Game.Instance.GetLocalPlayer();

        _chatInput.onEndEdit.AddListener(OnChatInputEndEdit);
        _chatButton.onClick.AddListener(SendChatMessage);

        _buttonSettting.onClick.AddListener(ShowSettingPanel);
        _buttonCloseSetting.onClick.AddListener(HiddenSettingPanel);

        previousCameraPosition = Camera.main.transform.position;

        SetNameChamp();
        SpawnPlayers();
        SendNoticationStartGame();
        HiddenNamePlayer();
        HiddenNameChamp();
        StartCoroutine(HiddenChat());
        HiddenProperties();
        layerPlayer = LayerMask.NameToLayer("Player");
        Physics2D.IgnoreLayerCollision(layerPlayer, layerPlayer);

        List<Resolution> validResolutions = new List<Resolution>();
        Resolution[] allResolutions = Screen.resolutions;
        HashSet<string> uniqueResolutions = new HashSet<string>();

        foreach (Resolution res in allResolutions)
        {
            if (Mathf.Approximately(res.width / (float)res.height, 16f / 9f))
            {
                string resolutionString = $"{res.width} x {res.height}";

                if (!uniqueResolutions.Contains(resolutionString))
                {
                    uniqueResolutions.Add(resolutionString);
                    validResolutions.Add(res);
                }
            }
        }

        _resolutions = validResolutions.ToArray();

        _dropdownResolution.ClearOptions();
        List<string> resolutionOptions = new List<string>();
        foreach (Resolution res in validResolutions)
        {
            resolutionOptions.Add($"{res.width} x {res.height}");
        }
        _dropdownResolution.AddOptions(resolutionOptions);

        // _sliderSound.value = 0;
        // _sliderVoice.value = 0;

        // _voiceSound.ToList().ForEach(sound => sound.volume = 0);
        // _effectSound.ToList().ForEach(sound => sound.volume = 0);

        // // _voiceSoundGameObject.ToList().ForEach(sound => sound.SetActive(false));
        // // _effectSoundGameObject.ToList().ForEach(sound => sound.SetActive(false));

        LoadSettings();

        _sliderMusic.onValueChanged.AddListener((value) => ChangeSliderMusic());
        ChangeSliderMusic();

        _buttonSaveSetting.onClick.AddListener(SaveSettings);
        _buttonLeaveRoom.onClick.AddListener(LeaveLobby);
        _buttonLeaveGameLose.onClick.AddListener(LeaveLobby);
        _buttonLeaveGameWin.onClick.AddListener(LeaveLobby);

        _pingText = GameObject.Find("PingText").GetComponent<TextMeshProUGUI>();
        _fpsText = GameObject.Find("FPSText").GetComponent<TextMeshProUGUI>();
        _timeText = GameObject.Find("TimeText").GetComponent<TextMeshProUGUI>();

        StartCoroutine(SetMaxBar());
        StartCoroutine(DelayedCameraFollow());
        StartCoroutine(MaxHhealthManaPlayer());
        if(PlayerPrefs.GetString("Scene") == "ComputerLobby")
        {
            _dummy.SetActive(true);
            _imageThumbnailPlayer02.sprite = Resources.Load<Sprite>("Thumbnail/ThumbnailDummy");
        }
    }

    void Update()
    {
        CameraStartZoom();
        ParallaxBackground();
        UpdateHitTimeWait();
        HiddenDamage();
        _voiceSound = GameObject.FindGameObjectsWithTag("VoiceSound").Select(go => go.GetComponent<AudioSource>()).ToArray();
        _effectSound = GameObject.FindGameObjectsWithTag("EffectSound").Select(go => go.GetComponent<AudioSource>()).ToArray();
        _voiceSoundGameObject = GameObject.FindGameObjectsWithTag("VoiceSound").ToArray();
        _effectSoundGameObject = GameObject.FindGameObjectsWithTag("EffectSound").ToArray();
        _voiceSound.ToList().ForEach(sound => sound.volume = _sliderVoice.value);
        _effectSound.ToList().ForEach(sound => sound.volume = _sliderSound.value);

    }

    void LateUpdate()
    {
        _cloneHitPlayer01ThisFrame = null;
        _cloneHitPlayer02ThisFrame = null;
    }

    IEnumerator MaxHhealthManaPlayer()
    {
        yield return new WaitForSeconds(5.0f);
        while (!_endGame)
        {
            if(_healthPlayer01 >= _healthMaxPlayer01)
            {
                _healthPlayer01 = _healthMaxPlayer01;
            }
            if(_healthPlayer02 >= _healthMaxPlayer02)
            {
                _healthPlayer02 = _healthMaxPlayer02;
            }
            if(_healthPlayer01 <= 0)
            {
                _healthPlayer01 = 0;
                if(PlayerPrefs.GetString("Scene") != "ComputerLobby")
                {
                    _endGame = true;
                    if (playerInRoom == 0)
                    {
                        _UILose.SetActive(true);
                    }
                    if (playerInRoom == 1)
                    {
                        _UIWin.SetActive(true);
                    }
                }
            }
            if(_healthPlayer02 <= 0)
            {
                _healthPlayer02 = 0;
                if(PlayerPrefs.GetString("Scene") != "ComputerLobby")
                {
                    _endGame = true;
                    if (playerInRoom == 0)
                    {
                        _UIWin.SetActive(true);
                    }
                    if (playerInRoom == 1)
                    {
                        _UILose.SetActive(true);
                    }
                }
            }

            if(_manaPlayer01 >= _manaMaxPlayer01)
            {
                _manaPlayer01 = _manaMaxPlayer01;
            }
            if(_manaPlayer02 >= _manaMaxPlayer02)
            {
                _manaPlayer02 = _manaMaxPlayer02;
            }
            if(_manaPlayer01 <= 0 && !_endGame)
            {
                _manaPlayer01 = 0;
            }
            if(_manaPlayer02 <= 0 && !_endGame)
            {
                _manaPlayer02 = 0;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void FixedUpdate()
    {
        Configuration();
    }

    void Configuration()
    {
        float ping = PhotonNetwork.GetPing();
        _pingText.text = "Ping: " + ping.ToString() + "ms";
        float fps = 1.0f / Time.deltaTime;
        _fpsText.text = "FPS: " + fps.ToString("###,###.#");
        _timeGame += Time.deltaTime;
        int minutes = Mathf.FloorToInt(_timeGame / 60F);
        int seconds = Mathf.FloorToInt(_timeGame - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        _timeText.text = "Time: " + niceTime;
    }

    void ShowSettingPanel()
    {
        _settingPanel.SetActive(true);
    }

    void HiddenSettingPanel()
    {
        _settingPanel.SetActive(false);
    }

    void LoadSettings()
    {
        int resolutionIndex = PlayerPrefs.GetInt(ResolutionIndexKey);
        _dropdownResolution.value = resolutionIndex;
        _toggleFullscreen.isOn = PlayerPrefs.GetInt(FullscreenKey, 1) == 1;
        _toggleHiddenName.isOn = PlayerPrefs.GetInt(HiddenNameKey) == 1;
        _toggleHiddenProperties.isOn = PlayerPrefs.GetInt(HiddenPropertiesKey) == 1;
        _toggleHiddenNameChamp.isOn = PlayerPrefs.GetInt(HiddenNameChampKey) == 1;
        _toggleHiddenDamage.isOn = PlayerPrefs.GetInt(HiddenDamageKey) == 1;
        _toggleHiddenChat.isOn = PlayerPrefs.GetInt(HiddenChatKey) == 1;
        _sliderMusic.value = PlayerPrefs.GetFloat(MusicVolumeKey, 0.3f);
        _sliderSound.value = PlayerPrefs.GetFloat(SoundVolumeKey, 0.5f);
        _sliderVoice.value = PlayerPrefs.GetFloat(VoiceVolumeKey, 0.5f);
        _backgroundMusic.volume = PlayerPrefs.GetFloat(MusicVolumeKey);

        SetResolution(resolutionIndex);
    }

    void SetResolution(int resolutionIndex)
    {
        if (resolutionIndex >= 0 && resolutionIndex < _resolutions.Length)
        {
            Resolution selectedResolution = _resolutions[resolutionIndex];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, _toggleFullscreen.isOn);
        }
    }

    void ChangeSliderMusic()
    {
        _backgroundMusic.volume = _sliderMusic.value;
    }

    void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionIndex", _dropdownResolution.value);
        PlayerPrefs.SetInt("Fullscreen", _toggleFullscreen.isOn ? 1 : 0);
        PlayerPrefs.SetInt("HiddenName", _toggleHiddenName.isOn ? 1 : 0);
        PlayerPrefs.SetInt("HiddenProperties", _toggleHiddenProperties.isOn ? 1 : 0);
        PlayerPrefs.SetInt("HiddenNameChamp", _toggleHiddenNameChamp.isOn ? 1 : 0);
        PlayerPrefs.SetInt("HiddenDamage", _toggleHiddenDamage.isOn ? 1 : 0);
        PlayerPrefs.SetInt("HiddenChat", _toggleHiddenChat.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("MusicVolume", _sliderMusic.value);
        PlayerPrefs.SetFloat("SoundVolume", _sliderSound.value);
        PlayerPrefs.SetFloat("VoiceVolume", _sliderVoice.value);

        _voiceSound.ToList().ForEach(sound => sound.volume = _sliderVoice.value);
        _effectSound.ToList().ForEach(sound => sound.volume = _sliderSound.value);
        
        PlayerPrefs.Save();
        LoadSettings();
        HiddenNamePlayer();
        HiddenNameChamp();
        HiddenProperties();
        StartCoroutine(HiddenChat());
        HiddenSettingPanel();
    }

    void HiddenNamePlayer()
    {
        if (namePlayerObjects == null)
        {
            namePlayerObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("NamePlayer"));
        }

        bool hiddenName = PlayerPrefs.GetInt(HiddenNameKey, 0) == 1;

        foreach (GameObject go in namePlayerObjects)
        {
            go.SetActive(!hiddenName);
        }
    }

    void HiddenNameChamp()
    {
        if (nameChampObjects == null)
        {
            nameChampObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("NameChamp"));
        }

        bool hiddenNameChamp = PlayerPrefs.GetInt(HiddenNameChampKey, 0) == 1;

        foreach (GameObject go in nameChampObjects)
        {
            go.SetActive(!hiddenNameChamp);
        }
    }

    void HiddenDamage()
    {
        if (dameObjects == null)
        {
            dameObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("DameValue"));
        }

        bool hiddenDameObjects = PlayerPrefs.GetInt(HiddenDamageKey, 0) == 1;

        foreach (GameObject go in dameObjects)
        {
            go.SetActive(!hiddenDameObjects);
        }
    }

    void HiddenProperties()
    {
        if (propertiesObjects == null)
        {
            propertiesObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Properties"));
        }

        bool hiddenProperties = PlayerPrefs.GetInt(HiddenPropertiesKey, 0) == 1;

        foreach (GameObject go in propertiesObjects)
        {
            go.SetActive(!hiddenProperties);
        }
    }

    IEnumerator HiddenChat()
    {
        yield return new WaitForSeconds(1.0f);
        if (chatObjects == null)
        {
            chatObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Chat"));
        }

        bool hiddenChat = PlayerPrefs.GetInt(HiddenChatKey, 0) == 1;

        foreach (GameObject go in chatObjects)
        {
            go.SetActive(!hiddenChat);
        }
    }

    private void ParallaxBackground()
    {
        Vector3 playerDelta = Camera.main.transform.position - previousCameraPosition;
        for (int i = 0; i < backgroundLayers.Length; i++)
        {
            float parallaxX =+ playerDelta.x * parallaxSpeeds[i] * Time.deltaTime;
            float parallaxY =+ playerDelta.y * parallaxSpeeds[i] * Time.deltaTime;

            Vector3 backgroundTargetPosition = backgroundLayers[i].position + new Vector3(parallaxX, parallaxY, 0f);
            backgroundLayers[i].position = Vector3.Lerp(backgroundLayers[i].position, backgroundTargetPosition, Time.deltaTime);
        }

        previousCameraPosition = Camera.main.transform.position;
    }

    private void CameraStartZoom()
    {
        if (!canZoomCamera)
            return;

        if (localPlayer != null)
        {
            GameObject opponent = GetOpponentPlayer();
            if (opponent != null)
            {
                // Calculate target midpoint and zoom size based on distance
                Vector3 midpoint = (localPlayer.transform.position + opponent.transform.position) / 2f;
                float distance = Vector2.Distance(localPlayer.transform.position, opponent.transform.position);
                float targetSize = 5.0f;
                if (distance > 4f)
                {
                    targetSize = 5.0f + (distance - 4f) * 0.3f;
                }
                targetSize = Mathf.Clamp(targetSize, 5.0f, 8.0f);

                // Lerp zoom and position towards midpoint
                Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetSize, 1.0f * Time.deltaTime);
                
                // Clamp target position
                float sizeDiff = Camera.main.orthographicSize - 5.0f;
                float aspect = Camera.main.aspect;
                float adjMaxX = maxX - sizeDiff * aspect;
                float adjMinX = minX + sizeDiff * aspect;
                float adjMaxY = maxY - sizeDiff;
                float adjMinY = minY + sizeDiff;

                if (adjMinX > adjMaxX)
                {
                    float center = (maxX + minX) / 2f;
                    adjMinX = center;
                    adjMaxX = center;
                }
                if (adjMinY > adjMaxY)
                {
                    float center = (maxY + minY) / 2f;
                    adjMinY = center;
                    adjMaxY = center;
                }

                float clampedX = Mathf.Clamp(midpoint.x, adjMinX, adjMaxX);
                float clampedY = Mathf.Clamp(midpoint.y, adjMinY, adjMaxY);
                Vector3 targetPos = new Vector3(clampedX, clampedY, Camera.main.transform.position.z);
                
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPos, 2.0f * Time.deltaTime);

                // Check if we reached the target zoom size and position
                if (Mathf.Abs(Camera.main.orthographicSize - targetSize) < 0.05f && Vector3.Distance(Camera.main.transform.position, targetPos) < 0.1f)
                {
                    canZoomCamera = false;
                    Camera.main.orthographicSize = targetSize;
                    Camera.main.transform.position = targetPos;
                }
            }
            else
            {
                // Fallback if no opponent is present
                Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, 5.0f, 1.0f * Time.deltaTime);
                Vector3 targetPos = new Vector3(localPlayer.transform.position.x, localPlayer.transform.position.y, Camera.main.transform.position.z);
                
                // Clamp targetPos
                float clampedX = Mathf.Clamp(targetPos.x, minX, maxX);
                float clampedY = Mathf.Clamp(targetPos.y, minY, maxY);
                Vector3 finalTargetPos = new Vector3(clampedX, clampedY, Camera.main.transform.position.z);
                
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, finalTargetPos, 2.0f * Time.deltaTime);

                if (Mathf.Abs(Camera.main.orthographicSize - 5.0f) < 0.05f && Vector3.Distance(Camera.main.transform.position, finalTargetPos) < 0.1f)
                {
                    canZoomCamera = false;
                    Camera.main.orthographicSize = 5.0f;
                    Camera.main.transform.position = finalTargetPos;
                }
            }
        }
    }

    private IEnumerator DelayedCameraFollow()
    {
        yield return new WaitForSeconds(4.0f);
        while (true)
        {
            if (localPlayer != null)
            {
                Vector3 targetPosition;
                GameObject opponent = GetOpponentPlayer();
                if (opponent != null)
                {
                    Vector3 midpoint = (localPlayer.transform.position + opponent.transform.position) / 2f;
                    targetPosition = new Vector3(midpoint.x, midpoint.y, Camera.main.transform.position.z);

                    // Smoothly adjust zoom based on player distance if initial zoom is done
                    if (!canZoomCamera)
                    {
                        float distance = Vector2.Distance(localPlayer.transform.position, opponent.transform.position);
                        float targetSize = 5.0f;
                        if (distance > 4f)
                        {
                            targetSize = 5.0f + (distance - 4f) * 0.3f;
                        }
                        targetSize = Mathf.Clamp(targetSize, 5.0f, 8.0f);
                        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetSize, 2.0f * Time.deltaTime);
                    }
                }
                else
                {
                    targetPosition = new Vector3(localPlayer.transform.position.x, localPlayer.transform.position.y, Camera.main.transform.position.z);
                    if (!canZoomCamera)
                    {
                        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 5.0f, 2.0f * Time.deltaTime);
                    }
                }

                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, 3.0f * Time.deltaTime);
            }

            // Adjust clamp boundaries based on the current camera orthographicSize to prevent showing out of bounds area
            float sizeDiff = Camera.main.orthographicSize - 5.0f;
            float aspect = Camera.main.aspect;
            float adjMaxX = maxX - sizeDiff * aspect;
            float adjMinX = minX + sizeDiff * aspect;
            float adjMaxY = maxY - sizeDiff;
            float adjMinY = minY + sizeDiff;

            if (adjMinX > adjMaxX)
            {
                float center = (maxX + minX) / 2f;
                adjMinX = center;
                adjMaxX = center;
            }
            if (adjMinY > adjMaxY)
            {
                float center = (maxY + minY) / 2f;
                adjMinY = center;
                adjMaxY = center;
            }

            if (cameraPosition.transform.position.x > adjMaxX)
            {
                cameraPosition.transform.position = new Vector3(adjMaxX, cameraPosition.transform.position.y, cameraPosition.transform.position.z);
            }
            if (cameraPosition.transform.position.x < adjMinX)
            {
                cameraPosition.transform.position = new Vector3(adjMinX, cameraPosition.transform.position.y, cameraPosition.transform.position.z);
            }

            if (cameraPosition.transform.position.y > adjMaxY)
            {
                cameraPosition.transform.position = new Vector3(cameraPosition.transform.position.x, adjMaxY, cameraPosition.transform.position.z);
            }
            if (cameraPosition.transform.position.y < adjMinY)
            {
                cameraPosition.transform.position = new Vector3(cameraPosition.transform.position.x, adjMinY, cameraPosition.transform.position.z);
            }

            yield return null;
        }
    }



    private GameObject GetOpponentPlayer()
    {
        if (PlayerPrefs.GetString("Scene") == "ComputerLobby")
        {
            return _dummy;
        }
        else
        {
            if (localPlayer != null)
            {
                string opponentTag = localPlayer.CompareTag("Player01") ? "Player02" : "Player01";
                GameObject[] opponents = GameObject.FindGameObjectsWithTag(opponentTag);
                foreach (GameObject opponent in opponents)
                {
                    if (opponent.GetComponent<NarutoClone>() == null)
                    {
                        return opponent;
                    }
                }
            }
        }
        return null;
    }

    private void SetNameChamp()
    {
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            Player player = players[i];
            ExitGames.Client.Photon.Hashtable CustomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
            if (CustomProperties.ContainsKey("PlayerData"))
            {
                foreach (var key in ((Dictionary<string, ExitGames.Client.Photon.Hashtable>)CustomProperties["PlayerData"]).Keys)
                {
                    ExitGames.Client.Photon.Hashtable playerData = (ExitGames.Client.Photon.Hashtable)((Dictionary<string, ExitGames.Client.Photon.Hashtable>)CustomProperties["PlayerData"])[key];
                    if (playerData["PlayerName"].ToString() == player.NickName)
                    {
                        playerChampMap[player] = playerData["ChampName"].ToString();
                        playerChampNames[player] = playerData["ChampName"].ToString();
                    }
                }
            }
        }
    }

    private void SpawnPlayers()
    {
        vector2Player = new Vector2[4];
        vector2Player[0] = new Vector2(-6, -4);
        vector2Player[1] = new Vector2(6, -4);
        vector2Player[2] = new Vector2(-10, -4);
        vector2Player[3] = new Vector2(10, -4);

        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            Player player = players[i];

            if (player.Equals(PhotonNetwork.LocalPlayer))
            {
                Vector2 spawnPosition = vector2Player[i];

                GameObject playerObject = PhotonNetwork.Instantiate(playerChampNames[player] + "Prefab", spawnPosition, Quaternion.identity);
                
                Game.Instance.SetLocalPlayer(playerObject);

                playerTransform = Game.Instance.GetLocalPlayer().transform; 
                playerObjectMap[player] = playerObject;

                if (i % 2 == 0)
                {
                    playerObject.transform.localScale = new Vector2(1, 1);
                }
                else
                {
                    playerObject.transform.localScale = new Vector2(-1, 1);
                }
                photonView.RPC("SetPlayerTag", RpcTarget.AllBuffered, playerObject.GetPhotonView().ViewID, "Player0" + (i + 1).ToString());
                photonView.RPC("SetThumbnail", RpcTarget.All, i, playerChampNames[player]);
                playerInRoom = i;
            }
            else
            {
                if(i == 0)
                {
                    _fillHealthBarPlayer01.color = new Color32(255, 0, 0, 255);
                    _fillGealthDelayBarPlayer01.color = new Color32(255, 100, 0, 255);
                }
                else
                {
                    _fillHealthBarPlayer02.color = new Color32(255, 0, 0, 255);
                    _fillGealthDelayBarPlayer02.color = new Color32(255, 100, 0, 255);
                }
            }
            
        }
    }

    [PunRPC]
    private void SetThumbnail(int idPlayer, string nameChamp)
    {
        if(idPlayer == 0)
            _imageThumbnailPlayer01.sprite = Resources.Load<Sprite>("Thumbnail/Thumbnail" + nameChamp);
        else
            _imageThumbnailPlayer02.sprite = Resources.Load<Sprite>("Thumbnail/Thumbnail" + nameChamp);
    }

    [PunRPC]
    private void SetPlayerTag(int viewID, string tag)
    {
        GameObject playerObject = PhotonView.Find(viewID).gameObject;
        playerObject.tag = tag;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _photonView.RPC("SendNoticationLeaveRoom", RpcTarget.All, otherPlayer);
    }

    private void OnChatInputEndEdit(string text)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                SendChatMessage();
            }

            StartCoroutine(SelectChatInputNextFrame());
        }
    }

    private void SendChatMessage()
    {
        string message = _chatInput.text;
        if (!string.IsNullOrWhiteSpace(message))
        {
            _photonView.RPC("AddChatMessage", RpcTarget.All, message, PhotonNetwork.LocalPlayer);
            _chatInput.text = "";
            _chatInput.Select();
        }
    }

    [PunRPC]
    private void AddChatMessage(string message, Player sendingPlayer)
    {
        GameObject messageObject = Instantiate(_contentChatPrefab, _chatListContent);
        TextMeshProUGUI messageText = messageObject.GetComponentInChildren<TextMeshProUGUI>();

        string playerName = sendingPlayer.NickName;
        string champName = "No Champ";
        if (playerChampMap.TryGetValue(sendingPlayer, out string selectedChampName))
        {
            champName = selectedChampName;
        }

        if (message.Length > 40)
        {
            message = message.Substring(0, 40) + "...";
        }

        // Color whiteColor = new Color(1.0f, 1.0f, 1.0f);
        // messageText.color = whiteColor;

        messageText.text = $"{playerName} ({champName}): {message}";

        _chatMessages.Add(messageObject);

        StartCoroutine(DelayedScrollToBottom());
    }

    private void SendNoticationStartGame()
    {
        GameObject messageObject = Instantiate(_contentChatPrefab, _chatListContent);
        TextMeshProUGUI messageText = messageObject.GetComponentInChildren<TextMeshProUGUI>();

        Color orangeColor = new Color(0.227451f, 0.4470588f, 0.3372549f);
        messageText.color = orangeColor;

        messageText.text = $"Hệ thống: Trận đấu đã được bắt đầu!!";

        _chatMessages.Add(messageObject);

        StartCoroutine(DelayedScrollToBottom());
    }

    [PunRPC]
    private void SendNoticationLeaveRoom(Player ortherPlayer)
    {
        GameObject messageObject = Instantiate(_contentChatPrefab, _chatListContent);
        TextMeshProUGUI messageText = messageObject.GetComponentInChildren<TextMeshProUGUI>();

        Color redColor = new Color(1.0f, 0.0f, 0.0f);
        messageText.color = redColor;

        messageText.text = $"Hệ thống: {ortherPlayer.NickName} đã rời phòng!!";

        _chatMessages.Add(messageObject);

        StartCoroutine(DelayedScrollToBottom());
    }

    private IEnumerator DelayedScrollToBottom()
    {
        yield return new WaitForSeconds(0.1f);
        _chatListView.verticalNormalizedPosition = 0f;
    }

    private IEnumerator SelectChatInputNextFrame()
    {
        yield return null;

        _chatInput.Select();
        _chatInput.ActivateInputField();
    }

    private void LeaveLobby()
    {
        PhotonNetwork.LeaveRoom();
        StartCoroutine(DelayedSceneChange());
    }

    private IEnumerator DelayedSceneChange()
    {
        yield return new WaitForSeconds(1f);
        PhotonNetwork.LoadLevel("Menu");
    }

    [PunRPC]
    public void UpdatePropertiesPlayer01(float damage, float defense, float healthMax ,float health, float healing, float manaMax, float mana, float restoreMana, float critical, float criticalDamage, float dodge, float bloodsucking, float reducedHealing, float speed, float resistance, float penetrate, float attackSpeed, float reducedTime, float stun)
    {
        _damagePlayer01 = damage;
        _defensePlayer01 = defense;
        _healthMaxPlayer01 = healthMax;
        _healthPlayer01 = health;
        _healingPlayer01 = healing;
        _manaMaxPlayer01 = manaMax;
        _manaPlayer01 = mana;
        _restoreManaPlayer01 = restoreMana;
        _criticalPlayer01 = critical;
        _criticalDamagePlayer01 = criticalDamage;
        _dodgePlayer01 = dodge;
        _bloodsuckingPlayer01 = bloodsucking;
        _reducedHealingPlayer01 = reducedHealing;
        _speedPlayer01 = speed;
        _resistancePlayer01 = resistance;
        _penetratePlayer01 = penetrate;
        _attackSpeedPlayer01 = attackSpeed;
        _reducedTimePlayer01 = reducedTime;
        _stunPlayer01 = stun;
    }

    [PunRPC]
    public void UpdatePropertiesPlayer02(float damage, float defense, float healthMax ,float health, float healing, float manaMax, float mana, float restoreMana, float critical, float criticalDamage, float dodge, float bloodsucking, float reducedHealing, float speed, float resistance, float penetrate, float attackSpeed, float reducedTime, float stun)
    {
        _damagePlayer02 = damage;
        _defensePlayer02 = defense;
        _healthMaxPlayer02 = healthMax;
        _healthPlayer02 = health;
        _healingPlayer02 = healing;
        _manaMaxPlayer02 = manaMax;
        _manaPlayer02 = mana;
        _restoreManaPlayer02 = restoreMana;
        _criticalPlayer02 = critical;
        _criticalDamagePlayer02 = criticalDamage;
        _dodgePlayer02 = dodge;
        _bloodsuckingPlayer02 = bloodsucking;
        _reducedHealingPlayer02 = reducedHealing;
        _speedPlayer02 = speed;
        _resistancePlayer02 = resistance;
        _penetratePlayer02 = penetrate;
        _attackSpeedPlayer02 = attackSpeed;
        _reducedTimePlayer02 = reducedTime;
        _stunPlayer02 = stun;
    }

    public IEnumerator TakePropertiesPlayer01(float damage, float defense, float healthMax ,float health, float healing, float manaMax, float mana, float restoreMana, float critical, float criticalDamage, float dodge, float bloodsucking, float reducedHealing, float speed, float resistance, float penetrate, float attackSpeed, float reducedTime, float stun)
    {
        yield return new WaitForSeconds(1.0f);
        _photonView.RPC("UpdatePropertiesPlayer01", RpcTarget.All, damage, defense, healthMax, health, healing, manaMax, mana, restoreMana, critical, criticalDamage, dodge, bloodsucking, reducedHealing, speed, resistance, penetrate, attackSpeed, reducedTime, stun);
    }

    public IEnumerator TakePropertiesPlayer02(float damage, float defense, float healthMax ,float health, float healing, float manaMax, float mana, float restoreMana, float critical, float criticalDamage, float dodge, float bloodsucking, float reducedHealing, float speed, float resistance, float penetrate, float attackSpeed, float reducedTime, float stun)
    {
        yield return new WaitForSeconds(1.0f);
        _photonView.RPC("UpdatePropertiesPlayer02", RpcTarget.All, damage, defense, healthMax, health, healing, manaMax, mana, restoreMana, critical, criticalDamage, dodge, bloodsucking, reducedHealing, speed, resistance, penetrate, attackSpeed, reducedTime, stun);
    }

    [PunRPC]
    public void SetPropertiesPlayer01()
    {
        _damagePlayer01Text.text = GetFormattedAttributeText("Công", _damagePlayer01);
        _defensePlayer01Text.text = GetFormattedAttributeText("Phòng thủ", _defensePlayer01);
        
        float displayedHealth = _healthPlayer01;
        float displayedMaxHealth = _healthMaxPlayer01;

        _healthMaxPlayer01Text.text = displayedMaxHealth.ToString("###,###");
        _healthPlayer01Text.text = displayedHealth.ToString("###,###");
        _healingPlayer01Text.text = GetFormattedAttributeText("Hồi máu", _healingPlayer01) + "/s";
        _manaMaxPlayer01Text.text = _manaMaxPlayer01.ToString("###,###");
        _manaPlayer01Text.text = _manaPlayer01.ToString("###,###");
        _restoreManaPlayer01Text.text = GetFormattedAttributeText("Hồi mana", _restoreManaPlayer01) + "/s";
        _criticalPlayer01Text.text = GetFormattedAttributeText("Bạo kích", _criticalPlayer01) + "%";
        _criticalDamagePlayer01Text.text = GetFormattedAttributeText("Bạo thương", _criticalDamagePlayer01) + "%";
        _dodgePlayer01Text.text = GetFormattedAttributeText("Né", _dodgePlayer01) + "%";
        _bloodsuckingPlayer01Text.text = GetFormattedAttributeText("Hút máu", _bloodsuckingPlayer01) + "%";
        _reducedHealingPlayer01Text.text = GetFormattedAttributeText("Giảm hồi máu", _reducedHealingPlayer01) + "%";
        _speedPlayer01Text.text = GetFormattedAttributeText("Tốc độ", _speedPlayer01);
        _resistancePlayer01Text.text = GetFormattedAttributeText("Kháng hiệu ứng", _resistancePlayer01);
        _penetratePlayer01Text.text = GetFormattedAttributeText("Xuyên giáp", _penetratePlayer01);
        _attackSpeedPlayer01Text.text = GetFormattedAttributeText("Tốc đánh", _attackSpeedPlayer01) + "%";
        _reducedTimePlayer01Text.text = GetFormattedAttributeText("Giảm hồi chiêu", _reducedTimePlayer01) + "%";
        _stunPlayer01Text.text = GetFormattedAttributeText("Bất động", _stunPlayer01) + "%";
        

        healthDelayBarPlayer01.SetMaxHealth(displayedMaxHealth);
        healthDelayBarPlayer01.SetHealth(displayedHealth);
        manaDelayBarPlayer01.SetHealth(_manaPlayer01);

        healthBarPlayer01.SetMaxHealth(displayedMaxHealth);
        healthBarPlayer01.SetHealth(displayedHealth);

        manaBarPlayer01.SetMaxHealth(_manaMaxPlayer01);
        manaBarPlayer01.SetHealth(_manaPlayer01);
    }

    [PunRPC]
    public void SetPropertiesPlayer02()
    {
        _damagePlayer02Text.text = GetFormattedAttributeText("Công", _damagePlayer02);
        _defensePlayer02Text.text = GetFormattedAttributeText("Phòng thủ", _defensePlayer02);
        
        float displayedHealth = _healthPlayer02;
        float displayedMaxHealth = _healthMaxPlayer02;

        _healthMaxPlayer02Text.text = displayedMaxHealth.ToString("###,###");
        _healthPlayer02Text.text =  displayedHealth.ToString("###,###");
        _healingPlayer02Text.text = GetFormattedAttributeText("Hồi máu", _healingPlayer02) + "/s";
        _manaMaxPlayer02Text.text = _manaMaxPlayer02.ToString("###,###");
        _manaPlayer02Text.text = _manaPlayer02.ToString("###,###");
        _restoreManaPlayer02Text.text = GetFormattedAttributeText("Hồi mana", _restoreManaPlayer02) + "/s";
        _criticalPlayer02Text.text = GetFormattedAttributeText("Bạo kích", _criticalPlayer02) + "%";
        _criticalDamagePlayer02Text.text = GetFormattedAttributeText("Bạo thương", _criticalDamagePlayer02) + "%";
        _dodgePlayer02Text.text = GetFormattedAttributeText("Né", _dodgePlayer02) + "%";
        _bloodsuckingPlayer02Text.text = GetFormattedAttributeText("Hút máu", _bloodsuckingPlayer02) + "%";
        _reducedHealingPlayer02Text.text = GetFormattedAttributeText("Giảm hồi máu", _reducedHealingPlayer02) + "%";
        _speedPlayer02Text.text = GetFormattedAttributeText("Tốc độ", _speedPlayer02);
        _resistancePlayer02Text.text = GetFormattedAttributeText("Kháng hiệu ứng", _resistancePlayer02);
        _penetratePlayer02Text.text = GetFormattedAttributeText("Xuyên giáp", _penetratePlayer02);
        _attackSpeedPlayer02Text.text = GetFormattedAttributeText("Tốc đánh", _attackSpeedPlayer02) + "%";
        _reducedTimePlayer02Text.text = GetFormattedAttributeText("Giảm hồi chiêu", _reducedTimePlayer02) + "%";
        _stunPlayer02Text.text = GetFormattedAttributeText("Bất động", _stunPlayer02) + "%";
 
        
        healthDelayBarPlayer02.SetMaxHealth(displayedMaxHealth);
        healthDelayBarPlayer02.SetHealth(displayedHealth);
        manaDelayBarPlayer02.SetHealth(_manaPlayer02);
 
        healthBarPlayer02.SetMaxHealth(displayedMaxHealth);
        healthBarPlayer02.SetHealth(displayedHealth);
 
        manaBarPlayer02.SetMaxHealth(_manaMaxPlayer02);
        manaBarPlayer02.SetHealth(_manaPlayer02);
    }

    private IEnumerator SetMaxBar()
    {
        yield return new WaitForSeconds(4.0f);
        healthDelayBarPlayer01.SetMaxHealth(_healthMaxPlayer01);
        healthDelayBarPlayer02.SetMaxHealth(_healthMaxPlayer02);

        manaDelayBarPlayer01.SetMaxHealth(_manaMaxPlayer01);
        manaDelayBarPlayer02.SetMaxHealth(_manaMaxPlayer02);
    }

    private string GetFormattedAttributeText(string attributeName, float attributeValue)
    {
        if(attributeValue == 0)
        {
            return attributeName + ": " + 0;
        }
        else if(attributeValue < 1 && attributeValue > 0)
        {
            return attributeName + ": " + "0" + attributeValue.ToString("###,###.#");
        }
        else
        {
            return attributeName + ": " + attributeValue.ToString("###,###.#");
        }
    }

    public IEnumerator UpdatePropertiesPlayer(string tag)
    {
        yield return new WaitForSeconds(1.5f);
        if (tag == "Player01")
        {
            _photonView.RPC("SetPropertiesPlayer01", RpcTarget.All);
        }
        else if (tag == "Player02")
        {
            _photonView.RPC("SetPropertiesPlayer02", RpcTarget.All);
        }
    }

    public float GetDamagePlayer01()
    {
        return _damagePlayer01;
    }

    public float GetDefensePlayer01()
    {
        return _defensePlayer01;
    }

    public float GetHealthMaxPlayer01()
    {
        return _healthMaxPlayer01;
    }

    public float GetHealthPlayer01()
    {
        return _healthPlayer01;
    }

    public float GetHealingPlayer01()
    {
        return _healingPlayer01;
    }
    public float GetManaMaxPlayer01()
    {
        return _manaMaxPlayer01;
    }

    public float GetManaPlayer01()
    {
        return _manaPlayer01;
    }

    public float GetRestoreManaPlayer01()
    {
        return _restoreManaPlayer01;
    }

    public float GetCriticalPlayer01()
    {
        return _criticalPlayer01;
    }

    public float GetCriticalDamagePlayer01()
    {
        return _criticalDamagePlayer01;
    }

    public float GetDodgePlayer01()
    {
        return _dodgePlayer01;
    }

    public float GetBloodsuckingPlayer01()
    {
        return _bloodsuckingPlayer01;
    }

    public float GetReducedHealingPlayer01()
    {
        return _reducedHealingPlayer01;
    }

    public float GetSpeedPlayer01()
    {
        return _speedPlayer01;
    }

    public float GetResistancePlayer01()
    {
        return _resistancePlayer01;
    }

    public float GetPenetratePlayer01()
    {
        return _penetratePlayer01;
    }

    public float GetAttackSpeedPlayer01()
    {
        return _attackSpeedPlayer01;
    }

    public float GetReducedTimePlayer01()
    {
        return _reducedTimePlayer01;
    }

    public float GetStunPlayer01()
    {
        return _stunPlayer01;
    }

    public float GetDamagePlayer02()
    {
        return _damagePlayer02;
    }

    public float GetDefensePlayer02()
    {
        return _defensePlayer02;
    }

    public float GetHealthMaxPlayer02()
    {
        return _healthMaxPlayer02;
    }

    public float GetHealthPlayer02()
    {
        return _healthPlayer02;
    }

    public float GetHealingPlayer02()
    {
        return _healingPlayer02;
    }

    public float GetManaMaxPlayer02()
    {
        return _manaMaxPlayer02;
    }

    public float GetManaPlayer02()
    {
        return _manaPlayer02;
    }

    public float GetRestoreManaPlayer02()
    {
        return _restoreManaPlayer02;
    }

    public float GetCriticalPlayer02()
    {
        return _criticalPlayer02;
    }

    public float GetCriticalDamagePlayer02()
    {
        return _criticalDamagePlayer02;
    }

    public float GetDodgePlayer02()
    {
        return _dodgePlayer02;
    }

    public float GetBloodsuckingPlayer02()
    {
        return _bloodsuckingPlayer02;
    }

    public float GetReducedHealingPlayer02()
    {
        return _reducedHealingPlayer02;
    }

    public float GetSpeedPlayer02()
    {
        return _speedPlayer02;
    }

    public float GetResistancePlayer02()
    {
        return _resistancePlayer02;
    }

    public float GetPenetratePlayer02()
    {
        return _penetratePlayer02;
    }

    public float GetAttackSpeedPlayer02()
    {
        return _attackSpeedPlayer02;
    }

    public float GetReducedTimePlayer02()
    {
        return _reducedTimePlayer02;
    }

    public float GetStunPlayer02()
    {
        return _stunPlayer02;
    }

    [PunRPC]
    public void SetDamageHurtPlayer01(float damage, float hitWaitTime)
    {
        _healthPlayer01 -= damage;
        _hitWaitTimePlayer01 = hitWaitTime;
    }

    [PunRPC]
    public void SetDamageHurtPlayer02(float damage, float hitWaitTime)
    {
        _healthPlayer02 -= damage;
        _hitWaitTimePlayer02 = hitWaitTime;
    }
    
    public void TakeDamagePlayer01(float damage, float hitWaitTime)
    {
        StartCoroutine(ProcessDamagePlayer01(damage, hitWaitTime));
    }

    private IEnumerator ProcessDamagePlayer01(float damage, float hitWaitTime)
    {
        yield return null;
        if (_cloneHitPlayer01ThisFrame != null)
        {
            _cloneHitPlayer01ThisFrame.TakeDamage(damage);
            _cloneHitPlayer01ThisFrame = null;
        }
        else
        {
            _photonView.RPC("SetDamageHurtPlayer01", RpcTarget.All, damage, hitWaitTime);
        }
    }

    public void TakeDamagePlayer02(float damage, float hitWaitTime)
    {
        StartCoroutine(ProcessDamagePlayer02(damage, hitWaitTime));
    }

    private IEnumerator ProcessDamagePlayer02(float damage, float hitWaitTime)
    {
        yield return null;
        if (_cloneHitPlayer02ThisFrame != null)
        {
            _cloneHitPlayer02ThisFrame.TakeDamage(damage);
            _cloneHitPlayer02ThisFrame = null;
        }
        else
        {
            _photonView.RPC("SetDamageHurtPlayer02", RpcTarget.All, damage, hitWaitTime);
        }
    }

    [PunRPC]
    public void SetPlusHealthPlayer01(float health)
    {
        _healthPlayer01 += health;
        if (_healthPlayer01 > _healthMaxPlayer01)
        {
            _healthPlayer01 = _healthMaxPlayer01;
        }
    }

    [PunRPC]
    public void SetPlusHealthPlayer02(float health)
    {
        _healthPlayer02 += health;
        if (_healthPlayer02 > _healthMaxPlayer02)
        {
            _healthPlayer02 = _healthMaxPlayer02;
        }
    }

    public void TakePlusHealthPlayer01(float health)
    {
        _photonView.RPC("SetPlusHealthPlayer01", RpcTarget.All, health);
    }

    public void TakePlusHealthPlayer02(float health)
    {
        _photonView.RPC("SetPlusHealthPlayer02", RpcTarget.All, health);
    }

    [PunRPC]
    public void SetPlusManaPlayer01(float mana)
    {
        _manaPlayer01 += mana;
        if (_manaPlayer01 > _manaMaxPlayer01)
        {
            _manaPlayer01 = _manaMaxPlayer01;
        }
    }

    [PunRPC]
    public void SetPlusManaPlayer02(float mana)
    {
        _manaPlayer02 += mana;
        if (_manaPlayer02 > _manaMaxPlayer02)
        {
            _manaPlayer02 = _manaMaxPlayer02;
        }
    }

    public void TakePlusManaPlayer01(float mana)
    {
        _photonView.RPC("SetPlusManaPlayer01", RpcTarget.All, mana);
    }

    public void TakePlusManaPlayer02(float mana)
    {
        _photonView.RPC("SetPlusManaPlayer02", RpcTarget.All, mana);
    }

    [PunRPC]
    public void SetMinusManaPlayer01(float mana)
    {
        _manaPlayer01 -= mana;
    }

    [PunRPC]
    public void SetMinusManaPlayer02(float mana)
    {
        _manaPlayer02 -= mana;
    }

    public void TakeMinusManaPlayer01(float mana)
    {
        _photonView.RPC("SetMinusManaPlayer01", RpcTarget.All, mana);
    }

    public void TakeMinusManaPlayer02(float mana)
    {
        _photonView.RPC("SetMinusManaPlayer02", RpcTarget.All, mana);
    }

    


    [PunRPC]
    public void SetPlusDamPlayer01(float dam)
    {
        _damagePlayer01 += dam;
    }

    [PunRPC]
    public void SetPlusDamPlayer02(float dam)
    {
        _damagePlayer02 += dam;
    }

    public void TakePlusDamPlayer01(float dam)
    {
        _photonView.RPC("SetPlusDamPlayer01", RpcTarget.All, dam);
    }

    public void TakePlusDamPlayer02(float dam)
    {
        _photonView.RPC("SetPlusDamPlayer02", RpcTarget.All, dam);
    }


    public void UpdateHitTimeWait()
    {
        _hitWaitTimePlayer01 -= Time.deltaTime;
        _hitWaitTimePlayer02 -= Time.deltaTime;
    }

    public float GetHitTimeWaitPlayer01()
    {
        return _hitWaitTimePlayer01;
    }

    public float GetHitTimeWaitPlayer02()
    {
        return _hitWaitTimePlayer02;
    }

    public bool GetEndGame()
    {
        return _endGame;
    }
}
