using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 0, fileName = "Weapon", menuName = "Data/Weapon")]
public class WeaponScritableObject : ScriptableObject
{
	public float m_AttackAnimationSpeed;
	public float m_DamageAnimationTiming;
	public float m_MovementSpeed;
	public float m_AttackRange;
}
