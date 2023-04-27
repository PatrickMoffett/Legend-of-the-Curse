using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private float spawnTime = 2;
    [SerializeField] private int maxToSpawn = 10;
    [SerializeField] private float rangeToSpawn = 5;
    
    private GameObject _bucket;
    private float _timeSinceLastSpawn = 0;
    private List<GameObject> _spawnedEnemyCharacters = new List<GameObject>();
    private AttributeSet _attributeSet;
    
    private void Start()
    {
        _bucket = new GameObject(this.gameObject.name + "_Bucket");
        _bucket.transform.parent = transform.parent;
        _attributeSet = GetComponent<AttributeSet>();
        _attributeSet.currentHealth.OnValueChanged += HealthChanged;
    }

    private void HealthChanged(ModifiableAttributeValue health)
    {
        if (health.CurrentValue <= 0)
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        // increase time
        _timeSinceLastSpawn += Time.deltaTime;
        //return if max amount of enemies spawned
        if (_spawnedEnemyCharacters.Count > maxToSpawn) return;
        //check if it is time to spawn an enemy
        if (_timeSinceLastSpawn > spawnTime)
        {
            //reset timer
            _timeSinceLastSpawn = 0;
            
            //get spawn position
            Vector3 spawnPosition = Random.insideUnitCircle * rangeToSpawn;
            spawnPosition += transform.position;
            
            //spawn enemy
            GameObject newEnemy =Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
            //put in bucket to keep scene hierarchy clean
            newEnemy.transform.parent = _bucket.transform;
            
            //listen to when enemy dies
            newEnemy.GetComponent<EnemyCharacter>().OnEnemyDied += OnEnemyDied;
            _spawnedEnemyCharacters.Add(newEnemy);
        }
    }

    private void OnEnemyDied(GameObject enemyThatDied)
    {
        _spawnedEnemyCharacters.Remove(enemyThatDied);
    }
}
