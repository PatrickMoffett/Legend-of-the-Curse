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

        // only update text if value has changed (for better perf)

        if (myStats.shots != prevShots) {
            shotsText.text = "Shots fired: " + myStats.shots;
            prevShots = myStats.shots;
        }

        if (myStats.hits != prevHits) {
            hitsText.text = "Enemies Hit: " + myStats.hits;
            prevHits = myStats.hits;
        }

            if (myStats.steps != prevSteps) {
            stepsText.text = "Steps taken: " + myStats.steps;
            prevSteps = myStats.steps;
        }

        // for debugging purposes only!
        myStats.steps++;



    }
}
