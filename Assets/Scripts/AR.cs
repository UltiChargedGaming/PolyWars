using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AR : MonoBehaviour
{
	public float fireRate = 0.2f;
	public int damage = 33;
	public int range = 45;
	public int ammo = 30;
	public bool state = false;
	public string name = "Ar";

	public void Init(float fireRate,int damage,int range,int ammo,bool state,string name)
	{
		this.fireRate = fireRate;
		this.damage = damage;
		this.range = range;
		this.ammo = ammo;
		this.state = state;
		this.name = name;
	}
		
}





