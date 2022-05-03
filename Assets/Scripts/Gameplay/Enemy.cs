using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   public int m_Life;

   private void Awake()
   {
      EnemyManager.Instance.Register(this);
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
      Destroy(gameObject);
      
      // Something for die animation ?
   }
}
