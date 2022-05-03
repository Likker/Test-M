using System;
using System.Collections;
using System.Collections.Generic;
using CustomPackages;
using UnityEngine;

public enum EPlayerState
{
   NONE,
   ATTACK
}

public class Player : SingletonMB<Player>
{
   private       float m_RadiusCollision;
   private const float c_OffsetRadius = 2.0f;

   private EPlayerState m_PlayerState;
   
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
      m_PlayerState      = EPlayerState.NONE;
      
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
      m_CircleRenderer.SetRadius(m_RadiusCollision);
   }
   
   public void SearchEnemy()
   {
      if (m_IsTargetingEnemy == false)
         return;
      m_TargetEnemy = EnemyManager.Instance.GetClosestEnemy(transform.position);
   }
   
   private void Attack(Enemy _Enemy)
   {
      m_PlayerState = EPlayerState.ATTACK;
      StartCoroutine(AttackCoroutine(_Enemy, () =>
      {
         m_PlayerState = EPlayerState.NONE;
      }));
   }

   private void Update()
   {
      if (m_PlayerController.IsMoving())
         return;

      switch (m_PlayerState)
      {
         case EPlayerState.NONE:
         {
            if (!MappingManager.Instance.IsEntityPresent(m_PlayerController.GetPosition(), GetRadiusDetection()))
               return;

            MappingManager.Instance.FindEntities(m_PlayerController.GetPosition(), GetRadiusDetection(), ref m_SearchBuffer);
            if (m_SearchBuffer.Count > 0)
               Attack(GetClosestEnemy());
            break;
         }
         case EPlayerState.ATTACK:
            break;
      }
   }

   private Enemy GetClosestEnemy()
   {
      float        maxDistance  = Mathf.Infinity;
      MappedObject mappedObject = null;

      for (int i = 0; i < m_SearchBuffer.Count; i++)
      {
         float distance = Vector3.Distance(m_PlayerController.GetPosition(), m_SearchBuffer[i].transform.position);
         if (distance < maxDistance)
         {
            maxDistance  = distance;
            mappedObject = m_SearchBuffer[i];
         }
      }

      return mappedObject.GetComponent<Enemy>();
   }

   private float GetRadiusDetection()
   {
      return m_RadiusCollision;
   }

   private IEnumerator AttackCoroutine(Enemy _Enemy, Action _Callback)
   {
      yield return new WaitForSeconds(m_PlayerEquiment.GetDamageAnimationTiming());
      _Enemy.Hit(1);
      Debug.Log("HIT");
      yield return new WaitForSeconds(1.0f / m_PlayerEquiment.GetAttackAnimationSpeed() - m_PlayerEquiment.GetDamageAnimationTiming());
      Debug.Log("RESET ATTACK");
      _Callback?.Invoke();
   }

   private void OnDrawGizmos()
   {
      if (m_PlayerController == null)
         return;
      
      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(m_PlayerController.GetPosition(), m_RadiusCollision * m_RadiusCollision);
   }
}
