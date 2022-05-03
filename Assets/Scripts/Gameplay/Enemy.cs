using System.Collections;
using System.Collections.Generic;
using CustomPackages;
using UnityEngine;

public class Enemy : MappedObject
{
   public int m_Life;

   protected override void Awake()
   {
      base.Awake();
      EnemyManager.Instance.Register(this);
      RegisterMap();
   }

   public void Hit(int _Damage)
   {
      m_Life -= _Damage;
      if (m_Life <= 0)
         Die();
   }

   private void Die()
   {
      EnemyManager.Instance.Unregister(this);
      UnregisterMap();
      Destroy(gameObject);
      
      // Something for die animation ?
   }
}
