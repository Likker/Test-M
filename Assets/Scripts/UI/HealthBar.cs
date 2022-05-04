using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
   public Image m_LifeImage;
   public Image m_LifeDelayImage;
   
   // CACHE
   private Coroutine  m_DelayLifeCoroutine;
   private ICharacter m_ICharacter;
   
   private void Awake()
   {
      m_ICharacter       =  GetComponentInParent<ICharacter>();
      m_ICharacter.OnHit += Refresh;

      m_LifeImage.fillAmount      = 1.0f;
      m_LifeDelayImage.fillAmount = 1.0f;
   }

   private void OnDestroy()
   {
      m_ICharacter.OnHit -= Refresh;
   }

   private void Refresh(float _PercentLife)
   {
      m_LifeImage.fillAmount = _PercentLife;
      
      if (m_DelayLifeCoroutine != null)
         StopCoroutine(m_DelayLifeCoroutine);
      
      m_DelayLifeCoroutine = StartCoroutine(LifeCoroutine(0.35f));
   }

   private IEnumerator LifeCoroutine(float _Delay)
   {
      float time           = 0.0f;
      float baseAmoutDelay = m_LifeDelayImage.fillAmount;
      
      while (time < _Delay)
      {
         m_LifeDelayImage.fillAmount =  Mathf.Lerp(baseAmoutDelay, m_LifeImage.fillAmount, time / _Delay);
         time                        += Time.deltaTime;
         yield return null;
      }

      m_LifeDelayImage.fillAmount = m_LifeImage.fillAmount;
   }
}
