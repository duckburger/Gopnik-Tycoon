using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used by anything that wants to adjust the character's behaviour
public class ExternalPlayerController : MonoBehaviour
{

    public static ExternalPlayerController Instance;

    [SerializeField] Animator playerAnimator;
    public Animator PlayerAnimator => this.playerAnimator;

    [SerializeField] MCharAttack playerAttackController;
    public MCharAttack PlayerAttackController => this.playerAttackController;

    [SerializeField] MCharWalk playerWalkController;
    public MCharWalk PlayerWalkController => this.playerWalkController;

    [SerializeField] MCharCarry playerCarryController;
    public MCharCarry PlayerCarryController => this.playerCarryController;

    Collider2D playerCollider;

    GameObject playerGO;

    public GameObject PlayerGO => this.playerGO;

    public Transform PlayerTransform => this.playerGO.transform;

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
            this.playerAnimator = this.playerGO.GetComponent<Animator>();
            this.playerCollider = this.playerGO.GetComponent<Collider2D>();
        }
    }

    public void TurnOffAllPlayerSystems()
    {
        this.playerAttackController.enabled = false;
        this.playerWalkController.enabled = false;
        this.playerCarryController.enabled = false;
        this.playerAnimator.SetTrigger("Idle");
    }

    public void TurnOnAllPlayerSystems()
    {
        this.playerAttackController.enabled = true;
        this.playerWalkController.enabled = true;
        this.playerCarryController.enabled = true;
        this.playerCollider.enabled = false;
        this.playerCollider.enabled = true;
    }
}
