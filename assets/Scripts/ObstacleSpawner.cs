using UnityEngine;

/// <summary>
/// Obstacle system: spawn hazard patterns. GDD: obstacles-hazard-spawning-patterns.md
/// </summary>
public class ObstacleSpawner : MonoBehaviour
{
    private GameData.ObstacleData data;
    private float nextSpawnZ;
    private float trackWidth = 4f;

    void Start()
    {
        data = GameData.Obstacles ?? new GameData.ObstacleData();
        var envData = GameData.Environment ?? new GameData.EnvironmentData();
        trackWidth = envData.trackWidth;
        nextSpawnZ = 30f; // first pattern after safe zone
    }

    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.CurrentState != GameManager.State.Playing)
            return;

        var player = FindFirstObjectByType<PlayerController>();
        if (player == null) return;

        // Spawn ahead of player
        while (nextSpawnZ < player.transform.position.z + 80f)
        {
            SpawnPattern(nextSpawnZ);
            float spacing = data.patternSpacingBase / player.speedMultiplier;
            nextSpawnZ += spacing;
        }
    }

    void SpawnPattern(float z)
    {
        int type = Random.Range(0, 4);
        switch (type)
        {
            case 0: SpawnWall(z); break;
            case 1: SpawnGap(z); break;
            case 2: SpawnLowBar(z); break;
            case 3: SpawnSpike(z); break;
        }
    }

    void SpawnWall(float z)
    {
        // Wall on ground — jump over
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.tag = "Obstacle";
        go.transform.position = new Vector3(0, data.obstacleHeightWall / 2, z);
        go.transform.localScale = new Vector3(trackWidth, data.obstacleHeightWall, 0.5f);
        go.GetComponent<Renderer>().material.color = Color.red;
    }

    void SpawnGap(float z)
    {
        // Visual gap markers — kill if player falls in (handled by fall detection)
        var left = GameObject.CreatePrimitive(PrimitiveType.Cube);
        left.tag = "Untagged";
        left.transform.position = new Vector3(-trackWidth / 2 - 0.5f, 0, z);
        left.transform.localScale = new Vector3(1, 0.2f, 1);
        left.GetComponent<Renderer>().material.color = new Color(0.3f, 0.3f, 0.5f);

        var right = GameObject.CreatePrimitive(PrimitiveType.Cube);
        right.tag = "Untagged";
        right.transform.position = new Vector3(trackWidth / 2 + 0.5f, 0, z);
        right.transform.localScale = new Vector3(1, 0.2f, 1);
        right.GetComponent<Renderer>().material.color = new Color(0.3f, 0.3f, 0.5f);
    }

    void SpawnLowBar(float z)
    {
        // Low bar — dash under
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.tag = "Obstacle";
        go.transform.position = new Vector3(0, 1.5f, z);
        go.transform.localScale = new Vector3(trackWidth, 0.5f, 0.5f);
        go.GetComponent<Renderer>().material.color = new Color(1f, 0.5f, 0f);
    }

    void SpawnSpike(float z)
    {
        // Spike — jump or dash
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.tag = "Obstacle";
        go.transform.position = new Vector3(0, 0.25f, z);
        go.transform.localScale = new Vector3(trackWidth, 0.5f, 0.5f);
        go.GetComponent<Renderer>().material.color = Color.magenta;
    }
}