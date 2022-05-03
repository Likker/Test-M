using System;
using System.Collections;
using System.Collections.Generic;
using CustomPackages;
using UnityEngine;

public class Player : SingletonMB<Player>
{
   [Header("CONFIG")]
   public  float m_RadiusCollision;

   // CACHE
   private PlayerController m_PlayerController;
   
   // BUFFER
   private bool             m_IsTargetingEnemy;
   private Enemy            m_TargetEnemy;
   private List<GameObject> m_SearchBuffer;
   private Vector3          m_DirectionNorm;
   
   private void Awake()
   {
      m_PlayerController             =  GetComponent<PlayerController>();
      m_SearchBuffer                 =  new List<GameObject>();
      m_IsTargetingEnemy             =  false;
      
      m_PlayerController.OnStartMove += OnStartMove;
      m_PlayerController.OnStopMove  += OnStopMove;
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
      SearchEnemy();
   }

   public void SearchEnemy()
   {
      if (m_IsTargetingEnemy == false)
         return;
      m_TargetEnemy = EnemyManager.Instance.GetClosestEnemy(transform.position);
   }
   
   private void Attack()
   {
      Debug.Log("ATTACK");
   }
   
   private void Update()
   {
      MappingManager.Instance.FindEntities(m_PlayerController.GetPosition(), m_RadiusCollision * m_RadiusCollision, ref m_SearchBuffer);
      if (m_SearchBuffer.Count > 0)
         Attack();
   }
   
   private void FixedUpdate()
   {
      if (!m_IsTargetingEnemy)
         return;
      
      m_DirectionNorm   = (m_TargetEnemy.transform.position - m_PlayerController.GetPosition()).normalized;
      m_DirectionNorm.y = 0.0f;

      m_PlayerController.Move(m_DirectionNorm);
   }

   private void OnDrawGizmos()
   {
      if (m_PlayerController == null)
         return;
      
      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(m_PlayerController.GetPosition(), m_RadiusCollision * m_RadiusCollision);
   }
}
