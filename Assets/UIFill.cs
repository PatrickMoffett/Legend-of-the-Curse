using UnityEngine;
using UnityEngine.UI;

public class UIFill : MonoBehaviour
{
    [SerializeField] private CharacterStats _stats;
    [SerializeField] private Image _image;
    void Update()
    {
        _image.fillAmount = _stats.health / _stats.maxHealth;
    }
}
