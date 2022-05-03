using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : SingletonMB<WeaponManager>
{
	public List<WeaponDataObj> m_Weapons;

	public WeaponDataObj GetRandomWeapon()
	{
		return m_Weapons[Random.Range(0, m_Weapons.Count)];
	}
}
