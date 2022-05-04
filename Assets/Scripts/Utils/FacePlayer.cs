using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
   public float     m_SpeedLerp = 5.0f;
   private Transform m_PlayerTransform;
   
   // Cache
   private Vector3 m_CacheDir;
   
   private void Awake()
   {
      
   }

   private void LateUpdate()
   {
      if (Player.Instance == null)
         return;
      
      if (m_PlayerTransform == null)
         m_PlayerTransform = Player.Instance.transform;
      
      m_CacheDir        = m_PlayerTransform.transform.position - transform.position;
      m_CacheDir.y      = 0.0f;
      transform.forward = Vector3.Lerp(transform.forward, m_CacheDir, Time.deltaTime * m_SpeedLerp);
   }
}
