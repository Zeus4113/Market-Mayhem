using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
	void SetHealth(float health);
	void Damage(float damage);
	void IsAlive();

}
