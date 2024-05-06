using UnityEngine;

public class InGameDialogue : MonoBehaviour
{
    public GameObject Prolog;
    public GameObject Dialog1;
    public GameObject Dialog2;
    public GameObject Dialog3;
    public GameObject Dialog4;
    public GameObject Dialog5;
    public GameObject Dialog6;
    public GameObject enemySpawner;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerAttack _playerAttack;
    [SerializeField] private Rigidbody _rigidbodyPlayer;
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
        else if(Prolog.activeInHierarchy && !Dialog2.activeInHierarchy && Dialog3.activeInHierarchy) {
            PrologInstruction3();
        }
        else if(Prolog.activeInHierarchy && !Dialog3.activeInHierarchy  && Dialog4.activeInHierarchy) {
            PrologInstruction4();
        }
        else if(Prolog.activeInHierarchy && !Dialog4.activeInHierarchy  && Dialog5.activeInHierarchy) {
            PrologInstruction5();
        }
        else if(Prolog.activeInHierarchy && !Dialog5.activeInHierarchy  && Dialog6.activeInHierarchy) {
            PrologInstruction6();
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

    private void PrologInstruction4()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Dialog4.SetActive(false);
            Dialog5.SetActive(true);
        }
    }

    private void PrologInstruction5()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Dialog5.SetActive(false);
            Dialog6.SetActive(true);
        }
    }

    private void PrologInstruction6()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Dialog6.SetActive(false);
            _playerAttack.enabled = true;
            _playerMovement.enabled = true;
            if (enemySpawner != null)
            {
                enemySpawner.SetActive(true);
            }
        }
    }
    
}
