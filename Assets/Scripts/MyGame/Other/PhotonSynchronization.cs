using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PhotonSynchronization : MonoBehaviour, IPunObservable
{
    Vector3 receivedPosition = Vector3.zero;
    Quaternion receivedRotation = Quaternion.identity;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            
            receivedPosition = (Vector3)stream.ReceiveNext();
            receivedRotation = (Quaternion)stream.ReceiveNext();
        }
    }
  
    private void Update()
    {
        if(GetComponent<PhotonView>().IsMine)
            return;
        
        Vector3 tmpPos = transform.position;
        tmpPos = Vector3.Lerp(tmpPos, receivedPosition, 10 * Time.deltaTime);

        transform.rotation = Quaternion.Lerp(transform.rotation, receivedRotation, 10 * Time.deltaTime);

        if (Mathf.Abs(Vector3.Dot(RoomController.instance.myPlayer.transform.forward, Vector3.forward)).AlmostEquals(1f, 0.001f))   // comparison with float can lose value
        {
            tmpPos.z = RoomController.instance.myPlayer.transform.position.z;
        }
        else
        {
            tmpPos.x = RoomController.instance.myPlayer.transform.position.x;
        }
        transform.position = tmpPos;
    }
}
