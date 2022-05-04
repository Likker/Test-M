using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Enemy : Character
{
   // Cache
   private SphereCollider m_SphereCollider;
   
   protected override void Awake()
   {
      base.Awake();
      m_SphereCollider = GetComponent<SphereCollider>();
   }

   protected virtual void Start()
   {
      EnemyManager.Instance.Register(this);
      RegisterMap(m_SphereCollider.radius);
   }
   
   protected override void Die()
   {
      if (Random.value < 1.0f)
      {
         Instantiate(WeaponManager.Instance.m_WeaponDrop, transform.position, quaternion.identity);
      }
      
      EnemyManager.Instance.Unregister(this);
      UnregisterMap();

      base.Die();
   }
}
