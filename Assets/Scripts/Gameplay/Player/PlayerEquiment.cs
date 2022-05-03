using System;
using UnityEngine;

public class PlayerEquiment : MonoBehaviour
{
	public Action OnEquipWeapon;
	
	// CACHE
	private WeaponScritableObject m_CurrentWeaponData;
	private GameObject            m_WeaponModel;
	
	private void Start()
	{
		Equip(WeaponManager.Instance.GetRandomWeapon());
		OnEquipWeapon?.Invoke();
	}

	private void Equip(WeaponDataObj _Weapon)
	{
		m_CurrentWeaponData = _Weapon.m_WeaponData;
		
		// Instantiate Weapon Model
	}

	public float GetAttackAnimationSpeed()
	{
		return  m_CurrentWeaponData != null ? m_CurrentWeaponData.m_AttackAnimationSpeed : 0.0f;
	}

	public float GetDamageAnimationTiming()
	{
		return  m_CurrentWeaponData != null ? m_CurrentWeaponData.m_DamageAnimationTiming : 0.0f;
	}

	public float GetMovementSpeed()
	{
		return m_CurrentWeaponData != null ? m_CurrentWeaponData.m_MovementSpeed : 0.0f;
	}

	public float GetAttackRange()
	{
		return m_CurrentWeaponData != null ? m_CurrentWeaponData.m_AttackRange : 0.0f;
	}
}
