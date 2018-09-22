using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    public virtual void Populate(ICharStats charState)
    {
        throw new System.NotImplementedException();
    }

    public virtual void Populate(float oneStat)
    {
        throw new System.NotImplementedException();
    }

    public virtual void Open() { }
    public virtual void Close() { }
    public virtual void Clear() { }
}
