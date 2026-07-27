using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Near-miss detection: player passes obstacle within 0.5m without hitting = +1 shard.
/// GDD: collectibles-light-shards-glow-system#detailed-rules
/// </summary>
public class NearMissDetector : MonoBehaviour
{
    private float nearMissDistance = 0.5f;
    private Transform player;
    private ObstacleSpawner obstacleSpawner;
    private List<float> countedObstacleZ = new List<float>();

    void Start()
    {
        var collData = GameData.Collectibles ?? new GameData.CollectibleData();
        nearMissDistance = collData.nearMissDistance;
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

        if (obstacleSpawner == null)
            obstacleSpawner = FindFirstObjectByType<ObstacleSpawner>();
        if (obstacleSpawner == null) return;

        // Check active obstacles
        foreach (var obs in obstacleSpawner.GetActiveObstacles())
        {
            if (obs == null) continue;
            float obsZ = obs.transform.position.z;
            // Check if already counted
            bool alreadyCounted = false;
            foreach (var cz in countedObstacleZ)
            {
                if (Mathf.Abs(cz - obsZ) < 0.1f) { alreadyCounted = true; break; }
            }
            if (alreadyCounted) continue;

            float dz = player.position.z - obsZ;
            // Player just passed obstacle (0 < dz < 2)
            if (dz > 0 && dz < 2f)
            {
                countedObstacleZ.Add(obsZ);

                // Calculate distance to obstacle edge
                float obsHeight = obs.transform.localScale.y;
                float obsCenterY = obs.transform.position.y;
                float yDist = Mathf.Abs(player.position.y - obsCenterY) - obsHeight / 2f;
                float xDist = Mathf.Abs(player.position.x - obs.transform.position.x);

                // Near-miss: close in X or Y but didn't hit
                if (xDist < nearMissDistance || yDist < nearMissDistance)
                {
                    var pc = player.GetComponent<PlayerController>();
                    if (pc != null && !pc.isInvincible)
                    {
                        GameManager.Instance.AddShard(1);
                        var holder = player.GetComponentInChildren<SFXHolder>();
                        if (holder != null) holder.Play(holder.shardClip);
                    }
                }
            }
        }

        // Clean up counted Z positions that are far behind player
        countedObstacleZ.RemoveAll(z => player.position.z - z > 60f);
    }
}