using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HUD + menu + death screen. GDD: game-states-menu-play-death-restart.md
/// </summary>
public class UIController : MonoBehaviour
{
    private GameObject hudPanel;
    private GameObject menuPanel;
    private GameObject deathPanel;
    private Text scoreText;
    private Text highScoreText;
    private Text glowBarText;
    private Text dashStatusText;
    private Text menuHighScore;
    private Text deathScore;
    private Text deathHighScore;

    void Start()
    {
        CreateUI();
    }

    void CreateUI()
    {
        // Canvas
        var canvasGo = new GameObject("GameCanvas");
        var canvas = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGo.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasGo.AddComponent<GraphicRaycaster>();

        // HUD panel
        hudPanel = CreatePanel(canvasGo.transform, "HUDPanel", new Color(0, 0, 0, 0.3f));
        var hudLayout = hudPanel.AddComponent<VerticalLayoutGroup>();
        hudLayout.padding = new RectOffset(20, 0, 20, 0);
        hudLayout.childAlignment = TextAnchor.UpperLeft;
        hudLayout.spacing = 5;

        scoreText = CreateText(hudPanel.transform, "Score: 0", 24, Color.white);
        highScoreText = CreateText(hudPanel.transform, "Best: 0", 18, Color.gray);
        glowBarText = CreateText(hudPanel.transform, "Glow: [----------]", 18, Color.cyan);
        dashStatusText = CreateText(hudPanel.transform, "Dash: READY", 18, Color.green);

        // Menu panel
        menuPanel = CreatePanel(canvasGo.transform, "MenuPanel", new Color(0, 0, 0, 0.7f));
        var menuLayout = menuPanel.AddComponent<VerticalLayoutGroup>();
        menuLayout.childAlignment = TextAnchor.MiddleCenter;
        menuLayout.spacing = 20;
        CreateText(menuPanel.transform, "GLOW RUNNER", 48, Color.cyan);
        CreateText(menuPanel.transform, "Press SPACE to Start", 24, Color.white);
        menuHighScore = CreateText(menuPanel.transform, "Best: 0", 20, Color.gray);

        // Death panel
        deathPanel = CreatePanel(canvasGo.transform, "DeathPanel", new Color(0, 0, 0, 0.7f));
        var deathLayout = deathPanel.AddComponent<VerticalLayoutGroup>();
        deathLayout.childAlignment = TextAnchor.MiddleCenter;
        deathLayout.spacing = 15;
        CreateText(deathPanel.transform, "YOU DIED", 40, Color.red);
        deathScore = CreateText(deathPanel.transform, "Score: 0", 28, Color.white);
        deathHighScore = CreateText(deathPanel.transform, "Best: 0", 22, Color.gray);
        CreateText(deathPanel.transform, "[SPACE] Try Again   [ESC] Menu", 20, Color.yellow);
    }

    GameObject CreatePanel(Transform parent, string name, Color bg)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        var img = go.AddComponent<Image>();
        img.color = bg;
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        return go;
    }

    Text CreateText(Transform parent, string content, int fontSize, Color color)
    {
        var go = new GameObject("Text_" + content.Substring(0, Mathf.Min(10, content.Length)));
        go.transform.SetParent(parent, false);
        var text = go.AddComponent<Text>();
        text.text = content;
        text.fontSize = fontSize;
        text.color = color;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.alignment = TextAnchor.MiddleCenter;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        return text;
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        switch (GameManager.Instance.CurrentState)
        {
            case GameManager.State.Menu:
                hudPanel.SetActive(false);
                deathPanel.SetActive(false);
                menuPanel.SetActive(true);
                menuHighScore.text = $"Best: {GameManager.Instance.HighScore}";
                break;
            case GameManager.State.Playing:
                hudPanel.SetActive(true);
                menuPanel.SetActive(false);
                deathPanel.SetActive(false);
                scoreText.text = $"Score: {GameManager.Instance.Score}";
                highScoreText.text = $"Best: {GameManager.Instance.HighScore}";
                int glowBars = Mathf.RoundToInt(GameManager.Instance.GlowIntensity * 10);
                glowBarText.text = $"Glow: [{"#".PadLeft(glowBars, '#').PadRight(10, '-')}]".
                    Replace("#", "#").Replace("-", "-");
                // Simple glow bar
                string bar = "";
                for (int i = 0; i < 10; i++) bar += i < glowBars ? "#" : "-";
                glowBarText.text = $"Glow: [{bar}]";

                // Dash cooldown indicator
                var player = FindFirstObjectByType<PlayerController>();
                if (player != null)
                {
                    // Read dashCooldownTimer via reflection (private field)
                    var field = typeof(PlayerController).GetField("dashCooldownTimer",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (field != null)
                    {
                        float cdTimer = (float)field.GetValue(player);
                        float maxCd = GameData.Movement != null ? GameData.Movement.dashCooldown : 1.5f;
                        if (cdTimer <= 0)
                        {
                            dashStatusText.text = "Dash: READY";
                            dashStatusText.color = Color.green;
                        }
                        else
                        {
                            float pct = 1f - (cdTimer / maxCd);
                            int dashBars = Mathf.RoundToInt(pct * 10);
                            string dashBar = "";
                            for (int i = 0; i < 10; i++) dashBar += i < dashBars ? "#" : "-";
                            dashStatusText.text = $"Dash: [{dashBar}]";
                            dashStatusText.color = new Color(1f, 0.5f, 0f); // orange = charging
                        }
                    }
                }
                break;
            case GameManager.State.Dead:
                hudPanel.SetActive(false);
                menuPanel.SetActive(false);
                deathPanel.SetActive(true);
                deathScore.text = $"Score: {GameManager.Instance.Score}";
                deathHighScore.text = $"Best: {GameManager.Instance.HighScore}";
                break;
        }
    }
}