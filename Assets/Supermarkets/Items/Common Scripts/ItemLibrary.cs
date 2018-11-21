using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLibrary : MonoBehaviour
{
    public static ItemLibrary Instance;

    [Header("Shopping Bag Sprites")]
    [SerializeField] List<Sprite> paperBagAppearances;
    [Space(10)]
    public Sprite shoppingBagContentsLow;
    public Sprite shoppingBagContentsMed;
    public Sprite shoppingBagContentsHigh;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

   [SerializeField] GameObject shopppingBagPrefab;
    
   public GameObject CreateShoppingBag()
    {
        GameObject newBag = Instantiate(this.shopppingBagPrefab, LevelData.CurrentLevel.Floor);
        ShoppingBag bagScript = newBag.GetComponent<ShoppingBag>();
        if (bagScript != null)
        {
            bagScript.BagSpriteRenderer.sprite = this.paperBagAppearances[Random.Range(0, this.paperBagAppearances.Count - 1)];
        }
        return newBag;
    }

}
