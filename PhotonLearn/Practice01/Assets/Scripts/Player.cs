using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Player : MonoBehaviourPunCallbacks
{
    public PhotonView photonView;
    public Rigidbody2D rb;

    public float moveSpeed;
    public float jumpPower;

    public float maxSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        Move();
        Jump();
    }

    [PunRPC]
    public void Move()
    {
        if (photonView.IsMine && rb.velocity.x < maxSpeed)
        {

            rb.AddForce(new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, 0));
        }
    }
    [PunRPC]
    public void Jump()
    {
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode2D.Impulse);
        }
    }
}
