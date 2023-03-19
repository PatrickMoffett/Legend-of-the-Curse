using UnityEngine;
using UnityEngine.UI;

public class UIFill : MonoBehaviour
{
    [SerializeField] private CharacterStats _stats;
    [SerializeField] private Image _image;
    [SerializeField] private bool _lerp;
    private float lerpRate = 0.01f;
    void Update()
    {
        if (!_lerp)
        {
            _image.fillAmount = _stats.health / _stats.maxHealth;
        }
        else 
        {
            float newFillAmount = _stats.health / _stats.maxHealth;
            _image.fillAmount = Mathf.Lerp(_image.fillAmount, newFillAmount, lerpRate);
        }
    }
}
