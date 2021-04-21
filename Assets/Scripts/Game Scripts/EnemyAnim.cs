using UnityEngine;

public class EnemyAnim : MonoBehaviour
{
    private Animator enemyAnimator;

    // Awake is called to ensure animator is referenced before it is needed in parent object's scripts.
    private void Awake()
    {
        enemyAnimator = GetComponentInChildren<Animator>();
    }

    // Play melee attack anim once
    public void MeleeAttack()
    {
        enemyAnimator.SetTrigger("MeleeAttack");
    }

    // Play ranged attack anim once
    public void RangedAttack()
    {
        enemyAnimator.SetTrigger("RangedAttack");
    }

    // Start the running animation
    public void StartRunning()
    {
        SetIsRunning(true);
    }

    // Stop the running animation 
    public void StopRunning()
    {
        SetIsRunning(false);
    }

    // Play die animation
    public void Die()
    {
        enemyAnimator.SetTrigger("Die");
    }

    // Play looping victory animation
    public void Victory()
    {
        enemyAnimator.SetTrigger("Victory");
    }

    private void SetIsRunning(bool isRunning)
    {
        enemyAnimator.SetBool("IsRunning", isRunning);
    }
}
