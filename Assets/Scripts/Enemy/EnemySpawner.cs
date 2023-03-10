using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private float spawnTime = 2;
    private float _timeSinceLastSpawn = 0;

    // Update is called once per frame
    void Update()
    {
        _timeSinceLastSpawn += Time.deltaTime;
        if (_timeSinceLastSpawn > spawnTime)
        {
            _timeSinceLastSpawn = 0;
            Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
        }
    }
}
