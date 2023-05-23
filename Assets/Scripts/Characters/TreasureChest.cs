using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour, IInteractable
{
    public string Prompt { get; set; }
    public bool InteractionEnabled { get; set; }
    public void ReceiveInteraction(GameObject interactor)
    {
        Debug.Log("interaction received");
        Debug.Log("treasure chest opened");
        InteractionEnabled = false;
    }
}
