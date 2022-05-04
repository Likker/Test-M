using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : SingletonMB<WeaponManager>
{
	public List<WeaponWrapper> m_Weapons;
	public GameObject          m_WeaponDrop;
	
	public WeaponWrapper GetRandomWeapon()
	{
		return m_Weapons[Random.Range(0, m_Weapons.Count)];
	}

	public WeaponWrapper GetWeaponByID(int _ID)
	{
		return m_Weapons[_ID];
	}
}
