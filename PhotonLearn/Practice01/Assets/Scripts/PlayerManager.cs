using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
public class PlayerManager : MonoBehaviourPunCallbacks
{
    public Vector3 offset;

    private void Start() {
        PhotonNetwork.Instantiate("playerPrefab", new Vector3(0, 0, 0), Quaternion.identity, 0);
    }

    private void Update()
    {
        for(int i = 0; i < FindObjectsOfType<PhotonView>().Length; i++)
        {
            if (FindObjectsOfType<PhotonView>()[i].IsMine)
            {
                if(FindObjectsOfType<PhotonView>()[i].gameObject.name != "NetworkManager") Camera.main.transform.position = FindObjectsOfType<PhotonView>()[i].gameObject.transform.position + offset;
            }
        }
    }
}
