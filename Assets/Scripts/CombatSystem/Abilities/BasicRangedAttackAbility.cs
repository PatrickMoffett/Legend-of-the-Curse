using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Abilities
{
    [CreateAssetMenu(menuName="ScriptableObject/Abilities/BasicRangedAttackAbility")]
    public class BasicRangedAttackAbility : Ability
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileVelocity = 10f;
        [SerializeField] private List<StatusEffect> effectsToApplyOnHit;

        public bool onCooldown = false;
        public override void Activate(Vector2 direction)
        {
            if(onCooldown) return;
            ServiceLocator.Instance.Get<MonoBehaviorService>().StartCoroutine(
                AttackCooldown(1/_attributes.attackSpeed.CurrentValue));
            var rotation = Quaternion.Euler(0, 0, (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90));
            GameObject projectile = Instantiate(projectilePrefab,_owner.transform.position,rotation);
            projectile.GetComponent<Projectile>().AddStatusEffects(effectsToApplyOnHit);

            projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileVelocity;
        }

        private IEnumerator AttackCooldown(float cooldownLength)
        {
            onCooldown = true;
            yield return new WaitForSeconds(cooldownLength);
            onCooldown = false;
        }
    }
}