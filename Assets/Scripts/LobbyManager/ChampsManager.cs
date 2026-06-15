using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class ChampsManager : MonoBehaviour
{
    private const string SelectedChampKey = "SelectedChamp";

    public GameObject ChampContainer;
    public TextMeshProUGUI _champNameText;

    public Transform[] champs;

    public Button _nextChampButton;
    public Button _previousChampButton;

    private int currentChampIndex = 0;

    private string[] champNames;

    private void Start()
    {
        champNames = new string[champs.Length];
        for (int i = 0; i < champNames.Length; i++)
        {
            champNames[i] = champs[i].name;
        }

        if (PlayerPrefs.HasKey(SelectedChampKey))
        {
            currentChampIndex = PlayerPrefs.GetInt(SelectedChampKey);
            ChooseChamp();
        }
        else
        {
            ChooseChamp();
        }

        champs = new Transform[ChampContainer.transform.childCount];
        for (int i = 0; i < ChampContainer.transform.childCount; i++)
        {
            champs[i] = ChampContainer.transform.GetChild(i);
            champs[i].gameObject.SetActive(i == currentChampIndex);
        }

        _nextChampButton.onClick.AddListener(ShowNextChamp);
        _previousChampButton.onClick.AddListener(ShowPreviousChamp);
    }

    public void ShowNextChamp()
    {
        champs[currentChampIndex].gameObject.SetActive(false);
        currentChampIndex = (currentChampIndex + 1) % champs.Length;
        champs[currentChampIndex].gameObject.SetActive(true);
        ChooseChamp();
    }

    public void ShowPreviousChamp()
    {
        champs[currentChampIndex].gameObject.SetActive(false);
        currentChampIndex = (currentChampIndex - 1 + champs.Length) % champs.Length;
        champs[currentChampIndex].gameObject.SetActive(true);
        ChooseChamp();
    }

    public void ChooseChamp()
    {
        int selectedChampIndex = currentChampIndex;

        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
        playerProperties["SelectedChampIndex"] = selectedChampIndex;
        playerProperties["SelectedChampName"] = champNames[selectedChampIndex];
        PlayerPrefs.SetInt(SelectedChampKey, selectedChampIndex);
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        _champNameText.text = champNames[selectedChampIndex];
    }

}
