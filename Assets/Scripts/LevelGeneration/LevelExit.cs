
using System;
using Services;
using UnityEngine;

public class LevelExit : MonoBehaviour , IInteractable
{
    public string Prompt { get; set; }
    public bool InteractionEnabled { get; set; }

    private void Start()
    {
        Prompt = "Go to next Level";
        InteractionEnabled = true;
    }

    private void GoToNextLevel()
    {
        ServiceLocator.Instance.Get<LevelSceneManager>().LoadNextLevel();
    }
    
    public void ReceiveInteraction(GameObject interactor)
    {
        if (interactor.CompareTag("Player"))
        {
            InteractionEnabled = false;
            GoToNextLevel();
        }
    }
}
