using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnim : MonoBehaviour
{
    private Animator enemyAnimator;

    // Awake is called to ensure animator is referenced before it is needed in parent object's scripts.
    private void Awake()
    {
        enemyAnimator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void MeleeAttack()
    {
        enemyAnimator.SetTrigger("MeleeAttack");
    }

    public void RangedAttack()
    {
        enemyAnimator.SetTrigger("RangedAttack");
    }

    public void StartRunning()
    {
        SetIsRunning(true);
    }

    public void StopRunning()
    {
        SetIsRunning(false);
    }

    public void Die()
    {
        enemyAnimator.SetTrigger("Die");
    }

    public void Victory()
    {
        enemyAnimator.SetTrigger("Victory");
    }

    void SetIsRunning(bool isRunning)
    {
        enemyAnimator.SetBool("IsRunning", isRunning);
    }
}
