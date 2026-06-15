using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Text;
using System.Linq;

public class Menu : MonoBehaviourPunCallbacks
{
    private const string PlayerNameKey = "PlayerName";
    public TextMeshProUGUI _playerNameText;
    public Button _editNameButton;
    public TMP_InputField _setNameInput;
    public Button _applyNameButton;
    public Button _closeUiChangeNameButton;
    public TextMeshProUGUI _errorNameText;
    public GameObject _uiChangeName;
    public GameObject _showNameText;
    public GameObject _closeNameUI;
    public Button _CreateSoloLobbyButton;
    public Button _CreateTeamLobbyButton;
    public Button _CreateComputerLobbyButton;
    public Button _joinRandomLobbySoloButton;
    public Button _joinRandomLobbyTeamButton;
    public Button _joinNameRoomButton;
    public Button _closeUiJoinRoomButton;

    public Button _joinLobbyButton;
    public GameObject _uiJoinRoom;

    public Button _sortAllButton;
    public Button _sortSoloButton;
    public Button _sortTeamButton;
    public TMP_InputField _setIdLobbyInput;
    public TextMeshProUGUI _errorJoinNameText;
    public TextMeshProUGUI _errorJoinRoomText;

    private bool isNameValid = false;
    private List<RoomInfo> roomList = new List<RoomInfo>();
    public GameObject roomInfoPrefab;
    public Transform roomListContent;
    private List<GameObject> instantiatedRoomInfos = new List<GameObject>();
    public GameObject _scrollTop;
    public GameObject _scrollBottom;

    public Animator animatorTagHome;
    public Animator animatorTagUser;
    public Animator animatorTagSkill;
    public Animator animatorTagSpell;
    public Animator animatorTagGem;
    public Animator animatorTagSettings;
    public Animator animatorTagInstruct;
    public Animator animatorCloseBook;
    public GameObject[] _changeScene;

    public GameObject _uiHome;
    public GameObject _uiUser;
    public GameObject _uiSkill;
    public GameObject _uiSpell;
    public GameObject _uiGem;
    public GameObject _uiSettings;
    public GameObject _uiInstruct;

    public GameObject _champ;
    public GameObject _skill;

    private void Start()
    {
        UpdateName();

        _scrollTop.SetActive(false);
        _scrollBottom.SetActive(false);

        _applyNameButton.onClick.AddListener(ApplyName);
        _CreateSoloLobbyButton.onClick.AddListener(OnCreateSoloLobbyButtonClick);
        _CreateTeamLobbyButton.onClick.AddListener(CreateTeamLobbyButton);
        _CreateComputerLobbyButton.onClick.AddListener(OnCreateComputerLobbyButtonClick);
        _joinRandomLobbySoloButton.onClick.AddListener(JoinSoloLobbyButton);
        _joinRandomLobbyTeamButton.onClick.AddListener(JoinTeamLobbyButton);

        _joinLobbyButton.onClick.AddListener(JoinLobby);

        _joinNameRoomButton.onClick.AddListener(() =>
        {
            _uiJoinRoom.SetActive(true);
        });
        _closeUiJoinRoomButton.onClick.AddListener(() =>
        {
            _uiJoinRoom.SetActive(false);
        });
        _editNameButton.onClick.AddListener(() =>
        {
            _uiChangeName.SetActive(true);
        });
        _closeUiChangeNameButton.onClick.AddListener(() =>
        {
            _uiChangeName.SetActive(false);
        });

        _sortAllButton.onClick.AddListener(SortAll);
        _sortSoloButton.onClick.AddListener(SortSolo);
        _sortTeamButton.onClick.AddListener(SortTeam);

        PhotonNetwork.JoinLobby();
        PhotonNetwork.AddCallbackTarget(this);
    }
    private IEnumerator DelayShowChangeName()
    {
        yield return new WaitForSeconds(3.0f);
        _showNameText.SetActive(true);
        _uiChangeName.SetActive(true);
        _closeNameUI.SetActive(false);
    }

    private void UpdateName()
    {
        if (PlayerPrefs.HasKey(PlayerNameKey))
        {
            string playerName = PlayerPrefs.GetString(PlayerNameKey);
            _setNameInput.text = playerName;
            _playerNameText.text = playerName;
            ApplyName();
        }
        else
        {
            StartCoroutine(DelayShowChangeName());
        }
    }

    private void SortAll()
    {
        foreach (GameObject roomInfo in instantiatedRoomInfos)
        {
            roomInfo.SetActive(true);
        }
    }

    private void SortSolo()
    {
        foreach (GameObject roomInfo in instantiatedRoomInfos)
        {
            if (roomInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text == "Solo Room")
            {
                roomInfo.SetActive(true);
            }
            else
            {
                roomInfo.SetActive(false);
            }
        }
    }

    private void SortTeam()
    {
        foreach (GameObject roomInfo in instantiatedRoomInfos)
        {
            if (roomInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text == "Team Room")
            {
                roomInfo.SetActive(true);
            }
            else
            {
                roomInfo.SetActive(false);
            }
        }
    }

    private void ApplyName()
    {
        string playerName = _setNameInput.text;
        if (playerName.Length > 2)
        {
            isNameValid = true;
            _errorNameText.text = "";
            PhotonNetwork.NickName = playerName;
            PlayerPrefs.SetString(PlayerNameKey, playerName);
            _playerNameText.text = playerName;
            _uiChangeName.SetActive(false);
            _showNameText.SetActive(false);
            _closeNameUI.SetActive(true);
        }
        else
        {
            _errorNameText.text = "Tên phải nhiều hơn 2 ký tự!";
            isNameValid = false;
        }
    }

    private void ChangeScene()
    {
        animatorCloseBook.SetBool("isClose", true);
        animatorTagHome.SetBool("isEnd", true);
        animatorTagUser.SetBool("isEnd", true);
        animatorTagSkill.SetBool("isEnd", true);
        animatorTagSpell.SetBool("isEnd", true);
        animatorTagGem.SetBool("isEnd", true);
        animatorTagSettings.SetBool("isEnd", true);
        animatorTagInstruct.SetBool("isEnd", true);
        animatorTagHome.SetBool("isStart", true);
        RandomChangeScene();
    }
    void RandomChangeScene()
    {
        int random = Random.Range(0, _changeScene.Length);
        _changeScene[random].SetActive(true);
    }
    private void HiddenAllUI()
    {
        _uiHome.SetActive(false);
        _uiUser.SetActive(false);
        _uiSkill.SetActive(false);
        _uiSpell.SetActive(false);
        _uiGem.SetActive(false);
        _uiSettings.SetActive(false);
        _uiInstruct.SetActive(false);
        _champ.SetActive(false);
        _skill.SetActive(false);
    }

    private void OnCreateComputerLobbyButtonClick()
    {
        ChangeScene();
        HiddenAllUI();
        StartCoroutine(CreateComputerLobbyButton());
    }

    private IEnumerator CreateComputerLobbyButton()
    {
        yield return new WaitForSeconds(3.0f);
        System.Random random = new System.Random();
        int randomValueFront = random.Next(0, 999);
        int randomValueBack = random.Next(0, 999);
        string timeNow = System.DateTime.Now.ToString("mmss");
        string lobbyName = randomValueFront.ToString("D3") + timeNow + randomValueBack.ToString("D3");
        ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable { { "Host", PhotonNetwork.NickName } };
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 1,
            CustomRoomProperties = roomProperties,
            CustomRoomPropertiesForLobby = new string[] { "Host" }
        };
        PhotonNetwork.CreateRoom(lobbyName, roomOptions);
    }

    private void OnCreateSoloLobbyButtonClick()
    {
        ChangeScene();
        HiddenAllUI();
        StartCoroutine(CreateSoloLobbyButton());
    }

    private IEnumerator CreateSoloLobbyButton()
    {
        yield return new WaitForSeconds(3.0f);
        System.Random random = new System.Random();
        int randomValueFront = random.Next(0, 999);
        int randomValueBack = random.Next(0, 999);
        string timeNow = System.DateTime.Now.ToString("mmss");
        string lobbyName = randomValueFront.ToString("D3") + timeNow + randomValueBack.ToString("D3");
        ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable { { "Host", PhotonNetwork.NickName } };
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 2,
            CustomRoomProperties = roomProperties,
            CustomRoomPropertiesForLobby = new string[] { "Host" }
        };
        PhotonNetwork.CreateRoom(lobbyName, roomOptions);
    }


    private void CreateTeamLobbyButton()
    {
        System.Random random = new System.Random();

        int randomValueFront = random.Next(0, 999);
        int randomValueBack = random.Next(0, 999);

        string timeNow = System.DateTime.Now.ToString("mmss");

        string lobbyName = randomValueFront.ToString("D3") + timeNow + randomValueBack.ToString("D3");

        ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable { { "Host", PhotonNetwork.NickName } };
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 4,
            CustomRoomProperties = roomProperties,
            CustomRoomPropertiesForLobby = new string[] { "Host" }
        };

        PhotonNetwork.CreateRoom(lobbyName, roomOptions);
    }

    private void JoinSoloLobbyButton()
    {
        List<RoomInfo> soloRooms = roomList.Where(room => room.MaxPlayers == 2 && room.PlayerCount < room.MaxPlayers).ToList();

        if (soloRooms.Count > 0)
        {
            int randomIndex = Random.Range(0, soloRooms.Count);
            RoomInfo selectedRoom = soloRooms[randomIndex];

            JoinRoom(selectedRoom.Name);
        }
        else
        {
            _errorJoinRoomText.text = "Không có phòng Đơn nào trống!";
        }
    }

    private void JoinTeamLobbyButton()
    {
        List<RoomInfo> teamRooms = roomList.Where(room => room.MaxPlayers == 4 && room.PlayerCount < room.MaxPlayers).ToList();

        if (teamRooms.Count > 0)
        {
            int randomIndex = Random.Range(0, teamRooms.Count);
            RoomInfo selectedRoom = teamRooms[randomIndex];

            JoinRoom(selectedRoom.Name);
        }
        else
        {
            _errorJoinRoomText.text = "Không có phòng Đội nào trống!";
        }
    }


    private void JoinLobby()
    {
        if (!isNameValid)
            return;

        string lobbyId = _setIdLobbyInput.text;
        if (string.IsNullOrEmpty(lobbyId))
        {
            _errorJoinNameText.text = "Vui lòng nhập mã phòng!";
            return;
        }

        if (!PhotonNetwork.IsConnectedAndReady)
            return;

        List<RoomInfo> roomChoose = roomList.Where(room => room.Name == lobbyId && room.PlayerCount == room.MaxPlayers).ToList();
               
        if (roomChoose.Count > 0)
        {
            _errorJoinRoomText.text = "Phòng đã đầy!";
            return;
        }

        PhotonNetwork.JoinRoom(lobbyId);
    }

    private void JoinRoom(string roomId)
    {
        if (!isNameValid)
            return;

        string lobbyId = roomId;
        if (string.IsNullOrEmpty(lobbyId))
        {
            _errorJoinNameText.text = "Vui lòng nhập mã phòng!";
            return;
        }

        if (!PhotonNetwork.IsConnectedAndReady)
            return;
        
        List<RoomInfo> roomChoose = roomList.Where(room => room.Name == lobbyId && room.PlayerCount == room.MaxPlayers).ToList();
               
        if (roomChoose.Count > 0)
        {
            _errorJoinRoomText.text = "Phòng đã đầy!";
            return;
        }

        PhotonNetwork.JoinRoom(lobbyId);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.MaxPlayers == 1)
        {
            PhotonNetwork.LoadLevel("ComputerLobby");
        }
        if (PhotonNetwork.CurrentRoom.MaxPlayers == 2)
        {
            PhotonNetwork.LoadLevel("SoloLobby");
        }
        if (PhotonNetwork.CurrentRoom.MaxPlayers == 4)
        {
            PhotonNetwork.LoadLevel("TeamLobby");
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            bool roomExists = false;
            for (int i = 0; i < this.roomList.Count; i++)
            {
                if (this.roomList[i].Name == room.Name)
                {
                    roomExists = true;
                    this.roomList[i] = room;
                    break;
                }
            }

            if (!roomExists && room.PlayerCount > 0)
            {
                this.roomList.Add(room);
            }
        }

        foreach (GameObject roomInfo in instantiatedRoomInfos)
        {
            Destroy(roomInfo);
        }
        instantiatedRoomInfos.Clear();

        foreach (RoomInfo room in this.roomList)
        {
            GameObject newRoomInfo = Instantiate(roomInfoPrefab, roomListContent);
            newRoomInfo.SetActive(true);

            TextMeshProUGUI idLobbyText = newRoomInfo.GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI nameHostText = newRoomInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI typeText = newRoomInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI activeText = newRoomInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            Button joinButton = newRoomInfo.transform.GetChild(4).GetComponent<Button>();

            idLobbyText.text = "Mã: " + room.Name;

            if (room.MaxPlayers == 2)
            {
                typeText.text = "Phòng Đơn";
            }
            else if (room.MaxPlayers == 4)
            {
                typeText.text = "Phòng Đội";
            }
            else
            {
                typeText.text = "";
            }

            if (room.PlayerCount >= room.MaxPlayers)
            {
                activeText.text = $"{room.PlayerCount}/{room.MaxPlayers}(Max)";
            }
            else
            {
                activeText.text = $"{room.PlayerCount}/{room.MaxPlayers}";
            }

            if (room.CustomProperties.ContainsKey("Host"))
            {
                nameHostText.text = "Chủ phòng: " + room.CustomProperties["Host"].ToString();
            }
            else
            {
                nameHostText.text = "Đã bị hủy";
                activeText.text = "";
            }

            joinButton.onClick.AddListener(() => JoinRoom(room.Name));
            instantiatedRoomInfos.Add(newRoomInfo); 
        }
        if (roomListContent.childCount > 6)
        {
            _scrollTop.SetActive(true);
            _scrollBottom.SetActive(true);
        }
        else
        {
            _scrollTop.SetActive(false);
            _scrollBottom.SetActive(false);
        } 
    }

    public override void OnLeftRoom()
    {
        roomList.Clear();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"Disconnected from Photon: {cause}. Returning to Loading scene.");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    private bool RoomNameExists(string roomName)
    {
        foreach (RoomInfo room in roomList)
        {
            if (room.Name == roomName)
            {
                return true;
            }
        }
        return false;
    }

}