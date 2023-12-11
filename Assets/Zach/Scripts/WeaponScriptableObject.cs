using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WeaponScriptableObject", order = 1)]
public class WeaponScriptableObject : ScriptableObject
{
    public Sprite m_spriteEquipped;
    public Sprite m_spriteUnequipped;
    public Sprite m_spriteDamaged;
    public Sprite m_spriteDestroyed;
    public GameObject m_breakParticles;
    public GameObject m_explodeParticles;
    public GameObject m_hitParticles;
    public AudioClip m_hitAudio;

    public int m_durability = 0;
    public float m_handleOffset = 0;
    public float m_handleVertOffset = 0.05f;
    public float m_rotationOffset = 0;
    public float m_damage = 0;
    public float m_knockback = 0;
    public bool m_explodeOnThrow = false;

    public bool m_ranged = false;
    public GameObject m_projectilePrefab;
}
