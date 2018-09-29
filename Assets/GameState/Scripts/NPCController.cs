using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public static NPCController Instance;

    [SerializeField] Transform npcParent;
    public Transform NpcParent
    {
        get
        {
            return this.npcParent;
        }
    }
    List<AI_CharController> allGopniks = new List<AI_CharController>();

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
    }

    public void AddNewGopnik(AI_CharController newGopnik)
    {
        if (!this.allGopniks.Contains(newGopnik))
        {
            this.allGopniks.Add(newGopnik);
        }
    }

    public List<AI_CharController> GetAllGopniks()
    {
        return allGopniks;
    }
}
