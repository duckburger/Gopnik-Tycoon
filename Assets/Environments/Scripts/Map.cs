using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    public static Map Instance;

	// Use this for initialization
	void Start () {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
