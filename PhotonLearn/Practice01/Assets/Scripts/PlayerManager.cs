using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
public class PlayerManager : MonoBehaviourPunCallbacks
{

    private void Start() {
        PhotonNetwork.Instantiate("playerPrefab", new Vector3(0, 0, 0), Quaternion.identity, 0);
    }
}
