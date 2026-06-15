using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
public class Loadding : MonoBehaviourPunCallbacks
{
    public void Start()
    {
        Application.runInBackground = true;
        PhotonNetwork.KeepAliveInBackground = 60f;
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        PhotonNetwork.LoadLevel("Menu");
    }
}