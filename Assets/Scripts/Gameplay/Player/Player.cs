using System;
using System.Collections;
using System.Collections.Generic;
using CustomPackages;
using UnityEngine;

public class Player : SingletonMB<Player>
{
   private       float m_RadiusCollision;
   private const float c_OffsetRadius = 2.0f;
   
   // CACHE
   private PlayerController   m_PlayerController;
   private PlayerEquiment     m_PlayerEquiment;
   private CircleLineRenderer m_CircleRenderer;
   
   // BUFFER
   private bool             m_IsTargetingEnemy;
   private Enemy            m_TargetEnemy;
   private List<MappedObject> m_SearchBuffer;
   private Vector3          m_DirectionNorm;
   
   private void Awake()
   {
      m_PlayerController = GetComponent<PlayerController>();
      m_PlayerEquiment   = GetComponent<PlayerEquiment>();
      m_CircleRenderer   = GetComponentInChildren<CircleLineRenderer>();
      m_SearchBuffer     = new List<MappedObject>();
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
      m_CircleRenderer.SetRadius(m_RadiusCollision * m_RadiusCollision);
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
      if (!MappingManager.Instance.IsEntityPresent(m_PlayerController.GetPosition(), GetRadiusDetection()))
         return;
      
      MappingManager.Instance.FindEntities(m_PlayerController.GetPosition(), GetRadiusDetection(), ref m_SearchBuffer);
      if (m_SearchBuffer.Count > 0)
         Attack();
   }

   private float GetRadiusDetection()
   {
      return m_RadiusCollision * m_RadiusCollision;
   }
   
   private void OnDrawGizmos()
   {
      if (m_PlayerController == null)
         return;
      
      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(m_PlayerController.GetPosition(), m_RadiusCollision * m_RadiusCollision);
   }
}
