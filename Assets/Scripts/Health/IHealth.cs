using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public int currentHP { get; }
    public int damageOnCollision { get; }

    public void ApplyDamage(int amount);
}
