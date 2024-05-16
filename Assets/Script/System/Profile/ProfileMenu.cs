using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileMenu : MonoBehaviour
{
    [SerializeField] GameObject profile;
    [SerializeField] GameObject inputName;
    [SerializeField] GameObject inputField;
    [SerializeField] GameObject selectProfile;
    [SerializeField] GameObject deleteProfile;
    [SerializeField] GameObject noProfile;
    [SerializeField] Text profile1;
    [SerializeField] Text profile2;
    [SerializeField] Text profile3;
    [SerializeField] Text profile1DelText;
    [SerializeField] Text profile2DelText;
    [SerializeField] Text profile3DelText;
    // Start is called before the first frame update
    void Start()
    {
        kondisi_profile();
    }

    public void kondisi_profile()
    {
        if(Profile.firstGame == true && string.IsNullOrEmpty(PlayerPrefs.GetString("Profile1")) && string.IsNullOrEmpty(PlayerPrefs.GetString("Profile2")) && string.IsNullOrEmpty(PlayerPrefs.GetString("Profile3")))
        {
            profile.SetActive(true);
            inputName.SetActive(true);
            Profile.firstGame = false;
        }else if(Profile.firstGame == true && (!string.IsNullOrEmpty(PlayerPrefs.GetString("Profile1")) || !string.IsNullOrEmpty(PlayerPrefs.GetString("Profile2")) || !string.IsNullOrEmpty(PlayerPrefs.GetString("Profile3")))){
            profile.SetActive(true);
            selectProfile.SetActive(true);
            Profile.firstGame = false;
        }
    }

    public void profileButton()
    {
        profile.SetActive(true);
        selectProfile.SetActive(true);
    }
    
    void Update()
    {
        if(selectProfile.activeSelf || deleteProfile.activeSelf){
            profile1.text = PlayerPrefs.GetString("Profile1");
            profile2.text = PlayerPrefs.GetString("Profile2");
            profile3.text = PlayerPrefs.GetString("Profile3");
            profile1DelText.text = PlayerPrefs.GetString("Profile1");
            profile2DelText.text = PlayerPrefs.GetString("Profile2");
            profile3DelText.text = PlayerPrefs.GetString("Profile3");
        }
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
        selectProfile.SetActive(true);
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

    public void DeleteProfile()
    {
        selectProfile.SetActive(false);
        deleteProfile.SetActive(true);
    }

    public void profile1Delete()
    {
        if(!string.IsNullOrEmpty(PlayerPrefs.GetString("Profile1"))){
            Profile.CurrentPlayerIndex = 0;
            PlayerPrefs.DeleteKey("Profile1");
            deleteProfile.SetActive(false);
            selectProfile.SetActive(true);
        }else{
            deleteProfile.SetActive(false);
            noProfile.SetActive(true);
        }
    }

    public void profile2Delete()
    {
        if(!string.IsNullOrEmpty(PlayerPrefs.GetString("Profile2"))){
            Profile.CurrentPlayerIndex = 0;
            PlayerPrefs.DeleteKey("Profile2");
            deleteProfile.SetActive(false);
            selectProfile.SetActive(true);
        }else{
            deleteProfile.SetActive(false);
            noProfile.SetActive(true);
        }
    }

    public void profile3Delete()
    {
        if(!string.IsNullOrEmpty(PlayerPrefs.GetString("Profile3"))){
            Profile.CurrentPlayerIndex = 0;
            PlayerPrefs.DeleteKey("Profile3");
            deleteProfile.SetActive(false);
            selectProfile.SetActive(true);
        }else{
            deleteProfile.SetActive(false);
            noProfile.SetActive(true);
        }
    }
    
    public void backToSelectProfile(){
        deleteProfile.SetActive(false);
        selectProfile.SetActive(true);
    }

    public void backToDeleteProfile(){
        noProfile.SetActive(false);
        deleteProfile.SetActive(true);
    }
}
