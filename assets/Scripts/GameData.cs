using UnityEngine;
using System.IO;

/// <summary>
/// Loads gameplay values from assets/data/*.json at runtime.
/// GDD: entities.yaml = locked facts, assets/data/*.json = runtime values.
/// </summary>
public static class GameData
{
    public static MovementData Movement;
    public static ObstacleData Obstacles;
    public static CollectibleData Collectibles;
    public static ScoringData Scoring;
    public static GameStatesData GameStates;
    public static EnvironmentData Environment;

    [System.Serializable]
    public class MovementData
    {
        public float baseSpeed = 8f;
        public float jumpForce = 9.8f;
        public float jumpMultiplierGround = 1f;
        public float jumpMultiplierDouble = 0.7f;
        public float dashDistance = 2f;
        public float dashDuration = 0.2f;
        public float dashInvincibility = 0.3f;
        public float dashCooldown = 1.5f;
        public float speedRampInterval = 30f;
        public float speedRampPercent = 10f;
        public float speedCapMultiplier = 2f;
        public float gravity = -9.81f;
        public float coyoteTime = 0.1f;
    }

    [System.Serializable]
    public class ObstacleData
    {
        public float patternSpacingBase = 10f;
        public float obstacleHeightWall = 1.5f;
        public float obstacleHeightLowbar = 1f;
        public float gapWidthMin = 2f;
        public float gapWidthMax = 4f;
        public float safeZoneCheckpoint = 3f;
    }

    [System.Serializable]
    public class CollectibleData
    {
        public int shardValue = 1;
        public int glowCap = 50;
        public float nearMissDistance = 0.5f;
        public int nearMissReward = 1;
        public float glowEmissiveMultiplier = 2f;
        public float trailParticleRateBase = 10f;
        public float trailParticleRateMax = 50f;
    }

    [System.Serializable]
    public class ScoringData
    {
        public int scorePerMeter = 1;
        public int scorePerShard = 10;
        public string highScoreKey = "glowrunner_highscore";
    }

    [System.Serializable]
    public class GameStatesData
    {
        public float transitionDeadToPlaying = 0.5f;
        public float transitionMenuToPlaying = 0.3f;
    }

    [System.Serializable]
    public class EnvironmentData
    {
        public float segmentLength = 30f;
        public float trackWidth = 4f;
        public float ambientBase = 0.2f;
        public float ambientGlowMultiplier = 0.6f;
        public float gridEmissiveBase = 0.3f;
        public float gridEmissiveGlowMultiplier = 2f;
        public float parallaxSpeedFar = 0.1f;
        public float parallaxSpeedMid = 0.3f;
        public float despawnDistance = 60f;
    }

    public static void LoadAll()
    {
        Movement = LoadJson<MovementData>("assets/data/movement.json") ?? new MovementData();
        Obstacles = LoadJson<ObstacleData>("assets/data/obstacles.json") ?? new ObstacleData();
        Collectibles = LoadJson<CollectibleData>("assets/data/collectibles.json") ?? new CollectibleData();
        Scoring = LoadJson<ScoringData>("assets/data/scoring.json") ?? new ScoringData();
        GameStates = LoadJson<GameStatesData>("assets/data/game-states.json") ?? new GameStatesData();
        Environment = LoadJson<EnvironmentData>("assets/data/environment.json") ?? new EnvironmentData();
    }

    private static T LoadJson<T>(string relativePath) where T : class
    {
        // Try streaming assets path, fallback to project assets
        string[] paths = {
            Path.Combine(Application.streamingAssetsPath, relativePath),
            Path.Combine(Application.dataPath, "data", Path.GetFileName(relativePath)),
            Path.Combine(Application.dataPath, "..", relativePath),
        };

        foreach (string path in paths)
        {
            try
            {
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    return JsonUtility.FromJson<T>(json);
                }
            }
            catch { }
        }

        Debug.LogWarning($"GameData: Could not load {relativePath}, using defaults");
        return null;
    }
}