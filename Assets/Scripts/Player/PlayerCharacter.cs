using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCharacter : MonoBehaviour
{
    public Ability basicAttack;

    public void Start()
    {
        basicAttack.Initialize(gameObject);
    }

    public void PerformBasicAttack()
    {
        basicAttack.Activate();
    }
}
