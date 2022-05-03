using System;
using System.Collections;
using System.Collections.Generic;
using CustomPackages;
using UnityEngine;

public class Player : SingletonMB<Player>
{
   private  float m_RadiusCollision;

   // CACHE
   private PlayerController m_PlayerController;
   private PlayerEquiment   m_PlayerEquiment;
   
   // BUFFER
   private bool             m_IsTargetingEnemy;
   private Enemy            m_TargetEnemy;
   private List<GameObject> m_SearchBuffer;
   private Vector3          m_DirectionNorm;
   
   private void Awake()
   {
      m_PlayerController = GetComponent<PlayerController>();
      m_PlayerEquiment   = GetComponent<PlayerEquiment>();
      m_SearchBuffer     = new List<GameObject>();
      m_IsTargetingEnemy = false;

      m_PlayerEquiment.OnEquipWeapon += OnEquipWeapon;
   }

   private void OnDestroy()
   {
      m_PlayerEquiment.OnEquipWeapon -= OnEquipWeapon;
   }
   
   private void OnEquipWeapon()
   {
      m_PlayerController.m_Speed = m_PlayerEquiment.GetMovementSpeed();
      m_RadiusCollision          = m_PlayerEquiment.GetAttackRange();
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

   private void OnDrawGizmos()
   {
      if (m_PlayerController == null)
         return;
      
      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(m_PlayerController.GetPosition(), m_RadiusCollision * m_RadiusCollision);
   }
}
