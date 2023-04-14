using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float distanceToPerformAttack;
    [SerializeField] private Ability basicAttack;


    private CombatSystem _combatSystem;
    private GameObject _player;

    private AttributeSet _attributeSet;

    // Start is called before the first frame update
    private void Start()
    {
        _combatSystem=GetComponent<CombatSystem>();
        _attributeSet = GetComponent<AttributeSet>();
        _attributeSet.currentHealth.OnValueChanged += HealthChanged;
        _player = GameObject.Find("Player");
        basicAttack = Instantiate(basicAttack);
        basicAttack.Initialize(gameObject);
    }

    private void HealthChanged(ModifiableAttributeValue health)
    {
        if (health.CurrentValue <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        var dir = _player.transform.position - transform.position;
        if (dir.sqrMagnitude > distanceToPerformAttack * distanceToPerformAttack)
        {
            dir.Normalize();
            transform.position += dir * (speed * Time.deltaTime);
        }
        else
        {
            dir.Normalize();
            basicAttack.TryActivate(dir);
        }
    }
}
