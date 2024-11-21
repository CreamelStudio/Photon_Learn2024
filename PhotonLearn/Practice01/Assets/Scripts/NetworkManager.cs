using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class NetworkManager : MonoBehaviour
{
    public Text statusText;

    public InputField roomField;
    public InputField nickName;

    public Text roomData;
    public Text nickNameText;

    public GameObject startButton;

    public PhotonView photonView;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        photonView = GetComponent<PhotonView>();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Update()
    {
        statusText.text = PhotonNetwork.NetworkClientState.ToString(); //���� ���� ���� Ŭ���̾�Ʈ�� ���°�
        RoomData();
        initStartButton();
    }

    //�Ʒ��� �޼ҵ���� bool ���� ��ȯ�ϹǷ� if���� �־� ���� ����� ���

    public void JoinServer()
    {
        if (PhotonNetwork.ConnectUsingSettings()) //���� ��Ʈ��ũ ������ ����
        {
            Debug.Log("Join Server Success");
        }
        else
        {
            Debug.Log("Join Server Fail");
        }
    }
    public void CreateRoom()
    {
        if (PhotonNetwork.CreateRoom(roomField.text.ToString(),new RoomOptions { MaxPlayers = 4})) //�ִ� �÷��̾� 4������ ���� ���� �� �̸��� room Field�� �ִ°ɷ�
        {
            Debug.Log("Create Room Success");
        }
        else
        {
            Debug.Log("Create Room Fail");
        }
    }

    public void JoinRoom()
    {
        if (PhotonNetwork.JoinRoom(roomField.text.ToString())) //room Field �� �ִ� �ؽ�Ʈ�� �����ִ� ���� ���� (������ �����)
        {
            Debug.Log("Join Room Success");
        }
        else
        {
            Debug.Log("Join Room Fail");
        }
    }

    public void ChangeNickName()
    {
        PhotonNetwork.NickName = nickName.text; //���� ��Ʈ��ũ�� �г����� ����
        nickNameText.text = PhotonNetwork.NickName;
    }

    public void RoomData() //Update ȣ�� -> ���� �濡 �����̝���
    {
        if (PhotonNetwork.InRoom)
        {
            roomData.text = $"{PhotonNetwork.CurrentRoom.Name} | ({PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers})\n";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) //�÷��̾� ����Ʈ�� �ҷ��ͼ� for�� ������
            {
                roomData.text += $"Player {i+1} {PhotonNetwork.PlayerList[i].NickName.ToString()}\n"; //�÷��̾� �̸����� �ϳ��� roomData�� ���ϱ�
            }
            roomData.text += $"is Master {PhotonNetwork.IsMasterClient}"; //Master Client (����)���� Ȯ��
            initStartButton();
        }
        else
        {
            roomData.text = "�濡 ������ �ʽ��ϴ�!"; //�濡 ������ �ʴٸ� �� �ؽ�Ʈ ȣ��
        }
    }

    public void initStartButton() //���� Master Client �����̸� start ��ư Ȱ��ȭ
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
    }
    public void GameStart() => photonView.RPC("RequestSceneChange", RpcTarget.AllBuffered, "Ingame");

    [PunRPC]
    private void RequestSceneChange(string sceneName) => PhotonNetwork.LoadLevel(sceneName);
}
