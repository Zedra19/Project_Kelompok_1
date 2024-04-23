using UnityEngine;
using UnityEngine.UI;

public class ShopPopUp : MonoBehaviour
{
    public GameObject Dialog1Text;
    public GameObject Dialog2Text;
    public GameObject DialogUpgradeText;
    public GameObject DialogRoleText;
    public Button buttonUpgrade;
    public Button buttonRole;

    private bool dialog1PopUp = false;
    private bool dialog2PopUp = false;
    private bool dialogUpgradePopUp = false;
    private bool dialogChangeRolePopUp = false;

    private ShopTrigger shopTrigger;

    private void Start()
    {
        Dialog1Text.SetActive(false);
        Dialog2Text.SetActive(false);
        
        shopTrigger = FindObjectOfType<ShopTrigger>();
        
        shopTrigger.OnDialogTriggerEnter.AddListener(StartDialog1);
        buttonUpgrade.onClick.AddListener(OnClickbuttonUpgrade);
        buttonRole.onClick.AddListener(OnClickbuttonRole);
    }

    private void StartDialog1()
    {
        Dialog1Text.SetActive(true);
        dialog1PopUp = true;
    }

    private void StartDialog2()
    {
        Dialog2Text.SetActive(true);
        dialog2PopUp = true;
    }

    private void Update()
    {
         if (dialog1PopUp && Input.GetKeyDown(KeyCode.Return))
        {
            Dialog1Text.SetActive(false);
            dialog1PopUp = false;
            StartDialog2();
        }

        if ((dialog1PopUp || dialog2PopUp || dialogUpgradePopUp || dialogChangeRolePopUp) && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseAllDialogs();
        }
    }

    private void StartdialogUpgrade()
    {
        DialogUpgradeText.SetActive(true);
        dialogUpgradePopUp = true;
    }

    private void StartdialogChangeRole()
    {
        DialogRoleText.SetActive(true);
        dialogChangeRolePopUp = true;
    }
    private void OnClickbuttonUpgrade()
    {
        Dialog2Text.SetActive(false);
        dialog2PopUp = false;
        StartdialogUpgrade();
    }

    private void OnClickbuttonRole()
    {
        Dialog2Text.SetActive(false);
        dialog2PopUp = false;
        StartdialogChangeRole();
    }

    private void CloseAllDialogs()
    {
        Dialog1Text.SetActive(false);
        dialog1PopUp = false;
        Dialog2Text.SetActive(false);
        dialog2PopUp = false;
        DialogUpgradeText.SetActive(false);
        dialogUpgradePopUp = false;
        DialogRoleText.SetActive(false);
        dialogChangeRolePopUp = false;
    }
}
