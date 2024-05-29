using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowCurrentProfile : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentProfile;

    void FixedUpdate()
    {
        currentProfile.text = PlayerPrefs.GetString("Profile" + Profile.CurrentPlayerIndex);
    }
}
