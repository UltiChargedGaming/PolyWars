using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SMG : MonoBehaviour
{
	public float fireRate = 0.1f;
	public int damage = 15;
	public int range = 30;
	public int ammo = 35;
	public bool state = false;
	public string name = "Smg";

	public void Init(float fireRate,int damage,int range,int ammo,bool state,string name)
	{
		this.fireRate = fireRate;
		this.damage = damage;
		this.range = range;
		this.ammo = ammo;
		this.state = state;
		this.name = name;
	}

	public SMG smg;
}





