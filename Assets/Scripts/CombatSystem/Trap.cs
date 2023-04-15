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

    
    
    private GameObject objectOnSpike;
    
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
        if(spikeActivated && isOnSpike) 
        {        
            CombatSystem combatSystem = objectOnSpike.gameObject.GetComponent<CombatSystem>();
            if (combatSystem)
            {
                //Instantiate status effects
                List<OutgoingStatusEffectInstance> statusEffectInstances = new List<OutgoingStatusEffectInstance>();
                foreach (var effect in effectsToApplyOnHit)
                {
                    statusEffectInstances.Add(new OutgoingStatusEffectInstance(effect,combatSystem));
                }
            }
        }
    }
    
    public void StartTrap()
    {
        StopAllCoroutines();
        spikeActivated = true;
        spikeAnimator.Play(activatedAnimationName);
        StartCoroutine(ResetTrap());
    }

    private void OnTriggerEnter2D(Collider2D collider){
        isOnSpike = true;
        objectOnSpike = collider.gameObject;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        isOnSpike = false;
        objectOnSpike = null;
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
