using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int hp = 1;

    public void Damage(int dmg)
    {
        hp -= dmg;
    }

    public int getHealth() { return hp; }
}
