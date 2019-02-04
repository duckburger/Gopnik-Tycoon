using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTrigger : MonoBehaviour
{

    Animator myAnimator;
    AI_Generic aiController;
    MCharWalk walkController;
    SpriteRenderer mySpriteRenderer;

    [SerializeField] Material defaultSpriteMat;
    [SerializeField] Material flashSpriteMat;

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
        if (this.mySpriteRenderer == null)
        {
            this.mySpriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    public void GetHit(Vector2 hitDirection, GameObject attacker)
    {
        this.myAnimator?.SetFloat("hitDirX", hitDirection.x);
        this.myAnimator?.SetFloat("hitDirY", hitDirection.y);
        //this.walkController?.Paralyze(0.2f);
        //this.myAnimator?.SetTrigger("gotHit");
        StartCoroutine(FlashOnHit());
        this.aiController?.ReactToAttack(attacker);
    }

    IEnumerator FlashOnHit()
    {
        this.mySpriteRenderer.material = this.flashSpriteMat;
        this.mySpriteRenderer.material.SetFloat("_FlashAmount", 0.6f);
        yield return new WaitForSeconds(0.3f);
        this.mySpriteRenderer.material = this.defaultSpriteMat;
    }

}

