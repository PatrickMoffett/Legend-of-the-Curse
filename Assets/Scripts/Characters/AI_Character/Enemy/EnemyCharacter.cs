using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{

    [SerializeField] private float aggroRange = 8f;
    [SerializeField] private float distanceToPerformAttack;
    [SerializeField] private Ability basicAttack;

    private CombatSystem _combatSystem;
    private GameObject _player;
    private CharacterMovement _characterMovement;

    private AttributeSet _attributeSet;

    // Start is called before the first frame update
    private void Start()
    {
        _combatSystem=GetComponent<CombatSystem>();
        _characterMovement = GetComponent<CharacterMovement>();
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
        float sqrDistance = dir.sqrMagnitude;
        //if outside of aggro range do nothing
        if (sqrDistance > aggroRange * aggroRange) return;
        dir.Normalize();
        if (sqrDistance > distanceToPerformAttack * distanceToPerformAttack)
        {
            _characterMovement.Move(dir);
        }
        else
        {
            AbilityTargetData targetData = new AbilityTargetData();
            targetData.sourceCharacterDirection = dir;
            targetData.sourceCharacterLocation = transform.position;
            targetData.targetLocation = _player.transform.position;
            targetData.targetGameObject = _player;
            basicAttack.TryActivate(targetData);
        }
    }
}
