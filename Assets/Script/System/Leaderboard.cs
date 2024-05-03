using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public GameObject EasyButton;
    public GameObject MediumButton;
    public GameObject HardButton;

    private Animator easyButtonAnimator;
    private Animator mediumButtonAnimator;
    private Animator hardButtonAnimator;

    void Start()
    {
        // Dapatkan komponen Animator dari tombol-tombol
        easyButtonAnimator = EasyButton.GetComponent<Animator>();
        mediumButtonAnimator = MediumButton.GetComponent<Animator>();
        hardButtonAnimator = HardButton.GetComponent<Animator>();
    }

    public void Display()
    {
        // Reset state animasi tombol ke state normal
        ResetButtonAnimation(easyButtonAnimator);
        ResetButtonAnimation(mediumButtonAnimator);
        ResetButtonAnimation(hardButtonAnimator);
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
