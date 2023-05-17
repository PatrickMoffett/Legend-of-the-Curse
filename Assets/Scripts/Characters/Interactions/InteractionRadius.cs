using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteractionRadius : MonoBehaviour
{
    public Character character;
    public List<IInteractable> interactables = new List<IInteractable>();
    public float detectionDistance = 2f;


    void Start()
    {
        character = GetComponent<Character>();
    }

    void Update()
    {
        // keep a fresh list of interactables on every frame
        interactables.Clear();
        GameObject[] objects = FindObjectsOfType<GameObject>();

        // check if any interactables are within range and enabled
        foreach(GameObject otherObject in objects)
        {
            if(GameObject.ReferenceEquals(otherObject, gameObject))
            {
                continue;
            }

            IInteractable interactable = otherObject.GetComponent<IInteractable>();

            if  (
                    Vector3.Distance(transform.position, otherObject.transform.position) <= detectionDistance && 
                    interactable != null && 
                    interactable.InteractionEnabled
                )
            {
                Debug.Log("Interactable found");
                interactables.Add(interactable);
            }
        }
    }
}
