using TMPro;
using UnityEngine;

public class FishingInventoryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI display;

    [SerializeField] private FishingInventory fishingInventory;

    public void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        display.text = fishingInventory.GetAllFishString();
    }
}
