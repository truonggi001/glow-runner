using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Obstacle system: spawn hazard patterns. GDD: obstacles-hazard-spawning-patterns.md
/// </summary>
public class ObstacleSpawner : MonoBehaviour
{
    private GameData.ObstacleData data;
    private float nextSpawnZ;
    private float trackWidth = 4f;
    private List<GameObject> activeObstacles = new List<GameObject>();
    private Transform player;

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

        if (player == null)
        {
            var pc = FindFirstObjectByType<PlayerController>();
            if (pc != null) player = pc.transform;
            else return;
        }

        // Spawn ahead of player
        while (nextSpawnZ < player.position.z + 80f)
        {
            SpawnPattern(nextSpawnZ);
            float spacing = data.patternSpacingBase / player.GetComponent<PlayerController>().speedMultiplier;
            nextSpawnZ += spacing;
        }

        // Despawn obstacles behind player (memory management)
        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            if (activeObstacles[i] == null)
            {
                activeObstacles.RemoveAt(i);
                continue;
            }
            if (player.position.z - activeObstacles[i].transform.position.z > 60f)
            {
                Destroy(activeObstacles[i]);
                activeObstacles.RemoveAt(i);
            }
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

        // Spawn shard cluster between patterns
        var collectibleSpawner = FindFirstObjectByType<CollectibleSpawner>();
        if (collectibleSpawner != null)
        {
            float clusterZ = z + 5f; // midway to next pattern
            collectibleSpawner.SpawnCluster(clusterZ);
        }
    }

    public List<GameObject> GetActiveObstacles() { return activeObstacles; }

    void SpawnWall(float z)
    {
        var go = CreateFromModel("Assets/Models/obstaclewall.obj", z);
        if (go == null)
        {
            go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.position = new Vector3(0, data.obstacleHeightWall / 2, z);
            go.transform.localScale = new Vector3(trackWidth, data.obstacleHeightWall, 0.5f);
        }
        else
        {
            go.transform.position = new Vector3(0, data.obstacleHeightWall / 2, z);
            go.transform.localScale = new Vector3(trackWidth, data.obstacleHeightWall, 0.5f);
        }
        go.tag = "Obstacle";
        AssignNeonMaterial(go, "Assets/Materials/NeonRed.mat");
        activeObstacles.Add(go);
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
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.tag = "Obstacle";
        go.transform.position = new Vector3(0, 1.5f, z);
        go.transform.localScale = new Vector3(trackWidth, 0.5f, 0.5f);
        AssignNeonMaterial(go, "Assets/Materials/NeonOrange.mat");
        activeObstacles.Add(go);
    }

    void SpawnSpike(float z)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.tag = "Obstacle";
        go.transform.position = new Vector3(0, 0.25f, z);
        go.transform.localScale = new Vector3(trackWidth, 0.5f, 0.5f);
        AssignNeonMaterial(go, "Assets/Materials/NeonMagenta.mat");
        activeObstacles.Add(go);
    }

    void AssignNeonMaterial(GameObject go, string materialPath)
    {
        var matName = System.IO.Path.GetFileNameWithoutExtension(materialPath);
        var mat = Resources.Load<Material>($"Materials/{matName}");
        if (mat != null)
        {
            go.GetComponent<Renderer>().material = mat;
        }
    }

    GameObject CreateFromModel(string modelPath, float z)
    {
        var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(modelPath);
        if (prefab == null) return null;
        var go = Object.Instantiate(prefab);
        return go;
    }
}