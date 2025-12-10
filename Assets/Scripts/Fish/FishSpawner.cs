using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private Transform fishParent;
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private FishData[] fishData;
    [SerializeField] private FishBehavior[] fishBehaviors;
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private int initialAmount;
    [SerializeField] private bool autoSpawnFish;
    [SerializeField] private int maxFishAmount;
    [SerializeField] private float intervalPerFishSpawn;
    private int currentFishAmount;
    private float timer;

    private void Start()
    {
        if (fishPrefab.GetComponent<Fish>() == null)
        {
            Debug.LogWarning("Fish prefab inside fish spawner is not a fish");
        }
        timer = 0f;
        currentFishAmount = 0;
        for (int i = 0; i < initialAmount; i++)
        {
            SpawnFish();
        }
    }

    private void Update()
    {
        if (autoSpawnFish && currentFishAmount < maxFishAmount)
        {
            timer += Time.deltaTime;
            if (timer >= intervalPerFishSpawn)
            {
                timer = 0;
                SpawnFish();
            }
        }
    }

    public void SpawnFish()
    {
        currentFishAmount++; 
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        int fishDataIndex = Random.Range(0, fishData.Length);
        int fishBehaviorIndex = Random.Range(0, fishBehaviors.Length);

        GameObject fish = Instantiate(fishPrefab,
                                        spawnPoints[spawnPointIndex].position,
                                        Quaternion.identity,
                                        fishParent);
        fish.GetComponent<Fish>().SetFishDataAndBehavior(fishData[fishDataIndex], fishBehaviors[fishBehaviorIndex]);
    }

    public void OnFishKilled()
    {
        currentFishAmount--;
    }
}
