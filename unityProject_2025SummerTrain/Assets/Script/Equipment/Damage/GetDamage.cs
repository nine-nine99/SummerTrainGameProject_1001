using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDamage : MonoBehaviour, IGetDamage
{
    private float damage = 3; // 伤害值
    public float GetDamageValue()
    {
        return damage;
    }
    public void SetDamageValue(float value)
    {
        damage = value;
    }
}
