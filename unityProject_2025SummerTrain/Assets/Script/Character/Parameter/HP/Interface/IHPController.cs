using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHPController
{
    // 受伤
    void Hurt(float damage, GameObject attacker = null);
}
