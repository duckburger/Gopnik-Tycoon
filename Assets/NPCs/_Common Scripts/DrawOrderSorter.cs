using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawOrderSorter : MonoBehaviour {

    [SerializeField] bool runOnce = true;
    [SerializeField] float baseSortNumber = 5000;
    [SerializeField] float refreshRateInSecs = 0.3f;
    Renderer myRenderer;

	// Use this for initialization
	void Start () {
        this.myRenderer = GetComponent<Renderer>();
        StartCoroutine(ChangeRenderOrder());
	}
	
	IEnumerator ChangeRenderOrder()
    {
        if (this.myRenderer != null)
        {
            if (!this.runOnce)
            {
                while (true)
                {
                    this.myRenderer.sortingOrder = (int)(baseSortNumber - this.transform.position.y);
                    yield return new WaitForSeconds(refreshRateInSecs);
                }
            }
            else
            {
                this.myRenderer.sortingOrder = (int)(baseSortNumber - this.transform.position.y);
                yield break;
            }
            
        }
    }
}
