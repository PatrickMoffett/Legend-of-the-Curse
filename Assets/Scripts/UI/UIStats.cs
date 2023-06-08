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


    private int prevShots = -1;
    private int prevHits = -1;
    private int prevSteps = -1;
    private int prevDeaths = -1;
    private int prevKills = -1;

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
