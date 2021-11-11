using System.Collections.Generic;
using UnityEngine;

public class PlayerModelsHandler : MonoBehaviour
{
   public List<PlayerModel> playerModels;
   private PlayerModel _currentModel;

   private void Awake()
   {
      foreach (PlayerModel playerModel in playerModels)
      {
         playerModel.gameObject.SetActive(false);
      }
      
      playerModels[0].gameObject.SetActive(true);
      _currentModel = playerModels[0];
   }

   private void Update()
   {
      if (RoomController.Instance.isGameStarted)
            return;
         
      if (Input.GetKeyDown(KeyCode.Alpha1))
      {
         ActivateModel(0);
      }
      if (Input.GetKeyDown(KeyCode.Alpha2))
      {
         ActivateModel(1);
      }
      if (Input.GetKeyDown(KeyCode.Alpha3))
      {
         ActivateModel(2);
      }
      if (Input.GetKeyDown(KeyCode.Alpha4))
      {
         ActivateModel(3);
      }
      if (Input.GetKeyDown(KeyCode.Alpha5))
      {
         ActivateModel(4);
      }
   }
   
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


