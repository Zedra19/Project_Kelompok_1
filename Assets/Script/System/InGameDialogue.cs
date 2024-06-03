using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class InGameDialogue : MonoBehaviour
{
    public bool isTutorial;
    public GameObject Prolog;
    public GameObject Dialog1;
    public GameObject Dialog2;
    public GameObject Dialog3;
    public GameObject Dialog4;
    public GameObject Dialog5;
    public GameObject Dialog6;
    public GameObject enemySpawner;
    public GameObject staminabar;
    public GameObject healthbar;

    [FormerlySerializedAs("_playerAttack")] public PlayerAttack PlayerAttack;
    public PlayerAttack_Dukun PlayerAttackDukun;
    public PlayerAttack_Petani PlayerAttackPetani;
    public PlayerAttack_Prajurit PlayerAttackPrajurit;
    [FormerlySerializedAs("_playerMovement")] public PlayerMovement PlayerMovement;
    [SerializeField] private Rigidbody _rigidbodyPlayer;
    public GameObject TutorialPanel;

    public Texture texture1;
    public Texture texture2;
    public Texture texture3;
    public Texture texture4;

    private List<RawImage> mcImages = new List<RawImage>();

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        AssignPlayerComponents();
        DisablePlayerAbilities();

        if (!isTutorial)
        {
            Prolog.SetActive(true);
            Dialog1.SetActive(true);
            yield break;
        }
    }

    private void FindAndAssignMCImages()
    {
        mcImages.Clear(); // Clear the list before searching again

        RawImage[] rawImages = GameObject.FindObjectsOfType<RawImage>();

        foreach (RawImage rawImage in rawImages)
        {
            if (rawImage.gameObject.name == "MC")
            {
                mcImages.Add(rawImage);
            }
        }

        if (mcImages.Any())
        {
            UpdateMCImages();
        }
        else
        {
            Debug.LogError("GameObject with name 'MC' and RawImage component not found.");
        }
    }

    private void UpdateMCImages()
    {
        foreach (RawImage mcImage in mcImages)
        {
            UpdateMCImage(mcImage);
        }
    }

    private void UpdateMCImage(RawImage mcImage)
    {
        if (StaticPlayer.CurrentPlayerIndex == 1)
        {
            mcImage.texture = texture1;
        }
        else if (StaticPlayer.CurrentPlayerIndex == 2)
        {
            mcImage.texture = texture2;
        }
        else if (StaticPlayer.CurrentPlayerIndex == 3)
        {
            mcImage.texture = texture3;
        }
        else
        {
            mcImage.texture = texture4;
        }
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
            if (isTutorial)
            {
                Prolog.SetActive(false);
                TutorialPanel.SetActive(true);
                EnablePlayerAbilities();
            }
            else
            {
                Dialog3.SetActive(true);
            }
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
            FindAndAssignMCImages();
        }
    }

    private void PrologInstruction5()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Dialog5.SetActive(false);
            Dialog6.SetActive(true);
            FindAndAssignMCImages();
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
                staminabar.SetActive(true);
                healthbar.SetActive(true);
            }
        }
    }
}
