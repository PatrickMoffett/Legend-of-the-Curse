using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Abilities
{
    [CreateAssetMenu(menuName="ScriptableObject/Abilities/RangedAttackAbility")]
    public class RangedAttackAbility : Ability
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileVelocity = 10f;
        [SerializeField] private List<StatusEffect> effectsToApplyOnHit;

        public bool onCooldown = false;
        public override void Activate(Vector2 direction)
        {
            //return if on cooldown
            if(onCooldown) return;
            
            //start cooldown coroutine
            ServiceLocator.Instance.Get<MonoBehaviorService>().StartCoroutine(
                AttackCooldown(1/_attributes.attackSpeed.CurrentValue));
            
            //set rotation and spawn projectile
            var rotation = Quaternion.Euler(0, 0, (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90));
            GameObject projectile = Instantiate(projectilePrefab,_owner.transform.position,rotation);
            
            //Instantiate status effects
            List<StatusEffectInstance> statusEffectInstances = new List<StatusEffectInstance>();
            foreach (var effect in effectsToApplyOnHit)
            {
                statusEffectInstances.Add(new StatusEffectInstance(effect,_combatSystem));
            }
            
            //Add Status Effect Instances to projectile
            projectile.GetComponent<Projectile>().AddStatusEffects(statusEffectInstances);
            
            //Set projectile velocity
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