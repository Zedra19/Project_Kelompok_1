using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalAreaAnim : MonoBehaviour
{
    [SerializeField] private Animator _portalAnimator;
    [SerializeField] private string _animationParamName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _portalAnimator.SetBool(_animationParamName, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _portalAnimator.SetBool(_animationParamName, false);
        }
    }
}
