using System;
using CustomPackages;
using UnityEngine;

public class Enemy : Character
{
   // Cache
   private SphereCollider m_SphereCollider;
   
   protected override void Awake()
   {
      base.Awake();
      m_SphereCollider = GetComponent<SphereCollider>();
   }

   private void Start()
   {
      EnemyManager.Instance.Register(this);
      RegisterMap(m_SphereCollider.radius);
   }
   
   protected override void Die()
   {
      EnemyManager.Instance.Unregister(this);
      UnregisterMap();

      // Something for die animation ?
      Destroy(gameObject);
   }
}
