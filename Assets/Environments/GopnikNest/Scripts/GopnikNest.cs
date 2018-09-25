using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GopnikNest : MonoBehaviour, IBuildingSetup {

    [SerializeField] GameObject gopnikPrefab_LevelOne;
    [SerializeField] List<Transform> idlePoints = new List<Transform>();
    [SerializeField] SpriteRenderer mySpriteRenderer;
    [SerializeField] GopnikSpawnedEvent onGopSpawnedEvent;

    [SerializeField] List<GameObject> randomizedIdlingSpots = new List<GameObject>();

    public void InitialSetup()
    {
        BuildingPlacementPoint myPlacementPoint = this.transform.parent.GetComponent<BuildingPlacementPoint>();
        myPlacementPoint.SetOccupied(true);
        SpriteRenderer parentSpriteRend = myPlacementPoint.GetComponent<SpriteRenderer>();
        parentSpriteRend.color = Color.white;
        parentSpriteRend.enabled = false;
        mySpriteRenderer.color = Color.white;
        mySpriteRenderer.enabled = false;
        SpawnGopniks();
    }

    void SpawnGopniks()
    {
        // Spawn 2 gopniks each in his spot
        GameObject gopnikOne = Instantiate(gopnikPrefab_LevelOne, idlePoints[0].transform.position, Quaternion.identity, idlePoints[0]);
        GopnikAI firstGopAI = gopnikOne.GetComponent<GopnikAI>();
        if (onGopSpawnedEvent != null)
        {
            onGopSpawnedEvent.Raise(firstGopAI);
        }
        firstGopAI.AssingNest(this.gameObject);

        // TODO: Reactivate the second gopnik
        //GameObject gopnikTwo = Instantiate(gopnikPrefab_LevelOne, idlePoints[1].transform.position, Quaternion.identity, idlePoints[1]);
        //GopnikAI secondGopAI = gopnikTwo.GetComponent<GopnikAI>();
        //if (onGopSpawnedEvent != null)
        //{
        //    onGopSpawnedEvent.Raise(secondGopAI);
        //}
        //secondGopAI.AssingNest(this.gameObject);
    }

    public void CleanUpBeforeDestruction()
    {
        throw new System.NotImplementedException();
    }

    // Used for the gopniks to choose a random idling spot in the area of the nest
    // The Idling Goap Action will take care of disabling the used spot when the gopnik arrives there
    public GameObject SpawnAndGetRandomIdlePoint()
    {
        Transform chosenTransform = idlePoints[Random.Range(0, 1)];
        Vector2 randomizedPos = new Vector2(chosenTransform.position.x + Random.Range(-2.2f, 2.2f), chosenTransform.position.y + Random.Range(-2.2f, 2.2f));
        GameObject spotToUse = randomizedIdlingSpots[Random.Range(0, randomizedIdlingSpots.Count - 1)];
        spotToUse.transform.position = randomizedPos;
        spotToUse.SetActive(true);
        return spotToUse;
    }

   
  
}
