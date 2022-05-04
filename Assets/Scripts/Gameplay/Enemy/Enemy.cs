using System;
using CustomPackages;
using UnityEngine;

public class Enemy : MappedObject, ICharacter
{
   public  int m_Life;
   private int m_MaxLife;
   
   // Cache
   private SphereCollider m_SphereCollider;
   private BumpEffect     m_BumpEffect;
   
   public Action<float> OnHit { get; set; }
   
   protected override void Awake()
   {
      base.Awake();
      m_SphereCollider = GetComponent<SphereCollider>();
      m_BumpEffect     = GetComponentInChildren<BumpEffect>();

      m_MaxLife = m_Life;
   }

   private void Start()
   {
      EnemyManager.Instance.Register(this);
      RegisterMap(m_SphereCollider.radius);
   }

   public void TakeDamage(int _Damage)
   {
      if (m_BumpEffect != null)
         m_BumpEffect.Play();
      
      m_Life -= _Damage;
      OnHit?.Invoke(Mathf.Clamp01((float)m_Life / (float)m_MaxLife));
      
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
