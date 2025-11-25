using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private OxygenData oxygenTank;

    private void Start()
    {
        ResetOxygenLevel();
    }

    private void ResetOxygenLevel()
    {
        oxygenTank.ResetOxygenLevelToMax();
    }
}
