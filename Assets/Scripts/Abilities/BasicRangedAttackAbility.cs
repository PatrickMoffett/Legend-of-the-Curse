using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Object = UnityEngine.Object;

namespace Abilities
{
    [CreateAssetMenu(menuName="ScriptableObject/Abilities/BasicRangedAttackAbility")]
    public class BasicRangedAttackAbility : Ability
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileVelocity = 10f;

        public override void Activate()
        {
            GameObject projectile = Object.Instantiate(projectilePrefab,_owner.transform.position,_owner.transform.rotation);
            projectile.GetComponent<Rigidbody2D>().velocity = _owner.transform.up * projectileVelocity;
        }
    }
}