using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public GameObject StoryButton;
    public GameObject EndlessButton;

    private Animator StoryButtonAnimator;
    private Animator EndlessButtonAnimator;

    void Start()
    {
        // Dapatkan komponen Animator dari tombol-tombol
        StoryButtonAnimator = StoryButton.GetComponent<Animator>();
        EndlessButtonAnimator = EndlessButton.GetComponent<Animator>();
    }

    public void Display()
    {
        // Reset state animasi tombol ke state normal
        ResetButtonAnimation(StoryButtonAnimator);
        ResetButtonAnimation(EndlessButtonAnimator);
    }

    private void ResetButtonAnimation(Animator buttonAnimator)
    {
        // Cek jika animator tidak null
        if (buttonAnimator != null)
        {
            // Set parameter "Pressed" ke false untuk kembali ke state normal
            buttonAnimator.SetBool("Pressed", false);
        }
        else
        {
            Debug.LogWarning("Animator component not found!");
        }
    }
}
