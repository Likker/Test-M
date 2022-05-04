using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrop : MonoBehaviour
{
   public List<int> m_WeaponIDs;

   // CACHE
   private int        m_ID;
   private GameObject m_InstantiatedObj;

   private void Start()
   {
      m_ID = m_WeaponIDs[Random.Range(0, m_WeaponIDs.Count)];
      WeaponWrapper weaponWrapper = (WeaponManager.Instance.GetWeaponByID(m_ID));
      m_InstantiatedObj                    = Instantiate(weaponWrapper.m_ModelWeapon, transform);
      m_InstantiatedObj.transform.position = transform.position + Vector3.up * 1f;
      transform.localScale = Vector3.one * 3.0f;
   }

   private void OnTriggerEnter(Collider _Collider)
   {
      if (_Collider.gameObject.layer == LayerMask.NameToLayer("Player"))
      {
         Player.Instance.Equip(m_ID);
         Destroy(m_InstantiatedObj);
         Destroy(gameObject);
      }
   }
}
