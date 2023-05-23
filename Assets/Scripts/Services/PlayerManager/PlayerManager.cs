
using System;
using Services;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlayerManager : IService
{
    private GameObject _playerPrefab = Resources.Load<GameObject>("Player");
    private GameObject _player;

    public event Action<GameObject> OnPlayerSpawned;
    public void SpawnPlayer(Vector3 position)
    {
        _player = Object.Instantiate(_playerPrefab,position,Quaternion.identity);
        _player.GetComponent<PlayerCharacter>().OnPlayerDied += OnPlayerDied;
        Object.DontDestroyOnLoad(_player);
        OnPlayerSpawned?.Invoke(_player);
    }

    public void SetPlayerLocation(Vector3 position)
    {
        if (_player)
        {
            _player.transform.position = position;
        }
    }
    
    private void OnPlayerDied()
    {
        _player.GetComponent<PlayerCharacter>().OnPlayerDied -= OnPlayerDied;
        Object.Destroy(_player);
        _player = null;
    }

    public GameObject GetPlayer()
    {
        return _player;
    }
}
