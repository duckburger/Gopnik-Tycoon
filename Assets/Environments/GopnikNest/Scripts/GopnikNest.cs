using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GopnikNest : MonoBehaviour, IBuildingSetup {

    [SerializeField] GameObject gopnikPrefab_LevelOne;
    [SerializeField] List<Transform> idlePoints = new List<Transform>();
    [SerializeField] SpriteRenderer mySpriteRenderer;


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

    public void CleanUpBeforeDestruction()
    {
        throw new System.NotImplementedException();
    }

    void SpawnGopniks()
    {
        // Spawn 2 gopniks each in his spot
        GameObject gopnikOne = Instantiate(gopnikPrefab_LevelOne, idlePoints[0].transform.position, Quaternion.identity, idlePoints[0]);
        GopnikAI firstGopAI = gopnikOne.GetComponent<GopnikAI>();
        firstGopAI.AssingNest(this.gameObject);
        GameObject gopnikTwo = Instantiate(gopnikPrefab_LevelOne, idlePoints[1].transform.position, Quaternion.identity, idlePoints[1]);
        GopnikAI secondGopAI = gopnikTwo.GetComponent<GopnikAI>();
        secondGopAI.AssingNest(this.gameObject);
    }

  
}
