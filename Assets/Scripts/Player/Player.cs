using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private OxygenData oxygenTank;

    [SerializeField] private FloatVariable oxygenLostPerSecond;

    private const float second = 1f;
    private float timer;

    private void Start()
    {
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > second)
        {
            timer = 0f;
            oxygenTank.ChangeOxygenLevelBy(oxygenLostPerSecond.Value);
        }
    }

}
