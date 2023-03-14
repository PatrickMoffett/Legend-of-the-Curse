using UnityEngine;
using Services;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private CharacterStats _stats;

    void Update() 
    {
        if (_stats.health <= 0f)
        {
            ServiceLocator.Instance.Get<ApplicationStateManager>().PushState<GameOverState>();
        }
    }
}
