using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WeaponScriptableObject", order = 1)]
public class WeaponScriptableObject : ScriptableObject
{
    public Sprite m_spriteEquipped;
    public Sprite m_spriteUnequipped;
    public int m_durability;
    public float m_handleOffset;
    public float m_rotationOffset;
    public float m_damage;
    public float m_knockback;

    public bool m_ranged;
    public Sprite m_projectileSprite;
}
