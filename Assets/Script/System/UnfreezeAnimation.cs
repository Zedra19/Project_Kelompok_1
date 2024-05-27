using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnfreezeAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.Update(Time.unscaledDeltaTime);
    }
}
