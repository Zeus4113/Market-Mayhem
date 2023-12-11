using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawnScript : MonoBehaviour
{
    [SerializeField] private WeaponScriptableObject[] weaponTypes;
    [SerializeField] private GameObject m_weaponPrefab;
	[SerializeField] private float m_spawnDelay = 1f;
	[SerializeField] private int m_spawnAmount;

    private Transform[] children;
    private int randomInt;
    private GameObject spawnedWeapon;
    private WeaponScriptableObject weaponType;

	public void Init()
	{
		children = GetComponentsInChildren<Transform>();
	}

	bool C_IsSpawning = false;
	Coroutine C_Spawning;

    public void BeginSpawning()
    {
		if (C_IsSpawning) return;
		C_IsSpawning = true;

		if (C_Spawning != null) return;
        C_Spawning = StartCoroutine(SpawnWeapon());
    }

    private void StopSpawning()
    {
		if (!C_IsSpawning) return;
		C_IsSpawning = false;

		if (C_Spawning == null) return;
		StopCoroutine(C_Spawning);
    }

    private IEnumerator SpawnWeapon()
    {
        while (C_IsSpawning)
        {
            randomInt = UnityEngine.Random.Range(1, children.Length);
            spawnedWeapon = Instantiate(m_weaponPrefab, children[randomInt].position, children[randomInt].rotation);
            weaponType = weaponTypes[UnityEngine.Random.Range(0, weaponTypes.Length)];
            spawnedWeapon.GetComponent<Throwable>().Init(weaponType);        

            yield return new WaitForSeconds(m_spawnDelay);
        }
    }
}
