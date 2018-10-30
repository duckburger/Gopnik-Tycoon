using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shelf : MonoBehaviour
{
    public List<SpriteRenderer> myShelfItems = new List<SpriteRenderer>();
    public bool isOccupied = false;

    private void Start()
    {
        foreach (Transform obj in this.transform)
        {
            SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                this.myShelfItems.Add(renderer);
            }
        }
    }

    public void Occupy(Sprite spriteToApply)
    {
        foreach (SpriteRenderer renderer in this.myShelfItems)
        {
            renderer.enabled = true;
            renderer.sprite = spriteToApply;
        }
        this.isOccupied = true;
    }

    public void Clear()
    {
        foreach (SpriteRenderer renderer in this.myShelfItems)
        {
            renderer.sprite = null;
            renderer.enabled = false;
        }
        this.isOccupied = false;
    }
}
