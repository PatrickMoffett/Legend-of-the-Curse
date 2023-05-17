using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    string Prompt {get; set; }
    bool InteractionEnabled {get; set; }
    void ReceiveInteraction(Character character);
}