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
        statusText.text = PhotonNetwork.NetworkClientState.ToString(); //지금 현제 포톤 클라이언트의 상태값
        RoomData();
        initStartButton();
    }

    //아래의 메소드들은 bool 값을 반환하므로 if문에 넣어 실행 결과를 출력

    public void JoinServer()
    {
        if (PhotonNetwork.ConnectUsingSettings()) //포톤 네트워크 서버에 접속
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
        if (PhotonNetwork.CreateRoom(roomField.text.ToString(),new RoomOptions { MaxPlayers = 4})) //최대 플레이어 4명으로 방을 만듦 방 이름은 room Field에 있는걸루
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
        if (PhotonNetwork.JoinRoom(roomField.text.ToString())) //room Field 에 있는 텍스트로 적혀있는 방을 들어가줌 (없으면 만들기)
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
        PhotonNetwork.NickName = nickName.text; //포톤 네트워크의 닉네임을 변경
        nickNameText.text = PhotonNetwork.NickName;
    }

    public void RoomData() //Update 호출 -> 만약 방에 들어와이씅면
    {
        if (PhotonNetwork.InRoom)
        {
            roomData.text = $"{PhotonNetwork.CurrentRoom.Name} | ({PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers})\n";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) //플레이어 리스트를 불러와서 for문 돌리기
            {
                roomData.text += $"Player {i+1} {PhotonNetwork.PlayerList[i].NickName.ToString()}\n"; //플레이어 이름들을 하나씩 roomData에 더하기
            }
            roomData.text += $"is Master {PhotonNetwork.IsMasterClient}"; //Master Client (방장)인지 확인
            initStartButton();
        }
        else
        {
            roomData.text = "방에 들어가있지 않습니다!"; //방에 들어가있지 않다면 요 텍스트 호출
        }
    }

    public void initStartButton() //만약 Master Client 방장이면 start 버튼 활성화
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
