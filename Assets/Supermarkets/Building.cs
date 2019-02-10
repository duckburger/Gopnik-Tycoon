using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public string buildingName;

    public Sprite mainUIImage;
    public Sprite regularAppearance;
    public Sprite damagedAppearance;

    public int purchasePrice;
    public int slotsRequired = 1;

    public List<GameObject> listOfNearbyChars = new List<GameObject>();

    [SerializeField] float defaultHealth;
    float thresholdHealth;
    BuildingHealth myHealthController;
    SpriteRenderer mySpriteRenderer;

    public bool registerOnStart = false;

    public virtual void Place() { }

    private void Awake()
    {
        this.mySpriteRenderer = this.GetComponent<SpriteRenderer>();
        if (this.GetComponent<BuildingHealth>() != null)
        {
            this.myHealthController = this.GetComponent<BuildingHealth>();
            this.myHealthController.onHealthUpdated = null;
            this.myHealthController.onHealthUpdated += UpdateSpriteOnHealthChange;
            if (this.defaultHealth > 0)
            {
                this.myHealthController.AssignStartingHealth(this.defaultHealth);
            }
            else
            {
                this.defaultHealth = 100f;
                this.myHealthController.AssignStartingHealth(this.defaultHealth);
            }
            this.thresholdHealth = this.defaultHealth * 0.3f;
        }
    }


    void UpdateSpriteOnHealthChange(float newAmount)
    {
        if (newAmount <= this.thresholdHealth)
        {
            this.mySpriteRenderer.sprite = this.damagedAppearance;
            return;
        }
        else
        {
            this.mySpriteRenderer.sprite = this.regularAppearance;
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MCharCarry carryController = collision.gameObject.GetComponent<MCharCarry>();
        if (carryController != null)
        {
            if (!listOfNearbyChars.Contains(collision.gameObject))
            {
                this.listOfNearbyChars.Add(collision.gameObject);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        MCharCarry carryController = collision.gameObject.GetComponent<MCharCarry>();
        if (carryController != null)
        {
            if (!listOfNearbyChars.Contains(collision.gameObject))
            {
                this.listOfNearbyChars.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        MCharCarry carryController = collision.gameObject.GetComponent<MCharCarry>();
        if (carryController != null)
        {
            if (listOfNearbyChars.Contains(collision.gameObject))
            {
                this.listOfNearbyChars.Remove(collision.gameObject);
            }
        }
    }

}
