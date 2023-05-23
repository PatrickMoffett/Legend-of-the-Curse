using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour, IInteractable
{
    public string Prompt { get; set; }
    public bool InteractionEnabled { get; set; }

    [SerializeField] string prompt;

    void Start()
    {
        Prompt = prompt;
        InteractionEnabled = true;
    }

    public void ReceiveInteraction(GameObject interactor)
    {
        InteractionEnabled = false;
    }
}
