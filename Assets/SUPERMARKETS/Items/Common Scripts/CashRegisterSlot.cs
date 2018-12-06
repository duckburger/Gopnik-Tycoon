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

    List<Transform> queueSlots = new List<Transform>();

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
        GenerateAllAvailableQueueSlots();
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

    public Vector2 ProvideLastPersonInQueueLocation()
    {
        if (this.peopleInQueue.Count <= 0)
        {
            return this.transform.position;
        }
        return this.peopleInQueue[this.peopleInQueue.Count - 1].transform.position;
    }

    void GenerateAllAvailableQueueSlots()
    {
        this.queueSlots.Add(this.firstQueueSlot.transform);
        for (int i = 1; i < this.maxInQueue; i++)
        {
            Vector3 newPos = new Vector3(CreateXQueuePos(i), this.firstQueueSlot.transform.position.y, this.firstQueueSlot.transform.position.z);
            GameObject newSpot = Instantiate(new GameObject(), newPos, Quaternion.identity, this.transform);
            newSpot.name = "QueueSlot " + (i + 2).ToString();
            this.queueSlots.Add(newSpot.transform);
        }
    }

    public Vector2 ProvideQueueSpot(GameObject personAsking)
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
        return ProvideQueuePosition(personAsking);

    }

    Vector2 ProvideFirstQueuePosition()
    {
        if (this.firstQueueSlot != null)
        {
            return (Vector2)this.firstQueueSlot.transform.position;
        }
        return new Vector2(this.transform.position.x, this.transform.position.y + 0.3f);
    }

    Vector2 ProvideQueuePosition(GameObject personAsking)
    {
        // Add to queue and provide the next calculated position offset from the last provided one
        return this.queueSlots[this.peopleInQueue.IndexOf(personAsking)].transform.position;
    }

    float CreateXQueuePos(int i)
    {
        float randomModifier = Random.Range(0.1f, 0.4f);
        float newX = 0;
        newX = this.firstQueueSlot.transform.position.x - i + randomModifier;
        return newX - randomModifier;
    }

    float CreateYQueuePos()
    {
        float randomModifier = Random.Range(-0.1f, 0.2f);
        float newY = 0;
        newY = this.firstQueueSlot.transform.position.y + randomModifier;
        return newY/* - randomModifier*/;
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
