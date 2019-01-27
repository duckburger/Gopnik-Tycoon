using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used by anything that wants to adjust the character's behaviour
public class ExternalPlayerController : MonoBehaviour
{

    public static ExternalPlayerController Instance;

    [SerializeField] Animator playerAnimator;
    public Animator PlayerAnimator
    {
        get
        {
            return this.playerAnimator;
        }
    }

    [SerializeField] MCharAttack playerAttackController;
    public MCharAttack PlayerAttackController
    {
        get
        {
            return this.playerAttackController;
        }
    }

    [SerializeField] MCharWalk playerWalkController;
    public MCharWalk PlayerWalkController
    {
        get
        {
            return this.playerWalkController;
        }
    }

    [SerializeField] MCharCarry playerCarryController;
    public MCharCarry PlayerCarryController
    {
        get
        {
            return this.playerCarryController;
        }
    }

    GameObject playerGO;

    public GameObject PlayerGO
    {
        get
        {
            return this.playerGO;
        }
    }

    public Transform PlayerTransform
    {
        get
        {
            return this.playerGO.transform;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }

        this.playerGO = GameObject.FindGameObjectWithTag("Player");
        if (this.playerGO != null)
        {
            this.playerAttackController = this.playerGO.GetComponent<MCharAttack>();
            this.playerWalkController = this.playerGO.GetComponent<MCharWalk>();
            this.playerCarryController = this.playerGO.GetComponent<MCharCarry>();
            this.playerAnimator = this.PlayerGO.GetComponent<Animator>();
        }
    }

    public void TurnOffAllPlayerSystems()
    {
        this.playerAttackController.enabled = false;
        this.playerWalkController.enabled = false;
        this.playerCarryController.enabled = false;
        this.playerAnimator.Play("Idle");

    }

    public void TurnOnAllPlayerSystems()
    {
        this.playerAttackController.enabled = true;
        this.playerWalkController.enabled = true;
        this.playerCarryController.enabled = true;
    }
}
