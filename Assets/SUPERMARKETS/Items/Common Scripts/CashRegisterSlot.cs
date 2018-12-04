using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashRegisterSlot : MonoBehaviour
{
    [SerializeField] ScriptableEvent onLineAdvanced;
    [Space(10)]
    [SerializeField] int maxInQueue = 5;
    [Space(10)]
    [SerializeField] List<GameObject> peopleInQueue = new List<GameObject>();
    [SerializeField] GameObject firstQueueSlot;

    BuildingSlot myBuildingSlot;
    public int CurrentPeopleInQueue
    {
        get
        {
            return this.peopleInQueue.Count;
        }
    }

    private void Start()
    {
        this.myBuildingSlot = this.GetComponent<BuildingSlot>();
    }

    public bool AddToQueue(GameObject newPersonInQueue)
    {

        if (this.peopleInQueue.Count >= this.maxInQueue)
        {
            Debug.Log("Can't queue up as there are already too many in queue, will just wander or leave");
            return false;
        }

        if (!this.peopleInQueue.Contains(newPersonInQueue))
        {
            this.peopleInQueue.Add(newPersonInQueue);

            QueueNumber newNumber = newPersonInQueue.AddComponent<QueueNumber>();
            int numberInQueue = this.peopleInQueue.IndexOf(newPersonInQueue);
            newNumber.CurrentNumberInQueue = numberInQueue;
            newNumber.LastNumberInQueue = numberInQueue;
            return true;
        }

        return true;
    }

    public void RemoveFromQueue(GameObject personToRemove)
    {
        if (this.peopleInQueue.Contains(personToRemove))
        {
            this.peopleInQueue.Remove(personToRemove);
            Destroy(personToRemove.GetComponent<QueueNumber>());
        }
    }

    public Vector2 ProvideGeneralBuildingLocation()
    {
        return this.transform.position;
    }

    public Vector2 ProvideQueueSpot()
    {
        if (this.myBuildingSlot.CurrentBuilding == null)
        {
            Debug.Log("No cash register found in this slot");
            return Vector2.zero;
        }

        if (this.peopleInQueue.Count >= this.maxInQueue)
        {
            Debug.Log("Can't queue up as there are already too many in queue, will just wander or leave");
            return Vector2.zero;
        }

        if (this.peopleInQueue.Count <= 1)
        {
            return ProvideFirstQueuePosition();
        }
        return ProvideQueuePosition();

    }

    Vector2 ProvideFirstQueuePosition()
    {
        if (this.firstQueueSlot != null)
        {
            return (Vector2)this.firstQueueSlot.transform.position;
        }
        return new Vector2(this.transform.position.x, this.transform.position.y + 0.3f);
    }

    Vector2 ProvideQueuePosition()
    {
        // Add to queue and provide the next calculated position offset from the last provided one
        float xPos = CreateXQueuePos();
        float yPos = CreateYQueuePos();
        Debug.Log("Provided this position to the customer in line: <color=green> x: " + xPos + " y: " + yPos);
        return new Vector2(xPos, yPos);
    }

    float CreateXQueuePos()
    {
        float randomModifier = Random.Range(2f, 2.7f);
        float lastInLineX = 0;
        lastInLineX = this.peopleInQueue[peopleInQueue.Count - 2].transform.position.x;
  
        return lastInLineX - randomModifier;
    }

    float CreateYQueuePos()
    {
        float randomModifier = Random.Range(0.1f, 0.3f);
        float lastInLineY = 0;
        lastInLineY = this.peopleInQueue[peopleInQueue.Count - 2].transform.position.y;
       
        return lastInLineY/* - randomModifier*/;
    }

    public void AcceptPayment(float amount)
    {
        if (this.myBuildingSlot == null)
        {
            Debug.LogError("No building slot connected to the cash register slot");
            return;
        }

        CashRegister registerInMySlot = this.myBuildingSlot.GetComponentInChildren<CashRegister>();
        if (registerInMySlot != null)
        {
            registerInMySlot.AcceptPayment(amount);
        }
    }
}
