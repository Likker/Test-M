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
      m_PlayerController        =  GetComponent<PlayerController>();
      m_PlayerController.OnMove += OnMove;
      m_PlayerController.OnStop += OnStop;
      m_IsTargetingEnemy        =  false;
   }

   private void OnDestroy()
   {
      m_PlayerController.OnMove -= OnMove;
      m_PlayerController.OnStop -= OnStop;
   }

   private void OnMove()
   {
      m_IsTargetingEnemy = false;
      m_TargetEnemy      = null;
   }

   private void OnStop()
   {
      m_IsTargetingEnemy = true;
      m_TargetEnemy      = EnemyManager.Instance.GetClosestEnemy(transform.position);
   }

   private void Update()
   {
      if (!m_IsTargetingEnemy)
         return;
      
      m_DirectionNorm   = (m_TargetEnemy.transform.position - transform.position).normalized;
      m_DirectionNorm.y = 0.0f;

      transform.forward  =  m_DirectionNorm;
      transform.position += m_DirectionNorm * (m_PlayerController.m_Speed * Time.deltaTime);
   }
}
