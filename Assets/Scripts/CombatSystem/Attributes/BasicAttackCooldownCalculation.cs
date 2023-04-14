using UnityEngine;

[CreateAssetMenu(menuName="CombatSystem/CustomCalculation/BasicAttackCooldownCalculation")]
public class BasicAttackCooldownCalculation : CustomValueCalculation
{
    public override float Calculate(CombatSystem source, CombatSystem target)
    {
        return 1f/source.GetAttributeSet().attackSpeed.CurrentValue;
    }
}
