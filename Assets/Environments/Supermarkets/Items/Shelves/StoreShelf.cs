using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreShelf : Building
{
    [SerializeField] protected List<Transform> shelves;

    public virtual void Restock() { }
}
