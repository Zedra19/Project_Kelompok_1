using UnityEngine;

public class InGameDialogue : MonoBehaviour
{
    public GameObject Prolog;
    public GameObject Dialog1;
    public GameObject Dialog2;
    public GameObject Dialog3;
    public GameObject Dialog4;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerAttack _playerAttack;
    public GameObject TutorialPanel;

    private void Start() {
        _playerAttack.enabled = false;
        _playerMovement.enabled = false;
    }

    private void Update()
    {
        if(Prolog.activeInHierarchy && Dialog1.activeInHierarchy && !Dialog2.activeInHierarchy) {
            PrologInstruction1();
        }
        else if(Prolog.activeInHierarchy && !Dialog1.activeInHierarchy && Dialog2.activeInHierarchy) {
            PrologInstruction2();
        }
        else if(Prolog.activeInHierarchy && !Dialog1.activeInHierarchy && !Dialog2.activeInHierarchy && Dialog3.activeInHierarchy) {
            PrologInstruction3();
        }
    }

    private void PrologInstruction1()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Dialog1.SetActive(false);
            Dialog2.SetActive(true);
        }
    }

    private void PrologInstruction2()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Dialog2.SetActive(false);
            Prolog.SetActive(false);
            _playerAttack.enabled = true;
            _playerMovement.enabled = true;
            TutorialPanel.SetActive(true);
        }
    }

    private void PrologInstruction3()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Dialog3.SetActive(false);
            Dialog4.SetActive(true);
        }
    }
    
}
