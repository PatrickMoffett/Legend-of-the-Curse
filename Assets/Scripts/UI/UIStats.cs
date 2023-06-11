using System;
using Services;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStats : MonoBehaviour
{
    public PlayerStatistics myStats;
    public TextMeshProUGUI shotsText;
    public TextMeshProUGUI hitsText;
    public TextMeshProUGUI stepsText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI deathsText;

    public TextMeshProUGUI attackPowerText;
    public TextMeshProUGUI attackSpeedText;
    public TextMeshProUGUI moveSpeedText;
    public TextMeshProUGUI healthRegenText;
    public TextMeshProUGUI manaRegenText;

    private int prevShots = -1;
    private int prevHits = -1;
    private int prevSteps = -1;
    private int prevDeaths = -1;
    private int prevKills = -1;

    private void Start()
    {
        GameObject player = ServiceLocator.Instance.Get<PlayerManager>().GetPlayer();
        if(player != null)
            BindPlayerStats(player);
        ServiceLocator.Instance.Get<PlayerManager>().OnPlayerSpawned += BindPlayerStats;
    }

    private void BindPlayerStats(GameObject player)
    {
        AttributeSet attributeSet= player.GetComponent<AttributeSet>();
        
        //set current values
        attackPowerText.text = "Attack Power: " + attributeSet.attackPower.CurrentValue; 
        attackSpeedText.text = "Attack Speed: " + attributeSet.attackSpeed.CurrentValue; 
        moveSpeedText.text = "Move Speed: " + attributeSet.moveSpeed.CurrentValue; 
        healthRegenText.text = "Health Regen: " + attributeSet.healthRegen.CurrentValue; 
        manaRegenText.text = "Mana Regen: " + attributeSet.manaRegen.CurrentValue; 
        
        //listen for changes
        attributeSet.attackPower.OnValueChanged += SetAttackPowerText;
        attributeSet.attackSpeed.OnValueChanged += SetAttackSpeedText;
        attributeSet.moveSpeed.OnValueChanged += SetMoveSpeedText;
        attributeSet.healthRegen.OnValueChanged += SetHealthRegenText;
        attributeSet.manaRegen.OnValueChanged += SetManaRegenText;
    }
    private void SetAttackPowerText(ModifiableAttributeValue attribute, float oldValue)
    {
        attackPowerText.text = "Attack Power: "+ attribute.CurrentValue;
    }
    
    private void SetAttackSpeedText(ModifiableAttributeValue attribute, float oldValue)
    {
        attackSpeedText.text = "Attack Speed: "+ attribute.CurrentValue;
    }
    private void SetMoveSpeedText(ModifiableAttributeValue attribute, float oldValue)
    {
        moveSpeedText.text = "Move Speed: "+ attribute.CurrentValue;
    }
    private void SetHealthRegenText(ModifiableAttributeValue attribute, float oldValue)
    {
        healthRegenText.text = "Health Regen: "+ attribute.CurrentValue;
    }
    private void SetManaRegenText(ModifiableAttributeValue attribute, float oldValue)
    {
        manaRegenText.text = "Mana Regen: "+ attribute.CurrentValue;
    }
    
    void Update()
    {
        if (!myStats) return;
        if (!shotsText) return;
        if (!hitsText) return;
        if (!stepsText) return;
        if (!killsText) return;
        if (!deathsText) return;

        // only update text if value has changed (for better perf)

        if (myStats.shots != prevShots) {
            shotsText.text = "Shots: " + myStats.shots;
            prevShots = myStats.shots;
        }

        if (myStats.hits != prevHits) {
            hitsText.text = "Hits: " + myStats.hits;
            prevHits = myStats.hits;
        }

        if (myStats.steps != prevSteps) {
            stepsText.text = "Distance: " + myStats.steps;
            prevSteps = myStats.steps;
        }

        if (myStats.deaths != prevDeaths) {
            deathsText.text = "Deaths: " + myStats.deaths;
            prevDeaths = myStats.deaths;
        }

        if (myStats.kills != prevKills) {
            killsText.text = "Kills: " + myStats.kills;
            prevKills = myStats.kills;
        }


        // for debugging purposes only!
        // myStats.steps++;



    }
}
