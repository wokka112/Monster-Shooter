using UnityEngine;

public class MeleeAttack : StateMachineBehaviour
{
    public float damageDelay = 1.0f;
    float timeSinceAnimStart = 0f;
    bool dealtDamage = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeSinceAnimStart = 0;
        dealtDamage = false;
        EnemyController enemy = animator.GetComponentInParent<EnemyController>();
        enemy.BroadcastMessage("StopMovement");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeSinceAnimStart += Time.deltaTime;

        if (!dealtDamage 
            && timeSinceAnimStart >= damageDelay)
        {
            EnemyController enemy = animator.GetComponentInParent<EnemyController>();
            enemy.BroadcastMessage("DamagePlayer");
            dealtDamage = true;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        EnemyController enemy = animator.GetComponentInParent<EnemyController>();
        enemy.BroadcastMessage("StartMovement");
    }
}
