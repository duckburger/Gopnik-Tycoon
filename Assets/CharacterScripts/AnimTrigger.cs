using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTrigger : MonoBehaviour
{

    Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        if (this.myAnimator == null)
        {
            this.myAnimator = GetComponent<Animator>();
        }
    }

    public void GetHit(Vector2 hitDirection)
    {
        this.myAnimator?.SetFloat("hitDirX", hitDirection.x);
        this.myAnimator?.SetFloat("hitDirY", hitDirection.y);
        this.myAnimator?.SetTrigger("gotHit");
    }

}

