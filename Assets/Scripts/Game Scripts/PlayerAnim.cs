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

    // Start running animation
    public void Run()
    {
        SetRun(true);
    }

    // Start idle animation
    public void Idle()
    {
        SetRun(false);
    }

    // Play shoot animation once
    public void Shoot()
    {
        playerAnimator.SetTrigger("Shoot");
    }

    // Play die animation
    public void Die()
    {
        playerAnimator.SetBool("IsDead", true);
    }

    private void SetRun(bool isRunning)
    {
        playerAnimator.SetBool("IsRunning", isRunning);
    }
}
