using System.Collections;
using System.Collections.Generic;
using CustomPackages;
using UnityEngine;

public class Enemy : MappedObject
{
   public int  m_Life;

   // Cache
   private SphereCollider m_SphereCollider;
   private BumpEffect     m_BumpEffect;
   
   protected override void Awake()
   {
      base.Awake();
      m_SphereCollider = GetComponent<SphereCollider>();
      m_BumpEffect     = GetComponent<BumpEffect>();
   }

   private void Start()
   {
      EnemyManager.Instance.Register(this);
      RegisterMap(m_SphereCollider.radius);
   }

   public void Hit(int _Damage)
   {
      m_BumpEffect.Play();
      m_Life -= _Damage;
      if (m_Life <= 0)
         Die();
   }

   private void Die()
   {
      EnemyManager.Instance.Unregister(this);
      UnregisterMap();
      Player.Instance.SearchEnemy();
      
      // Something for die animation ?
      // Pooling ?
      Destroy(gameObject);
   }
}
