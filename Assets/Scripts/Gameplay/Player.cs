using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   private bool m_IsTargetingEnemy;
   
   // CACHE
   private PlayerController m_PlayerController;
   
   // BUFFER
   private Enemy   m_TargetEnemy;
   private Vector3 m_DirectionNorm;
   
   
   private void Awake()
   {
      m_PlayerController             =  GetComponent<PlayerController>();
      m_PlayerController.OnStartMove += OnStartMove;
      m_PlayerController.OnStopMove  += OnStopMove;
      m_IsTargetingEnemy             =  false;
   }

   private void OnDestroy()
   {
      m_PlayerController.OnStartMove -= OnStartMove;
      m_PlayerController.OnStopMove  -= OnStopMove;
   }

   private void OnStartMove()
   {
      m_IsTargetingEnemy = false;
      m_TargetEnemy      = null;
   }

   private void OnStopMove()
   {
      m_IsTargetingEnemy = true;
      m_TargetEnemy      = EnemyManager.Instance.GetClosestEnemy(transform.position);
   }

   private void FixedUpdate()
   {
      if (!m_IsTargetingEnemy)
         return;
      
      m_DirectionNorm   = (m_TargetEnemy.transform.position - m_PlayerController.GetPosition()).normalized;
      m_DirectionNorm.y = 0.0f;

      m_PlayerController.Move(m_DirectionNorm);
   }
}
