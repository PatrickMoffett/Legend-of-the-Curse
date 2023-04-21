using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Abilities
{
    [CreateAssetMenu(menuName="CombatSystem/Abilities/Spreadshot")]
    public class Spreadshot : Ability
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileVelocity = 10f;
        [SerializeField] private List<StatusEffect> effectsToApplyOnHit;
        [SerializeField] private SimpleAudioEvent audioEvent;
        [SerializeField] [Range(0,180)] private float angleSpreadShot;
        [SerializeField] private float projectilesPerShot;
        [SerializeField] private float spawnDistanceFromPlayer;

        protected override void Activate(AbilityTargetData activationData)
        {
            Vector2 direction = activationData.sourceCharacterDirection;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float startAngle = targetAngle;
            float endAngle = targetAngle;
            float currentAngle = targetAngle;
            float halfAngleSpread = 0f;
            float angleStep = 0f;

            if (angleSpreadShot != 0) {
	            angleStep = angleSpreadShot / (projectilesPerShot - 1);
	            halfAngleSpread = angleSpreadShot / 2f;
	            startAngle = targetAngle - halfAngleSpread;
	            endAngle = targetAngle + halfAngleSpread;
	            currentAngle = startAngle;
            }
            
            //Instantiate status effects
            List<OutgoingStatusEffectInstance> statusEffectInstances = new List<OutgoingStatusEffectInstance>();
            foreach (var effect in effectsToApplyOnHit)
            {
                statusEffectInstances.Add(new OutgoingStatusEffectInstance(effect,_combatSystem));
            }

            for (int i = 0; i < projectilesPerShot; i++){

                Vector2 pos = FindProjectileSpawnPosition(currentAngle);
                var rotation = Quaternion.Euler(0, 0, (currentAngle-90));
                GameObject multishotProjectile = Instantiate(projectilePrefab,pos,rotation);
                //Add Status Effect Instances to projectile
                multishotProjectile.GetComponent<Projectile>().AddStatusEffects(statusEffectInstances);
                multishotProjectile.GetComponent<Rigidbody2D>().velocity = (multishotProjectile.transform.position - _owner.transform.position) * projectileVelocity;
                currentAngle += angleStep;  
            }

            if (audioEvent)
            {
                audioEvent.Play(_owner);
            }
        }

        private Vector2 FindProjectileSpawnPosition(float currentAngle)
        {
	        float x = _owner.transform.position.x + spawnDistanceFromPlayer * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
	        float y = _owner.transform.position.y + spawnDistanceFromPlayer * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
	
	        Vector2 projectileSpawnPosition = new Vector2(x,y);
	
	        return projectileSpawnPosition;
        }
    }
}