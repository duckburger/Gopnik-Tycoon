using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivStates : MonoBehaviour {

    [SerializeField] bool isAgressive;

	public bool IsAgressive
    {
        get
        {
            return isAgressive;
        }
    }
}
