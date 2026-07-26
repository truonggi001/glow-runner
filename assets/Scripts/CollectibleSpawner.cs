using UnityEngine;

/// <summary>
/// Collectible system: light shards + glow. GDD: collectibles-light-shards-glow-system.md
/// </summary>
public class CollectibleSpawner : MonoBehaviour
{
    private GameData.CollectibleData data;

    void Start()
    {
        data = GameData.Collectibles ?? new GameData.CollectibleData();
    }

    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.CurrentState != GameManager.State.Playing)
            return;

        var player = FindFirstObjectByType<PlayerController>();
        if (player == null) return;

        // Spawn shard clusters between obstacle patterns
        // Simple approach: spawn 1 shard every ~5m at random X, height 1-2
        // Throttled by a spawn timer
    }

    public void SpawnCluster(float zCenter)
    {
        if (data == null) data = GameData.Collectibles ?? new GameData.CollectibleData();

        int count = Random.Range(3, 6);
        for (int i = 0; i < count; i++)
        {
            float x = Random.Range(-1.5f, 1.5f);
            float y = Random.Range(1f, 2.5f);
            float z = zCenter + Random.Range(-2f, 2f);
            CreateShard(new Vector3(x, y, z));
        }
    }

    void CreateShard(Vector3 pos)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.tag = "Collectible";
        go.transform.position = pos;
        go.transform.localScale = Vector3.one * 0.3f;
        go.GetComponent<Renderer>().material.color = Color.cyan;
        go.GetComponent<Collider>().isTrigger = true;

        var shard = go.AddComponent<Shard>();
    }
}

public class Shard : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance?.AddShard(1);
            Destroy(gameObject);
        }
    }
}