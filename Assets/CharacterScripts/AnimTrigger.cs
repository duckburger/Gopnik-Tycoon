using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTrigger : MonoBehaviour
{

    Animator myAnimator;
    AI_Generic aiController;
    MCharWalk walkController;

    // Start is called before the first frame update
    void Start()
    {
        if (this.myAnimator == null)
        {
            this.myAnimator = GetComponent<Animator>();
        }
        if (this.aiController == null)
        {
            this.aiController = GetComponent<AI_Generic>();
        }
        if (this.walkController == null)
        {
            this.walkController = GetComponent<MCharWalk>();
        }
    }

    public void GetHit(Vector2 hitDirection, GameObject attacker)
    {
        this.myAnimator?.SetFloat("hitDirX", hitDirection.x);
        this.myAnimator?.SetFloat("hitDirY", hitDirection.y);
        this.walkController?.Paralyze(1f);
        this.myAnimator?.SetTrigger("gotHit");
        this.aiController?.ReactToAttack(attacker);
    }

}

