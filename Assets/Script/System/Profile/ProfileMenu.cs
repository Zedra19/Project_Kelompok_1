using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileMenu : MonoBehaviour
{
    public GameObject profile;
    public GameObject inputName;
    public GameObject inputField;
    public GameObject selectProfile;
    public Text profile1;
    public Text profile2;
    public Text profile3;
    // Start is called before the first frame update
    void Start()
    {
        kondisi_profile();
    }

    public void kondisi_profile()
    {
        if(string.IsNullOrEmpty(PlayerPrefs.GetString("Profile1")) && string.IsNullOrEmpty(PlayerPrefs.GetString("Profile2")) && string.IsNullOrEmpty(PlayerPrefs.GetString("Profile3")))
        {
            profile.SetActive(true);
            inputName.SetActive(true);
        }else
        {
            profile.SetActive(true);
            selectProfile.SetActive(true);
        }
    }
    void Update()
    {

        profile1.text = PlayerPrefs.GetString("Profile1");
        profile2.text = PlayerPrefs.GetString("Profile2");
        profile3.text = PlayerPrefs.GetString("Profile3");
    }

    public void SaveName()
    {
        var name = inputField.GetComponent<InputField>().text;
        if(string.IsNullOrEmpty(PlayerPrefs.GetString("Profile1"))){
            PlayerPrefs.SetString("Profile1", name);
            Profile.CurrentPlayerIndex = 1;
        }else if(!string.IsNullOrEmpty(PlayerPrefs.GetString("Profile1")) && string.IsNullOrEmpty(PlayerPrefs.GetString("Profile2"))){
            PlayerPrefs.SetString("Profile2", name);
            Profile.CurrentPlayerIndex = 2;
        }else if(!string.IsNullOrEmpty(PlayerPrefs.GetString("Profile2")) && string.IsNullOrEmpty(PlayerPrefs.GetString("Profile3"))){
            PlayerPrefs.SetString("Profile3", name);
            Profile.CurrentPlayerIndex = 3; 
        }else{
            Debug.Log("Profile is full");
        }
        inputField.GetComponent<InputField>().text = "";
        inputName.SetActive(false);
        profile.SetActive(false);
    }

    public void profile1select()
    {
        Profile.CurrentPlayerIndex = 1;
        if(string.IsNullOrEmpty(PlayerPrefs.GetString("Profile1"))){
            selectProfile.SetActive(false);
            inputName.SetActive(true);
        }else{
            selectProfile.SetActive(false);
            profile.SetActive(false);
        }
    }

    public void profile2select()
    {
        Profile.CurrentPlayerIndex = 2;
        if(string.IsNullOrEmpty(PlayerPrefs.GetString("Profile2"))){
            selectProfile.SetActive(false);
            inputName.SetActive(true);
        }else{
            selectProfile.SetActive(false);
            profile.SetActive(false);
        }
    }

    public void profile3select()
    {
        Profile.CurrentPlayerIndex = 3;
        if(string.IsNullOrEmpty(PlayerPrefs.GetString("Profile3"))){
            selectProfile.SetActive(false);
            inputName.SetActive(true);
        }else{
            selectProfile.SetActive(false);
            profile.SetActive(false);
        }
    }

    public void ConfirmProfile()
    {
        selectProfile.SetActive(false);
    }
}
