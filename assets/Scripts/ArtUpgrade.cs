#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

/// <summary>
/// One-time art upgrade: neon materials for all game objects.
/// Menu: Tools > Art Upgrade
/// </summary>
public class ArtUpgrade
{
    [MenuItem("Tools/Art Upgrade")]
    public static void Run()
    {
        // Player
        var player = GameObject.Find("Player");
        var playerRenderer = player.GetComponent<Renderer>();
        var playerMat = new Material(Shader.Find("Standard"));
        playerMat.color = new Color(0.05f, 0.95f, 0.45f, 1f);
        playerMat.EnableKeyword("_EMISSION");
        playerMat.SetColor("_EmissionColor", new Color(0.1f, 0.8f, 0.2f) * 2f);
        playerMat.SetFloat("_Metallic", 0.3f);
        playerMat.SetFloat("_Glossiness", 0.8f);
        playerRenderer.material = playerMat;
        player.transform.localScale = new Vector3(0.7f, 1.2f, 0.7f);
        player.transform.position = new Vector3(0, 1.2f, 0);

        // Ground
        var ground = GameObject.Find("Ground");
        var groundRenderer = ground.GetComponent<Renderer>();
        var groundMat = new Material(Shader.Find("Standard"));
        groundMat.color = new Color(0.06f, 0.06f, 0.12f, 1f);
        groundMat.EnableKeyword("_EMISSION");
        groundMat.SetColor("_EmissionColor", new Color(0.02f, 0.05f, 0.1f));
        groundMat.SetFloat("_Metallic", 0.5f);
        groundMat.SetFloat("_Glossiness", 0.9f);
        groundRenderer.material = groundMat;

        // Neon materials for obstacles
        CreateNeonMat("Assets/Materials/NeonRed.mat", Color.red);
        CreateNeonMat("Assets/Materials/NeonMagenta.mat", Color.magenta);
        CreateNeonMat("Assets/Materials/NeonOrange.mat", new Color(1f, 0.5f, 0f));
        CreateNeonMat("Assets/Materials/NeonCyan.mat", Color.cyan);

        AssetDatabase.CreateAsset(playerMat, "Assets/Materials/PlayerGlow.mat");
        AssetDatabase.CreateAsset(groundMat, "Assets/Materials/GroundNeon.mat");
        AssetDatabase.SaveAssets();

        Debug.Log("Art upgrade done: player + ground + 4 neon mats");
    }

    static void CreateNeonMat(string path, Color c)
    {
        var mat = new Material(Shader.Find("Standard"));
        mat.color = c;
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", c * 1.5f);
        mat.SetFloat("_Metallic", 0.4f);
        mat.SetFloat("_Glossiness", 0.7f);
        AssetDatabase.CreateAsset(mat, path);
    }
}
#endif