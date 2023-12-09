using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawnScript : MonoBehaviour
{
    [SerializeField] private WeaponScriptableObject[] weaponTypes;
    [SerializeField] private GameObject m_weaponPrefab;

    private Transform[] children;
    private int randomInt;
    private GameObject spawnedWeapon;
    private WeaponScriptableObject weaponType;

    private void Awake()
    {
        children = GetComponentsInChildren<Transform>();
        beginSpawning();
    }

    private void beginSpawning()
    {
        StartCoroutine(spawnWeapon());
    }

    private void stopSpawning()
    {
        StopCoroutine(spawnWeapon());
    }

    private IEnumerator spawnWeapon()
    {
        while (true)
        {
            randomInt = Random.Range(1, children.Length);
            spawnedWeapon = Instantiate(m_weaponPrefab, children[randomInt].position, children[randomInt].rotation);
            weaponType = weaponTypes[Random.Range(0, weaponTypes.Length)];
            spawnedWeapon.GetComponent<Throwable>().Init(weaponType);
            yield return new WaitForSeconds(5);
        }
    }
}
