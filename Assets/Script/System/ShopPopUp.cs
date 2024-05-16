using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopPopUp : MonoBehaviour
{
    public GameObject Dialog1Text;
    public GameObject Dialog2Text;
    public GameObject DialogUpgradeText;
    public GameObject DialogRoleText;
    public Button buttonUpgrade;
    public Button buttonRole;

    public PlayerMovement PlayerMovement_I;
    public PlayerAttack PlayerAttack_I;
    private bool dialog1PopUp = false;
    private bool dialog2PopUp = false;
    private bool dialogUpgradePopUp = false;
    private bool dialogChangeRolePopUp = false;
    private bool _isShopUICurrentlyActive = false;

    private ShopTrigger shopTrigger;

    void Awake()
    {
        StartCoroutine(SetupShopTrigger());
    }

    public IEnumerator SetupShopTrigger()
    {
        Dialog1Text.SetActive(false);
        Dialog2Text.SetActive(false);

        buttonUpgrade.onClick.AddListener(OnClickbuttonUpgrade);
        buttonRole.onClick.AddListener(OnClickbuttonRole);

        yield return new WaitForSeconds(0.1f);
        shopTrigger = FindObjectOfType<ShopTrigger>();
        shopTrigger.OnDialogTriggerEnter.AddListener(StartDialog1);
        shopTrigger.OnDialogTriggerExit.AddListener(CloseAllDialogs);
    }

    private void StartDialog1()
    {
        if (_isShopUICurrentlyActive)
        {
            return;
        }
        _isShopUICurrentlyActive = true;
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
            //player entering shop
            Dialog1Text.SetActive(false);
            dialog1PopUp = false;
            StartDialog2();

            PlayerAttack_I.enabled = false;
            PlayerMovement_I.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //player exiting shop
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

    public void CloseAllDialogs()
    {
        if (dialog1PopUp || dialog2PopUp || dialogUpgradePopUp || dialogChangeRolePopUp)
        {
            Dialog1Text.SetActive(false);
            dialog1PopUp = false;
            Dialog2Text.SetActive(false);
            dialog2PopUp = false;
            DialogUpgradeText.SetActive(false);
            dialogUpgradePopUp = false;
            DialogRoleText.SetActive(false);
            dialogChangeRolePopUp = false;
            _isShopUICurrentlyActive = false;

            PlayerAttack_I.enabled = true;
            PlayerMovement_I.enabled = true;
        }
    }
}
