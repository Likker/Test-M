using System.Collections;
using System.Collections.Generic;
using CustomPackages;
using UnityEngine;

public class Enemy : MappedObject
{
   public int  m_Life;
   public bool m_Focusable { get; private set; }

   protected override void Awake()
   {
      base.Awake();
      m_Focusable = true;
      EnemyManager.Instance.Register(this);
      RegisterMap();
   }

   public bool Hit(int _Damage)
   {
      m_Life -= _Damage;
      if (m_Life <= 0)
      {
         Die();
         return true;
      }
      return false;
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