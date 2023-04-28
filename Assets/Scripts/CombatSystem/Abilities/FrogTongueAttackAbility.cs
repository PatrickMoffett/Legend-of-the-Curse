using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;


namespace Abilities
{
    [CreateAssetMenu(menuName="CombatSystem/Abilities/FrogTongueAttackAbility")]
    public class FrogTongueAttackAbility : Ability
    {
        [SerializeField] private GameObject tongueProjectilePrefab;
        [SerializeField] private float range = 5;
        [SerializeField] private float acceleration = 1;
        [SerializeField] private float launchDelay = .5f;
        [SerializeField] private List<StatusEffect> effectsToApplyOnHit;
        [SerializeField] private StatusEffect selfImmobilize;
        [SerializeField] private SimpleAudioEvent audioEvent;
        
        private static readonly int Attacking = Animator.StringToHash("Attacking");

        private Projectile _projectile;
        private Rigidbody2D _projectileRb;
        private Vector2 _initialDirection;
        private Coroutine _updateProjectileCoroutine;
        private StatusEffectInstance _immobilizeEffect;
        private LineRenderer _lineRenderer;

        public override bool TryActivate(AbilityTargetData activationData)
        {
            if (_projectile != null) return false; // don't fire again while tongue is still out
            return base.TryActivate(activationData);
        }

        protected override void Activate(AbilityTargetData activationData)
        {
            _owner.GetComponent<Animator>().SetBool(Attacking, true);

            _initialDirection = activationData.sourceCharacterDirection;
            ServiceLocator.Instance.Get<MonoBehaviorService>().StartCoroutine(FireProjectile());
            _immobilizeEffect = _combatSystem.ApplyStatusEffect(new OutgoingStatusEffectInstance(selfImmobilize, _combatSystem));

            if (audioEvent)
            {
                audioEvent.Play(_owner);
            }
        }

        private IEnumerator FireProjectile()
        {
            yield return new WaitForSeconds(launchDelay);
            //set rotation and spawn projectile
            var rotation = Quaternion.Euler(0, 0, (Mathf.Atan2(_initialDirection.y, _initialDirection.x) * Mathf.Rad2Deg - 90));
            GameObject projectile = Instantiate(tongueProjectilePrefab,_owner.transform.position,rotation);
            
            //Instantiate status effects
            List<OutgoingStatusEffectInstance> statusEffectInstances = new List<OutgoingStatusEffectInstance>();
            foreach (var effect in effectsToApplyOnHit)
            {
                statusEffectInstances.Add(new OutgoingStatusEffectInstance(effect,_combatSystem));
            }
            
            //Add Status Effect Instances to projectile
            _projectile = projectile.GetComponent<Projectile>();
            _projectile.AddStatusEffects(statusEffectInstances);
            
            //Set projectile velocity
            float speed = Mathf.Sqrt(2 * acceleration * range);
            _projectileRb = projectile.GetComponent<Rigidbody2D>();
            _projectileRb.velocity = _initialDirection * speed;
            
            //set lifetime
            Destroy(projectile, 2*speed/acceleration);
            
            //listen for destroy
            _projectile.OnDestroyed += OnProjectileDestroyed;

            //get line renderer
            _lineRenderer = projectile.GetComponent<LineRenderer>();
            
            //start acceleration
            if (_updateProjectileCoroutine != null)
            {
                ServiceLocator.Instance.Get<MonoBehaviorService>().StopCoroutine(_updateProjectileCoroutine);
            }

            _updateProjectileCoroutine = ServiceLocator.Instance.Get<MonoBehaviorService>().StartCoroutine(UpdateProjectile());
        }

        private void OnProjectileDestroyed()
        {
            _owner.GetComponent<Animator>().SetBool(Attacking, false);
            if (_updateProjectileCoroutine != null)
            {
                ServiceLocator.Instance.Get<MonoBehaviorService>().StopCoroutine(_updateProjectileCoroutine);
            }
            _combatSystem.RemoveStatusEffect(_immobilizeEffect);
        }

        private IEnumerator UpdateProjectile()
        {
            while (_projectileRb != null)
            {
                _projectileRb.velocity -= _initialDirection * (acceleration * Time.deltaTime);
                _lineRenderer.SetPosition(0,_owner.transform.position);
                _lineRenderer.SetPosition(1,_projectile.gameObject.transform.position);
                yield return null;
            }
        }
    }
}