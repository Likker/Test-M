using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipDamageWorld : MonoBehaviour
{
   [SerializeField] private TextMeshPro  m_Label;
   [SerializeField] private List<FXAnim> m_FXAnims;

   public void Play(int _ValueOnHit, Action _Callback)
   {
      m_Label.text = "-" + _ValueOnHit;

      int callbackCount = m_FXAnims.Count;
      foreach (var fxAnim in m_FXAnims)
         fxAnim.Play(() =>
         {
            --callbackCount;
            if (callbackCount == 0)
               _Callback?.Invoke();
         });
   }
}
