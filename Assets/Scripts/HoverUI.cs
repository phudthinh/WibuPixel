using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverUI : MonoBehaviour
{
    public GameObject _UI;

    private void OnMouseEnter()
    {
        _UI.SetActive(true);
    }

    private void OnMouseExit()
    {
        _UI.SetActive(false);
    }
}
