using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]private GameObject followTarget;
    // Start is called before the first frame update
    void Start()
    {
        followTarget = ServiceLocator.Instance.Get<PlayerManager>().GetPlayer();
    }

    private void OnEnable()
    {
        ServiceLocator.Instance.Get<PlayerManager>().OnPlayerSpawned += OnPlayerSpawned;
    }

    private void OnDisable()
    {
        ServiceLocator.Instance.Get<PlayerManager>().OnPlayerSpawned -= OnPlayerSpawned;
    }

    private void OnPlayerSpawned(GameObject player)
    {
        followTarget = player;
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPosition;
        newPosition.x = followTarget.transform.position.x;
        newPosition.y = followTarget.transform.position.y;
        newPosition.z = transform.position.z;
        transform.position = newPosition;

    }
}
