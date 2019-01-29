using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceMinigame_SpawnController : MonoBehaviour {

    [Header("Spawn Pos")]
    [Range(0, 100)]
    public float SpawnXMax = 9f; //9f
    [Range(-100, 0)]
    public float SpawnXMin = -8.7f; //-8.7f

    public float SpawnZ = 8; //8f
    public float SpawnY = -49; //-49f

    [Header("Spawn Time")]
    [Tooltip("Indicates the Time a new asteroid spawns in seconds")]
    [Range(0.1f, 100)]
    public float SpawnRate;
    [Tooltip("Self handeling, do not edit!")]
    public float RestTime;
    [Header("Prefab")]
    public GameObject AsteroidPrefab;

    private void Start()
    {
        RestTime = SpawnRate;
    }

    private void Update()
    {
        if (RestTime <= 0)
        {
            SpawnWaves();

            RestTime = SpawnRate;
        }

        RestTime = RestTime - Time.deltaTime;
    }

    float RandomXPos()
    {
        return Random.Range(SpawnXMin, SpawnXMax);
    }

    void SpawnWaves()
    {
        Quaternion AsteroidRotation = Quaternion.identity;
        Vector3 AstedoidSpawnPos = new Vector3(RandomXPos(), SpawnY, SpawnZ);

        Instantiate(AsteroidPrefab, AstedoidSpawnPos, AsteroidRotation);
    }
}
