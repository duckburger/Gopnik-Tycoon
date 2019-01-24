using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    public static Map Instance;
    [SerializeField] List<Transform> activeCivs = new List<Transform>();
    public List<Transform> ActiveCivs
    {
        get
        {
            return activeCivs;
        }
    }

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
	
	public void AddCivToActiveList(Transform newCiv)
    {
        if (!activeCivs.Contains(newCiv))
        {
            activeCivs.Add(newCiv);
        }
    }

    public void DeleteCivFromActiveList(Transform civToDelete)
    {
        if (activeCivs.Contains(civToDelete))
        {
            activeCivs.Remove(civToDelete);
        }
    }
}
