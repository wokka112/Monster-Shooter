using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int hp = 1;

    private EnemyController enemyControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        enemyControllerScript = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int dmg)
    {
        hp -= dmg;

        if (hp <= 0)
        {
            enemyControllerScript.Die();
        } else
        {
            enemyControllerScript.GetHit();
        }
    }
}
