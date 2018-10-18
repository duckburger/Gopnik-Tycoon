using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used by anything that wants to adjust the character's behaviour
public class ExternalPlayerController : MonoBehaviour
{

    public static ExternalPlayerController Instance;

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

    GameObject playerGO;

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
        }
    }
}
