using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointManager : MonoBehaviour
{
   private List<RespawnPoint> _respawnPoints = new List<RespawnPoint>();
   private RespawnPoint currentRespawnPoint;

   private void Awake()
   {

      foreach (Transform item in transform)
      {
         _respawnPoints.Add(item.GetComponent<RespawnPoint>());
      }

      currentRespawnPoint = _respawnPoints[0];
      
   }

   public void UpdateRespawnPoint(RespawnPoint newRespawnPoint)
   {
      currentRespawnPoint.DisableRespawnPoint();
      currentRespawnPoint = newRespawnPoint;
   }

   public void Respawn(GameObject objectToRespawn)
   {
      currentRespawnPoint.RespawnPlayer();
      objectToRespawn.SetActive(true);
   }

   public void RespawnAt(RespawnPoint respawnPoint, GameObject playerGO)
   {
      respawnPoint.SetPlayerGO(playerGO);
      Respawn(playerGO);
   }

   public void ResetAllSpawnPoint()
   {
      foreach (var item in _respawnPoints)
      {
         item.ResetRespawnPoint();;
      }

      currentRespawnPoint = _respawnPoints[0];
   }
   
}
