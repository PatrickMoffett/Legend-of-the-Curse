using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemySpawner : MonoBehaviour
{
    private GameObject _bucket;
    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private float spawnTime = 2;
    private float _timeSinceLastSpawn = 0;

    private void Start()
    {
        _bucket = new GameObject(this.gameObject.name + "_Bucket");
        _bucket.transform.parent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        _timeSinceLastSpawn += Time.deltaTime;
        if (_timeSinceLastSpawn > spawnTime)
        {
            _timeSinceLastSpawn = 0;
            GameObject newEnemy =Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
            newEnemy.transform.parent = _bucket.transform;
        }
    }
}
