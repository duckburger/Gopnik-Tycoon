using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedSpawner : MonoBehaviour {

    [SerializeField] GameObject pedPrefab;
    [SerializeField] List<Sprite> pedSprites;
    [SerializeField] float maxSpawnDelay;
    [SerializeField] float minSpawnDelay;

    private void Start()
    {
        StartCoroutine(StartSpawningPedestrians());   
    }

    IEnumerator StartSpawningPedestrians()
    {
        float delayForSpawn = Random.Range(minSpawnDelay, maxSpawnDelay);
        yield return new WaitForSeconds(delayForSpawn);
        SpawnAPedestrian();
        StartCoroutine(StartSpawningPedestrians());
    }

    void SpawnAPedestrian()
    {
        Instantiate(pedPrefab, this.transform.position, Quaternion.identity, this.transform);
    }

}
