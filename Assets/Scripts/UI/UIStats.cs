using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStats : MonoBehaviour
{
    public CharacterStats myStats;
    public TextMeshProUGUI shotsText;
    public TextMeshProUGUI hitsText;
    public TextMeshProUGUI stepsText;

    private int prevShots = 0;
    private int prevHits = 0;
    private int prevSteps = 0;

    void Update()
    {
        if (!myStats) return;
        if (!shotsText) return;
        if (!hitsText) return;
        if (!stepsText) return;
        // TODO:
        // if value has changed, update a text
    }
}
