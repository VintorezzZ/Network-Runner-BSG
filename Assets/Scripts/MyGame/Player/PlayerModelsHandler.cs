using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerModelsHandler : MonoBehaviour
{
   public List<PlayerModel> playerModels;
   private PlayerModel _currentModel;
   private PhotonView _photonView;

   private void Awake()
   {
      _photonView = GetComponent<PhotonView>();
      
      foreach (PlayerModel playerModel in playerModels)
      {
         playerModel.gameObject.SetActive(false);
      }
      
      playerModels[0].gameObject.SetActive(true);
      _currentModel = playerModels[0];
   }

   private void Update()
   {
      if (_photonView.IsMine)
      {
         if (RoomController.instance.isGameStarted)
            return;
         
         if (Input.GetKeyDown(KeyCode.Alpha1))
         {
            ActivateModel(0);
            _photonView.RPC("ActivateModel", RpcTarget.Others,0);
         }
         if (Input.GetKeyDown(KeyCode.Alpha2))
         {
            ActivateModel(1);
            _photonView.RPC("ActivateModel", RpcTarget.Others,1);
         }
         if (Input.GetKeyDown(KeyCode.Alpha3))
         {
            ActivateModel(2);
            _photonView.RPC("ActivateModel", RpcTarget.Others,2);
         }
         if (Input.GetKeyDown(KeyCode.Alpha4))
         {
            ActivateModel(3);
            _photonView.RPC("ActivateModel", RpcTarget.Others,3);
         }
         if (Input.GetKeyDown(KeyCode.Alpha5))
         {
            ActivateModel(4);
            _photonView.RPC("ActivateModel", RpcTarget.Others,4);
         }
      }
   }

   [PunRPC]
   private void ActivateModel(int chosenModel)
   {
      if (_currentModel != playerModels[chosenModel])
      {
         DeActivateCurrentModel();
         
         _currentModel = playerModels[chosenModel];
         playerModels[chosenModel].gameObject.SetActive(true);
      }
   }

   private void DeActivateCurrentModel()
   {
      PlayerModel previousModel = _currentModel;
      previousModel.gameObject.SetActive(false);
   }
}


