using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Game states + scoring singleton. GDD: game-states-menu-play-death-restart.md + scoring-distance-shard-count-high-score.md
/// </summary>
public class GameManager : MonoBehaviour
{
    public enum State { Menu, Playing, Dead }

    public static GameManager Instance { get; private set; }

    [Header("State")]
    public State CurrentState = State.Menu;

    [Header("Score")]
    public int Score;
    public int HighScore;
    public int ShardCount;
    public float DistanceZ;
    public float GlowIntensity;

    private GameData.ScoringData scoringData;
    private GameData.GameStatesData stateData;
    private float deathTransitionTimer;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        GameData.LoadAll();
        scoringData = GameData.Scoring ?? new GameData.ScoringData();
        stateData = GameData.GameStates ?? new GameData.GameStatesData();
        HighScore = PlayerPrefs.GetInt(scoringData.highScoreKey, 0);
    }

    private float menuAutoStartTimer = 0;
    private bool autoStartEnabled = false; // disabled for production

    void Update()
    {
        switch (CurrentState)
        {
            case State.Menu:
                if (Input.GetKeyDown(KeyCode.Space)) StartGame();
                // Auto-start for screenshot capture (remove in production)
                if (autoStartEnabled)
                {
                    menuAutoStartTimer += Time.deltaTime;
                    if (menuAutoStartTimer > 0.5f) StartGame();
                }
                break;
            case State.Playing:
                UpdateScore();
                break;
            case State.Dead:
                deathTransitionTimer -= Time.deltaTime;
                if (deathTransitionTimer <= 0)
                {
                    if (Input.GetKeyDown(KeyCode.Space)) RestartGame();
                    if (Input.GetKeyDown(KeyCode.Escape)) GoToMenu();
                }
                break;
        }
    }

    void UpdateScore()
    {
        var player = FindFirstObjectByType<PlayerController>();
        if (player == null) return;
        DistanceZ = player.transform.position.z;
        Score = Mathf.FloorToInt(DistanceZ) + ShardCount * scoringData.scorePerShard;
    }

    public void StartGame()
    {
        CurrentState = State.Playing;
        Score = 0;
        ShardCount = 0;
        DistanceZ = 0;
        GlowIntensity = 0;

        // Start BGM
        var audioSrc = GetComponent<AudioSource>();
        if (audioSrc != null && audioSrc.clip != null) audioSrc.Play();
    }

    public void OnPlayerDeath()
    {
        if (CurrentState != State.Playing) return;
        CurrentState = State.Dead;
        deathTransitionTimer = stateData.transitionDeadToPlaying;
        if (Score > HighScore)
        {
            HighScore = Score;
            PlayerPrefs.SetInt(scoringData.highScoreKey, HighScore);
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void GoToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddShard(int count = 1)
    {
        ShardCount += count;
        var collData = GameData.Collectibles ?? new GameData.CollectibleData();
        GlowIntensity = Mathf.Min((float)ShardCount / collData.glowCap, 1f);
    }
}