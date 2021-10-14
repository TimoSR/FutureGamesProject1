using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointManager : MonoBehaviour
{
   public List<RespawnPoint> respawnPoints = new List<RespawnPoint>();
   private RespawnPoint _currentRespawnPoint;

   private void Awake()
   {
      foreach (Transform item in transform)
      {
         respawnPoints.Add(item.GetComponent<RespawnPoint>());
      }
      _currentRespawnPoint = respawnPoints[0];
   }

   public void UpdateRespawnPoint(RespawnPoint newRespawnPoint)
   {
      _currentRespawnPoint.DisableRespawnPoint();
      _currentRespawnPoint = newRespawnPoint;
   }

   public void Respawn(GameObject objectToRespawn)
   {
      _currentRespawnPoint.RespawnPlayer();
      objectToRespawn.SetActive(true);
   }

   public void RespawnAt(RespawnPoint spawnPoint, GameObject playeGO)
   {

      spawnPoint.SetPlayerGO(playeGO);
      Respawn(playeGO);
   }

   public void ResetAllSpawnPoints()
   {
      foreach (var item in respawnPoints)
      {
         item.ResetRespawnPoint();
      }
      _currentRespawnPoint = respawnPoints[0];
   }
   
}
