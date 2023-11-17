using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServitorTurnAnimation : MonoBehaviour
{
    public Servitor servitor;
    private void Awake()
    {
        servitor = transform.parent.GetComponent<Servitor>();
        foreAnimator.gameObject.SetActive(false);
    }
    public Animator baseAnimator;
    public Animator foreAnimator;
    public void StartTurn(AnimatorOverrideController overrideController)
    {
        foreAnimator.gameObject.SetActive(true);
        foreAnimator.runtimeAnimatorController = baseAnimator.runtimeAnimatorController;
        foreAnimator.Play("move");
        baseAnimator.runtimeAnimatorController = overrideController;
        baseAnimator.Play("birth");
    }

    public void Die()
    {
        baseAnimator.SetTrigger("die");
    }

    public void Eat()
    {
        baseAnimator.SetTrigger("eat");
    }

    public void FinishBirth()
    {
        foreAnimator.gameObject.SetActive(false);
    }
    public void FinishDie()
    {
        servitor.Die();
    }
}
