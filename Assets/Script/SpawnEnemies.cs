using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour {

    public GameObject enemy;
    public GameObject boss;
    public List<GameObject> spawnPoints;
    public GameObject spawnPointBoss;
    public float spawnTime;

	void Start () {
        InvokeRepeating("SpawnEnemy", 1, spawnTime);
        InvokeRepeating("SpawnBoss", 30, 45);
    }

    void Update () {

    }

    void SpawnEnemy()
    {
        int r = Random.Range(0, spawnPoints.Count);
        Instantiate(enemy, spawnPoints[r].transform.position, spawnPoints[r].transform.rotation);
    }

    void SpawnBoss()
    {
        Instantiate(boss, spawnPointBoss.transform.position, spawnPointBoss.transform.rotation);
    }
}
