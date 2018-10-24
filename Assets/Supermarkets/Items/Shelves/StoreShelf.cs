using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StoreShelf : Building
{
    [SerializeField] protected List<Transform> shelves;

    public abstract void Restock();
    public abstract void DepleteStock();
}
