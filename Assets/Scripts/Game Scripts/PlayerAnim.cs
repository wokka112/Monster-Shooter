using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Animator playerAnimator;

    // Awake is called to ensure animator is referenced before it's needed in parent object's scripts.
    private void Awake()
    {
        playerAnimator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Run()
    {
        SetRun(true);
    }

    public void Idle()
    {
        SetRun(false);
    }

    public void Shoot()
    {
        playerAnimator.SetTrigger("Shoot");
    }

    public void Die()
    {
        playerAnimator.SetBool("IsDead", true);
    }

    void SetRun(bool isRunningForward)
    {
        playerAnimator.SetBool("IsRunning", isRunningForward);
    }
}
