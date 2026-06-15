using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Text;
using System.Linq;

public class SoloLobby : MonoBehaviourPunCallbacks
{
    private const string PlayerNameKey = "PlayerName";
    private const string SelectedChampKey = "SelectedChamp";
    private PhotonView _photonView;
    public Button _startButton;
    public Button _leaveButton;
    public TextMeshProUGUI _playerCountText;
    public TextMeshProUGUI _idLobbyText;
    public Button _copyIdLobbyButton;
    public GameObject[] _playerObjects;
    public TextMeshProUGUI[] _playerNameTexts;
    public TextMeshProUGUI[] _champNameTexts;
    public GameObject[] _champImageObjects;
    public GameObject[] _champImagePrefabs;
    public Button[] _kickButtons;
    public GameObject[] _kickButtonsHostOnly;
    public GameObject[] _hostTextsHostOnly;

    public TMP_InputField _chatInput;
    public Button _chatButton;
    public ScrollRect _chatListView;
    public Transform _chatListContent;
    public GameObject _contentChatPrefab;
    private List<GameObject> _chatMessages = new List<GameObject>();
    
    private Dictionary<Player, string> playerChampMap = new Dictionary<Player, string>();

    private bool isHost = false;
    private bool isKicked = false;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();

        _idLobbyText.text = PhotonNetwork.CurrentRoom.Name;

        _startButton.onClick.AddListener(StartGame);
        _copyIdLobbyButton.onClick.AddListener(CopyLobbyId);
        _leaveButton.onClick.AddListener(LeaveLobby);
        _chatInput.onEndEdit.AddListener(OnChatInputEndEdit);
        _chatButton.onClick.AddListener(SendChatMessage);

        UpdateName();

        InitializePlayerObjects();

        PlayerPrefs.SetString("Scene", "SoloLobby");
    }

    private void UpdateName()
    {
        string playerName = PlayerPrefs.GetString(PlayerNameKey);
        string newPlayerName = playerName;
        if (PlayerNameExists(playerName))
        {
            newPlayerName += "Z";
            PhotonNetwork.NickName = newPlayerName;
        }
        else
        {
            PhotonNetwork.NickName = playerName;
        }
    }

    private bool PlayerNameExists(string playerName)
    {
        Player[] players = PhotonNetwork.PlayerList;
        foreach (Player player in players)
        {
            if (player.NickName == playerName && player != PhotonNetwork.LocalPlayer)
            {
                return true;
            }
        }
        return false;
    }

    private void Update()
    {
        if (isKicked)
        {
            isKicked = false;
            StartCoroutine(DelayedSceneChange());
        }
    }
    
    private void Awake()
    {
        for (int i = 0; i < _kickButtons.Length; i++)
        {
            int playerIndex = i;
            _kickButtons[i].onClick.AddListener(() => KickPlayer(playerIndex));
        }
    }

    private void CopyLobbyId()
    {
        GUIUtility.systemCopyBuffer = PhotonNetwork.CurrentRoom.Name;
    }

    private void InitializePlayerObjects()
    {
        isHost = PhotonNetwork.IsMasterClient;

        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            Player player = PhotonNetwork.PlayerList[i];
            _playerObjects[i].SetActive(true);
            _playerNameTexts[i].text = player.NickName;

            if (player.CustomProperties.TryGetValue("SelectedChampName", out object selectedChampObj) && selectedChampObj is string selectedChampName)
            {
                _champNameTexts[i].text = selectedChampName;

                GameObject champImageObject = _champImageObjects[i];
                for (int j = 0; j < _champImagePrefabs.Length; j++)
                {
                    if (champImageObject.transform.childCount == 0 && _champImagePrefabs.Length > j && _champImagePrefabs[j] != null && _champImagePrefabs[j].name == selectedChampName)
                    {
                        GameObject instantiatedChampImagePrefab = Instantiate(_champImagePrefabs[j], champImageObject.transform);
                        instantiatedChampImagePrefab.transform.SetParent(champImageObject.transform, false);
                    }
                }
                playerChampMap[player] = selectedChampName;
            }
            else
            {
                _champNameTexts[i].text = "No Champ";
            }
        }

        for (int i = PhotonNetwork.CurrentRoom.PlayerCount; i < _playerObjects.Length; i++)
        {
            _playerObjects[i].SetActive(false);
        }
        for (int i = 0; i < _kickButtonsHostOnly.Length; i++)
        {
            _kickButtonsHostOnly[i].SetActive(isHost);
            _kickButtonsHostOnly[0].SetActive(false);
        }
        for (int i = 0; i < _hostTextsHostOnly.Length; i++)
        {
            _hostTextsHostOnly[i].SetActive(false);
            _hostTextsHostOnly[0].SetActive(true);
        }
        UpdateStartButtonState();
        UpdatePlayerCountText();
    }

    private void UpdateStartButtonState()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount > 1 && isHost)
            _startButton.interactable = true;
        else
            _startButton.interactable = false;
    }

    public void StartGame()
    {
        Player[] players = PhotonNetwork.PlayerList;

        ExitGames.Client.Photon.Hashtable CustomProperties = new ExitGames.Client.Photon.Hashtable();

        Dictionary<string, ExitGames.Client.Photon.Hashtable> playerData = new Dictionary<string, ExitGames.Client.Photon.Hashtable>();

        for(int i = 0; i < players.Length; i++)
        {
            Player player = players[i];
            ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
            playerProperties["PlayerName"] = player.NickName;
            
            if (player.CustomProperties.TryGetValue("SelectedChampName", out object selectedChampObj) && selectedChampObj is string selectedChampName)
            {
                playerProperties["ChampName"] = selectedChampName;
            }
            else
            {
                playerProperties["ChampName"] = "No Champ";
            }
            playerData[player.NickName] = playerProperties;
        }

        CustomProperties["PlayerData"] = playerData;
        PhotonNetwork.CurrentRoom.SetCustomProperties(CustomProperties);

        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        StartCoroutine(DelayedStartGame());
    }

    private IEnumerator DelayedStartGame()
    {
        yield return new WaitForSeconds(2f);
        _photonView.RPC("RequestSceneChangeToGame", RpcTarget.All);
    }
    private IEnumerator DelayedPlayerEnteredRoom()
    {
        yield return new WaitForSeconds(0.3f);
        InitializePlayerObjects();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        InitializePlayerObjects();
        _photonView.RPC("SendNoticationJoinRoom", RpcTarget.All, newPlayer);
        StartCoroutine(DelayedPlayerEnteredRoom());
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateName();
        Player[] players = PhotonNetwork.PlayerList;
        if (players.Length > 0)
        {
            Player newHost = players[0];
            PhotonNetwork.SetMasterClient(newHost);

            ExitGames.Client.Photon.Hashtable CustomProperties = new ExitGames.Client.Photon.Hashtable();
            CustomProperties["Host"] = newHost.NickName;
            PhotonNetwork.CurrentRoom.SetCustomProperties(CustomProperties);
        }

        for (int i = 0; i < _champImageObjects.Length; i++)
        {
            GameObject champImageObject = _champImageObjects[i];
            for (int j = 0; j < champImageObject.transform.childCount; j++)
            {
                Destroy(champImageObject.transform.GetChild(j).gameObject);
            }
        }

        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            Player player = PhotonNetwork.PlayerList[i];
            string selectedChampName;
            
            if (player.CustomProperties.TryGetValue("SelectedChampName", out object selectedChampObj) && selectedChampObj is string)
            {
                selectedChampName = (string)selectedChampObj;
            }
            else
            {
                selectedChampName = "No Champ";
            }
            
            GameObject champImageObject = _champImageObjects[i];
            for (int j = 0; j < _champImagePrefabs.Length; j++)
            {
                if (_champImagePrefabs[j] != null && _champImagePrefabs[j].name == selectedChampName)
                {
                    GameObject instantiatedChampImagePrefab = Instantiate(_champImagePrefabs[j], champImageObject.transform);
                    instantiatedChampImagePrefab.transform.SetParent(champImageObject.transform, false);
                }
            }
        }
        _photonView.RPC("SendNoticationLeaveRoom", RpcTarget.All, otherPlayer);
        InitializePlayerObjects();
    }

    private void UpdatePlayerCountText()
    {
        int currentPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        int maxPlayer = PhotonNetwork.CurrentRoom.MaxPlayers;
        _playerCountText.text = $"Số người trong phòng: {currentPlayerCount}/{maxPlayer}";
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
    private void RequestSceneChangeToGame()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    public void KickPlayer(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < PhotonNetwork.PlayerList.Length)
        {
            Player playerToKick = PhotonNetwork.PlayerList[playerIndex];
            if (playerToKick != null)
            {
                if (isHost)
                {
                    _photonView.RPC("KickPlayerRPC", playerToKick);
                }
            }
        }
    }

    [PunRPC]
    private void KickPlayerRPC()
    {
        isKicked = true;
        PhotonNetwork.LeaveRoom();
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

        messageText.text = $"{playerName} ({champName}): {message}";

        _chatMessages.Add(messageObject);

        StartCoroutine(DelayedScrollToBottom());
    }


    [PunRPC]
    private void SendNoticationJoinRoom(Player newPlayer)
    {
        GameObject messageObject = Instantiate(_contentChatPrefab, _chatListContent);
        TextMeshProUGUI messageText = messageObject.GetComponentInChildren<TextMeshProUGUI>();

        Color orangeColor = new Color(1.0f, 0.5f, 0.0f);
        messageText.color = orangeColor;

        messageText.text = $"Hệ thống: {newPlayer.NickName} đã vào phòng!!";

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
}