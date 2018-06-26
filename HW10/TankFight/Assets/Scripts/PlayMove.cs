using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayMove : NetworkBehaviour
{

    public GameObject bulletPrefab;

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKey(KeyCode.W))
        {
            moveForward();
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveBackWard();
        }
        float offsetX = Input.GetAxis("Horizontal");//获取水平轴上的增量，目的在于控制玩家坦克的转向
        turn(offsetX);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }

        // 相机跟随玩家坦克
        Camera.main.transform.position = new Vector3(this.transform.position.x, 15, this.transform.position.z);
    }

    [Command]
    void CmdFire()
    {
        // This [Command] code is run on the server!

        // create the bullet object locally
        var bullet = (GameObject)Instantiate(
             bulletPrefab,
             transform.position - 2 * transform.forward,
             Quaternion.identity);

        bullet.GetComponent<Rigidbody>().velocity = -transform.forward * 10;

        // spawn the bullet on the clients
        NetworkServer.Spawn(bullet);

        // when the bullet is destroyed on the server it will automaticaly be destroyed on clients
        Destroy(bullet, 2.0f);
    }

    public void moveForward()
    {
        this.GetComponent<Rigidbody>().velocity = this.transform.forward * 20;
    }
    public void moveBackWard()
    {
        this.GetComponent<Rigidbody>().velocity = this.transform.forward * -20;
    }
    public void turn(float offsetX)
    {//通过水平轴上的增量，改变玩家坦克的欧拉角，从而实现坦克转向
        float x = this.transform.localEulerAngles.y + offsetX * 5;
        float y = this.transform.localEulerAngles.x;
        this.transform.localEulerAngles = new Vector3(y, x, 0);
    }
}
