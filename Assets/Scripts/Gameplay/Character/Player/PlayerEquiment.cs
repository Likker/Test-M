using System;
using UnityEngine;

public class PlayerEquiment : MonoBehaviour
{
	[Header("CONFIG")]
	public Transform m_WeaponSlot;
	
	public Action OnEquipWeapon;
	
	// CACHE
	private WeaponScritableObject m_CurrentWeaponData;
	private GameObject            m_InstantiatedWeaponModel;
	
	private void Start()
	{
		Equip(WeaponManager.Instance.GetRandomWeapon());
		
		OnEquipWeapon?.Invoke();
	}

	private void Equip(WeaponDataObj _Weapon)
	{
		m_CurrentWeaponData = _Weapon.m_WeaponData;

		m_InstantiatedWeaponModel                         = Instantiate(_Weapon.m_ModelWeapon, m_WeaponSlot);
		m_InstantiatedWeaponModel.transform.localPosition = Vector3.zero;
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
