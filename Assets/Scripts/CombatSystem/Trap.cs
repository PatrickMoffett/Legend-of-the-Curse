using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private List<StatusEffect> effectsToApplyOnHit;
    
    [Space(10)]
    [Header("Activation Time")]
    [SerializeField] private float trapOnTime = 0.5f;
    [SerializeField] private float trapOffTime = 1.5f;
    [SerializeField] private float timeOffset = 0f;
    
    [Space(10)]
    [Header("Animator")]
    [SerializeField] private Animator spikeAnimator;
    [SerializeField] private string activatedAnimationName;
    [SerializeField] private string idleAnimationName;
    
    [Space(10)]
    [Header("Audio")]
    [SerializeField] private AudioEvent SpikeActivatedSound;
    [SerializeField] private AudioEvent SpikeDeactivatedSound;

    private bool _currentlyEnabled = true;
    private List<CombatSystem> _previouslyHit = new List<CombatSystem>();
    private List<GameObject> _objectsOnSpike = new List<GameObject>();
    private bool _spikeActivated;
    
    void Awake()
    {
        spikeAnimator = GetComponent<Animator>();
        
    }

    void Start()
    {
        StartCoroutine(WaitForOffset(timeOffset));
    }


    void Update()
    {
        if(_spikeActivated) 
        {
            foreach(GameObject objectOnSpike in _objectsOnSpike){
                if (objectOnSpike == null)
                {
                    _objectsOnSpike.Remove(objectOnSpike);
                }
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
        if (!_currentlyEnabled) return;
        _spikeActivated = true;
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

    private IEnumerator WaitForOffset(float offset)
    {
        yield return new WaitForSeconds(offset);
        StartTrap();
    }
    private IEnumerator ResetTrap()
    {
        SpikeActivatedSound?.Play(gameObject);
        yield return new WaitForSeconds(trapOnTime);
        _spikeActivated = false;
        spikeAnimator.Play(idleAnimationName);
        SpikeDeactivatedSound?.Play(gameObject);
        yield return new WaitForSeconds(trapOffTime);
        StartTrap();
    }
}
