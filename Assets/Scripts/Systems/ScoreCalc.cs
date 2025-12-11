using TMPro;
using UnityEngine;
using System.Text;

public class ScoreCalc : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultsDisplay;
    [SerializeField] private FishingInventory fishingInventory;
    [SerializeField] private bool showTotalAtBottom;

    private void Start()
    {
        DisplayResults();
    }

    public void DisplayResults()
    {
        if (fishingInventory == null || resultsDisplay == null)
        {
            Debug.LogWarning("FishingResultsUI is missing references!");
            return;
        }

        StringBuilder sb = new StringBuilder();

        float grandTotal = 0f;

        // Display each fish type
        foreach (var entry in fishingInventory.allFish)
        {
            FishData fish = entry.Key;
            int amount = entry.Value;
            float individualValue = fish.value;
            float totalValue = amount * individualValue;

            grandTotal += totalValue;

            // Format: "Fish Name | 5 x $10.00 | $50.00"
            string line = string.Format("{0,-18} | {1,3} x ${2,6:F2} | ${3,7:F2}",
                fish.fishName,
                amount,
                individualValue,
                totalValue);

            sb.AppendLine(line);
        }

        if (showTotalAtBottom)
        {
            sb.AppendLine("---------------------------------------------");
            sb.AppendLine(string.Format("GRAND TOTAL: ${0:F2}", grandTotal));
        }

        resultsDisplay.text = sb.ToString();
    }

    public void UpdateDisplay()
    {
        DisplayResults();
    }

}
