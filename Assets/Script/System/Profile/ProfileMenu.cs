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
    // Start is called before the first frame update
    void Start()
    {
        if(Profile.CurrentPlayerIndex == 0)
        {
            inputName.SetActive(true);
        }else
        {
            selectProfile.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveName()
    {
        var name = inputField.GetComponent<InputField>().text;
        Debug.Log(PlayerPrefs.GetString("Profile1").ToString());
        if(PlayerPrefs.GetString("Profile1") == null){
            PlayerPrefs.SetString("Profile1", name);
            if(Profile.CurrentPlayerIndex == 0){
                Profile.CurrentPlayerIndex = 1;
            }
            profile.SetActive(false);
            
        }else if(PlayerPrefs.GetString("Profile1") != null && PlayerPrefs.GetString("Profile2") == null){
            PlayerPrefs.SetString("Profile2", name);
            if(Profile.CurrentPlayerIndex == 0){
                Profile.CurrentPlayerIndex = 2;
                
            }
            profile.SetActive(false);
        }else if(PlayerPrefs.GetString("Profile2") != null && PlayerPrefs.GetString("Profile3") == null){
            PlayerPrefs.SetString("Profile3", name);
            if(Profile.CurrentPlayerIndex == 0){
                Profile.CurrentPlayerIndex = 3;
                
            }
            profile.SetActive(false);
        }
    }

    public void SelectProfile()
    {
        
    }
}
