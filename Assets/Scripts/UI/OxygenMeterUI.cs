using TMPro;
using UnityEngine;

public class OxygenMeterUI : MonoBehaviour
{
    [SerializeField] private OxygenData oxygen;

    [SerializeField] private TextMeshProUGUI meter;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        meter.text = "O2 level: " + oxygen.currentOxygenLevel + "/" + oxygen.maxOxygenLevel;
    }
}
