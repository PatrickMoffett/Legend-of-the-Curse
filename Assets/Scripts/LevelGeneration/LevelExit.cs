
using System;
using Services;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private void GoToNextLevel()
    {
        ServiceLocator.Instance.Get<LevelSceneManager>().LoadNextLevel();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GoToNextLevel();
        }
    }
}
