using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desk : MonoBehaviour
{
    public GameObject DeskA;
    public GameObject DeskB;
    public GameObject DeskC;
    public GameObject DeskD;
    void Start()
    {
        RandomDesk();
    }

    void RandomDesk()
    {
        int random = Random.Range(0, 4);
        if(random == 0)
        {
            DeskA.SetActive(true);
            DeskB.SetActive(false);
            DeskC.SetActive(false);
            DeskD.SetActive(false);
        }
        else if(random == 1)
        {
            DeskA.SetActive(false);
            DeskB.SetActive(true);
            DeskC.SetActive(false);
            DeskD.SetActive(false);
        }
        else if(random == 2)
        {
            DeskA.SetActive(false);
            DeskB.SetActive(false);
            DeskC.SetActive(true);
            DeskD.SetActive(false);
        }
        else if(random == 3)
        {
            DeskA.SetActive(false);
            DeskB.SetActive(false);
            DeskC.SetActive(false);
            DeskD.SetActive(true);
        }
    }
}
