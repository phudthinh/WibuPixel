using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DestroyObject : MonoBehaviourPunCallbacks
{
    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }

    [PunRPC]
    public void DestroyThisRPC()
    {
        Destroy(this.gameObject);
    }
}
