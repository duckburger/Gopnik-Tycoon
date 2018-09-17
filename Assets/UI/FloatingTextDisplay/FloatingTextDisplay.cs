using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Allows to spawn a variety of floating text notifications in the scene in the passed location
public class FloatingTextDisplay : MonoBehaviour
{
    public static FloatingTextDisplay Instance;
    [SerializeField] GameObject textObjPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    
   public void SpawnFloatingText(Vector2 screenPos, string textToInclude)
   {
        GameObject newText = Instantiate(textObjPrefab, screenPos, Quaternion.identity, this.transform);
   }
}
