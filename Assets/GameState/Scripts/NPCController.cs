using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    List<GopnikAI> allGopniks = new List<GopnikAI>();

    public void AddNewGopnik(GopnikAI newGopnik)
    {
        if (!this.allGopniks.Contains(newGopnik))
        {
            this.allGopniks.Add(newGopnik);
        }
    }

    public List<GopnikAI> GetAllGopniks()
    {
        return allGopniks;
    }
}
