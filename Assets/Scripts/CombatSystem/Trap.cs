using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private List<StatusEffect> effectsToApplyOnHit;
    [SerializeField] private float trapOnTime = 0.5f;
    [SerializeField] private float trapOffTime = 1.5f; 
    [SerializeField] private Animator spikeAnimator;
    [SerializeField] private bool spikeActivated;
    [SerializeField] private bool isOnSpike;
    [SerializeField] private string activatedAnimationName;
    [SerializeField] private string idleAnimationName;
    [SerializeField] private AudioEvent SpikeActivatedSound;
    [SerializeField] private AudioEvent SpikeDeactivatedSound;

    private List<CombatSystem> _previouslyHit = new List<CombatSystem>();

    private List<GameObject> _objectsOnSpike = new List<GameObject>();
    
    void Awake()
    {
        spikeAnimator = GetComponent<Animator>();
        
    }

    void Start()
    {
        StartTrap();
    }


    void Update()
    {
        if(spikeActivated) 
        {
            foreach(GameObject objectOnSpike in _objectsOnSpike){
                CombatSystem combatSystem = objectOnSpike.GetComponent<CombatSystem>();
                if (combatSystem && !_previouslyHit.Contains(combatSystem))
                {
                    //Instantiate status effects
                    List<OutgoingStatusEffectInstance> statusEffectInstances = new List<OutgoingStatusEffectInstance>();
                    foreach (var effect in effectsToApplyOnHit)
                    {
                        combatSystem.ApplyStatusEffect(new OutgoingStatusEffectInstance(effect, combatSystem));
                        _previouslyHit.Add(combatSystem);
                    }
                }
            }
        }
    }
    
    public void StartTrap()
    {
        StopAllCoroutines();
        spikeActivated = true;
        spikeAnimator.Play(activatedAnimationName);
        _previouslyHit.Clear();
        StartCoroutine(ResetTrap());
    }

    private void OnTriggerEnter2D(Collider2D collider){
        _objectsOnSpike.Add(collider.gameObject);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        _objectsOnSpike.Remove(collider.gameObject);
    }
    
    private IEnumerator ResetTrap()
    {
        SpikeActivatedSound?.Play(transform.position);
        yield return new WaitForSeconds(trapOnTime);
        spikeActivated = false;
        spikeAnimator.Play(idleAnimationName);
        SpikeDeactivatedSound?.Play(transform.position);
        yield return new WaitForSeconds(trapOffTime);
        StartTrap();
    }
}
