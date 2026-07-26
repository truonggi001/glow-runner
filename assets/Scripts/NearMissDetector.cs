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
            float dz = player.position.z - obs.transform.position.z;
            // Player just passed obstacle (0 < dz < 2)
            if (dz > 0 && dz < 2f)
            {
                // Check if already counted (tag it)
                if (obs.CompareTag("NearMissed")) continue;
                obs.tag = "NearMissed";

                float dy = Mathf.Abs(player.position.y - obs.transform.position.z);
                // Calculate actual Y distance to obstacle
                var obsHeight = obs.transform.localScale.y;
                var obsCenterY = obs.transform.position.y;
                float playerY = player.position.y;
                float yDist = Mathf.Abs(playerY - obsCenterY) - obsHeight / 2f;

                float xDist = Mathf.Abs(player.position.x - obs.transform.position.x);

                // Near-miss: close in X or Y but didn't hit
                if (xDist < nearMissDistance || yDist < nearMissDistance)
                {
                    // Check player not invincible (dash near-miss counts once)
                    var pc = player.GetComponent<PlayerController>();
                    if (pc != null && !pc.isInvincible)
                    {
                        GameManager.Instance.AddShard(1);
                        // Play shard SFX
                        var holder = player.GetComponentInChildren<SFXHolder>();
                        if (holder != null) holder.Play(holder.shardClip);
                    }
                }
            }
        }
    }
}