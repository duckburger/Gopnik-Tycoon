﻿using System.Collections;
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
        if (!this.peopleInQueue.Contains(newPersonInQueue))
        {
            this.peopleInQueue.Add(newPersonInQueue);
            return true;
        }

        return false;
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
            Debug.Log("Can't queue up as there are already too many in queue, will just wonder or leave");
            return Vector2.zero;
        }

        if (this.peopleInQueue.Count <= 0)
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
        return new Vector2(this.transform.position.x, this.transform.position.y + 0.7f);
    }

    Vector2 ProvideQueuePosition()
    {
        // Add to queue and provide the next calculated position offset from the last provided one
        float xPos = CreateXQueuePos();
        float yPos = CreateYQueuePos();
        return new Vector2(xPos, yPos);
    }

    float CreateXQueuePos()
    {
        float randomModifier = Random.Range(0.8f, 1.2f);
        float lastInLineX = this.peopleInQueue[peopleInQueue.Count - 1].transform.position.x;
        return lastInLineX - randomModifier;
    }

    float CreateYQueuePos()
    {
        float randomModifier = Random.Range(0.1f, 0.3f);
        float lastInLineY = this.peopleInQueue[peopleInQueue.Count - 1].transform.position.y;
        return lastInLineY - randomModifier;
    }
}
