using TMPro;
using UnityEngine;

public class OxygenMeterUI : MonoBehaviour
{
    [SerializeField]
    private OxygenData oxygen;

    private TextMeshProUGUI meter;

    private void Start()
    {
        meter = GetComponent<TextMeshProUGUI>();
        UpdateUI(); 
    }

    public void UpdateUI()
    {
        meter.text = oxygen.currentOxygenLevel + "/" + oxygen.maxOxygenLevel;
    }
}
