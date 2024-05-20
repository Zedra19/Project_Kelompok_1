using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

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

    [FormerlySerializedAs("_playerAttack")] public PlayerAttack PlayerAttack;
    public PlayerAttack_Dukun PlayerAttackDukun;
    public PlayerAttack_Petani PlayerAttackPetani;
    public PlayerAttack_Prajurit PlayerAttackPrajurit;
    [FormerlySerializedAs("_playerMovement")] public PlayerMovement PlayerMovement;
    [SerializeField] private Rigidbody _rigidbodyPlayer;
    public GameObject TutorialPanel;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        AssignPlayerComponents();
        DisablePlayerAbilities();
    }

    private void Update()
    {
        if (Prolog.activeInHierarchy && Dialog1.activeInHierarchy && !Dialog2.activeInHierarchy)
        {
            PrologInstruction1();
        }
        else if (Prolog.activeInHierarchy && !Dialog1.activeInHierarchy && Dialog2.activeInHierarchy)
        {
            PrologInstruction2();
        }
        else if (Prolog.activeInHierarchy && !Dialog2.activeInHierarchy && Dialog3.activeInHierarchy)
        {
            PrologInstruction3();
        }
        else if (Prolog.activeInHierarchy && !Dialog3.activeInHierarchy && Dialog4.activeInHierarchy)
        {
            PrologInstruction4();
        }
        else if (Prolog.activeInHierarchy && !Dialog4.activeInHierarchy && Dialog5.activeInHierarchy)
        {
            PrologInstruction5();
        }
        else if (Prolog.activeInHierarchy && !Dialog5.activeInHierarchy && Dialog6.activeInHierarchy)
        {
            PrologInstruction6();
        }
    }

    private void AssignPlayerComponents()
    {
        GameObject playerKsatria = GameObject.Find("Player-Ksatria(Clone)");
        GameObject playerDukun = GameObject.Find("Player_Dukun(Clone)");
        GameObject playerPetani = GameObject.Find("Player_Petani(Clone)");
        GameObject playerPrajurit = GameObject.Find("Player_Prajurit(Clone)");

        if (playerKsatria != null)
        {
            PlayerAttack = playerKsatria.GetComponent<PlayerAttack>();
            PlayerMovement = playerKsatria.GetComponent<PlayerMovement>();
            _rigidbodyPlayer = playerKsatria.GetComponent<Rigidbody>();
        }
        else if (playerDukun != null)
        {
            PlayerAttackDukun = playerDukun.GetComponent<PlayerAttack_Dukun>();
            PlayerMovement = playerDukun.GetComponent<PlayerMovement>();
            _rigidbodyPlayer = playerDukun.GetComponent<Rigidbody>();
        }
        else if (playerPetani != null)
        {
            PlayerAttackPetani = playerPetani.GetComponent<PlayerAttack_Petani>();
            PlayerMovement = playerPetani.GetComponent<PlayerMovement>();
            _rigidbodyPlayer = playerPetani.GetComponent<Rigidbody>();
        }
        else if (playerPrajurit != null)
        {
            PlayerAttackPrajurit = playerPrajurit.GetComponent<PlayerAttack_Prajurit>();
            PlayerMovement = playerPrajurit.GetComponent<PlayerMovement>();
            _rigidbodyPlayer = playerPrajurit.GetComponent<Rigidbody>();
        }
    }

    private void DisablePlayerAbilities()
    {
        if (PlayerAttack != null)
        {
            PlayerAttack.enabled = false;
        }
        if (PlayerAttackDukun != null)
        {
            PlayerAttackDukun.enabled = false;
        }
        if (PlayerAttackPrajurit != null)
        {
            PlayerAttackPrajurit.enabled = false;
        }
        if (PlayerAttackPetani != null)
        {
            PlayerAttackPetani.enabled = false;
        }
        if (PlayerMovement != null)
        {
            PlayerMovement.enabled = false;
        }
    }

    private void EnablePlayerAbilities()
    {
        if (PlayerAttack != null)
        {
            PlayerAttack.enabled = true;
        }
        if (PlayerAttackDukun != null)
        {
            PlayerAttackDukun.enabled = true;
        }
        if (PlayerAttackPetani != null)
        {
            PlayerAttackPetani.enabled = true;
        }
        if (PlayerAttackPrajurit != null)
        {
            PlayerAttackPrajurit.enabled = true;
        }
        if (PlayerMovement != null)
        {
            PlayerMovement.enabled = true;
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
            EnablePlayerAbilities();
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
            EnablePlayerAbilities();
            if (enemySpawner != null)
            {
                enemySpawner.SetActive(true);
            }
        }
    }
}
