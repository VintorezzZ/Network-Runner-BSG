using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PhotonSynchronization : MonoBehaviour, IPunObservable
{
    #region IPunObservable implementation

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
            // Network player, receive data
            // Math.abs  Vector3.Dot(); если отлично от 1 или -1 
            
            // receive all transfrom vectors from client
            Vector3 receivedPosition = (Vector3)stream.ReceiveNext();
            Vector3 position = transform.position;
            
            // check if z axis of player on one direction with world z axis
            if (Mathf.Abs(Vector3.Dot(transform.forward, Vector3.forward)) == 1)   // comparison with float can lose value
            {
                // if true, sync copy z pos with client z poz
                //position.z = 
                Mathf.Lerp(position.x, receivedPosition.x, 10 * Time.deltaTime);
                position.y = receivedPosition.y;
            }
            else
            {
                // if true, sync x poz with client x pos
                //position.x = 
                Mathf.Lerp(position.z, receivedPosition.z, 10 * Time.deltaTime);
                position.y = receivedPosition.y;
                
            }
            
            //this.transform.position = 
            this.transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }

    #endregion
}
