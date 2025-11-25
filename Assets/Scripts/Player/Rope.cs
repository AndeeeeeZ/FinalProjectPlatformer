using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform spearTransform;
    [SerializeField] private int segmentCount = 25;
    [SerializeField] private float waveSpeed = 2f;
    [SerializeField] private float waveAmplitude = 0.05f;

    private LineRenderer lineRenderer;
    private bool isRopeActive;
    private float waveTime; 
    private bool enableWave;

    void Awake()
    {
        SetupLineRenderer();
    }

    void Start()
    {
        isRopeActive = false;
        waveTime = 0f;
        //enableWave = false; 
    }

    void SetupLineRenderer()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();

        lineRenderer.positionCount = segmentCount;
        lineRenderer.useWorldSpace = true;

        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (isRopeActive)
        {
            waveTime += Time.deltaTime * waveSpeed;
            enableWave = true; 
            if (enableWave)
            {
                
            }
            UpdateRopeVisual();
        }
    }

    void UpdateRopeVisual()
    {
        if (playerTransform == null || spearTransform == null) return;

        Vector3 startPos = playerTransform.position;
        Vector3 endPos = spearTransform.position;

        float distance = Vector3.Distance(startPos, endPos);

        // Calculate direction perpendicular to the rope (for wave direction)
        Vector3 direction = (endPos - startPos).normalized;
        Vector3 perpendicular = new Vector3(-direction.y, direction.x, 0); // Perpendicular in 2D

        for (int i = 0; i < segmentCount; i++)
        {
            float t = i / (float)(segmentCount - 1);

            // Base position along the line
            Vector3 position = Vector3.Lerp(startPos, endPos, t);

            // Add wave animation
            if (enableWave)
            {
                float wave = Mathf.Sin(waveTime + t * Mathf.PI * 2f) * waveAmplitude;
                position += perpendicular * wave * (1f - Mathf.Abs(t - 0.5f) * 2f); // Wave stronger in middle
            }

            lineRenderer.SetPosition(i, position);
        }
    }

    public void ShowRope()
    {
        isRopeActive = true;
        lineRenderer.enabled = true;
    }

    public void HideRope()
    {
        isRopeActive = false;
        lineRenderer.enabled = false;
        DisableWave(); 
    }

    public void DisableWave()
    {
        enableWave = false; 
    }

    public void EnableWave()
    {
        enableWave = true; 
        waveTime = 0f; 
    }

}