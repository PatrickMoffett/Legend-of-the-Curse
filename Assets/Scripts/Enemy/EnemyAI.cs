using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private GameObject _player;
    // Start is called before the first frame update
    private void Start()
    {
        _player = GameObject.Find("Player");
    }

    // Update is called once per frame
    private void Update()
    {
        var dir = _player.transform.position - transform.position;
        dir.Normalize();
        transform.position += dir * (speed * Time.deltaTime);
    }
}
