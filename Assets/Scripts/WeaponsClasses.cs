using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsClasses : MonoBehaviour {

	public class SMG1 : WeaponsClasses {
		public float fireRate = 0.1f;
		public int damage = 6;
		public int range = 30;
		public int ammo = 35;
		public bool state = false;
		public string gunName = "Smg";
	}
		
	public class AR1 : WeaponsClasses {
		public float fireRate = 0.2f;
		public int damage = 12;
		public int range = 45;
		public int ammo = 30;
		public bool state = false;
		public string gunName = "Ar";
	}

	public SMG1 smg1;
	public AR1 ar1;
}
