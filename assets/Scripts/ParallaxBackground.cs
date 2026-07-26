using UnityEngine;

/// <summary>
/// Parallax background: 2 layers (far/mid) scroll slower than player.
/// GDD: environment-track-visual-theme#formulas
/// </summary>
public class ParallaxBackground : MonoBehaviour
{
    public enum Layer { Far, Mid }

    public Layer layerType = Layer.Far;
    private Transform player;
    private float parallaxSpeed;
    private float startPosZ;
    private float bgLength = 50f;

    void Start()
    {
        var envData = GameData.Environment ?? new GameData.EnvironmentData();
        parallaxSpeed = layerType == Layer.Far ? envData.parallaxSpeedFar : envData.parallaxSpeedMid;
        startPosZ = transform.position.z;

        // Create simple gradient texture for background
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            var tex = new Texture2D(1, 64);
            for (int y = 0; y < 64; y++)
            {
                float t = (float)y / 64f;
                Color c = layerType == Layer.Far
                    ? new Color(0.03f + t * 0.02f, 0.03f + t * 0.03f, 0.08f + t * 0.05f)
                    : new Color(0.05f + t * 0.03f, 0.05f + t * 0.04f, 0.1f + t * 0.06f);
                tex.SetPixel(0, y, c);
            }
            tex.Apply();
            var mat = new Material(Shader.Find("Standard"));
            mat.mainTexture = tex;
            renderer.material = mat;
        }
    }

    void Update()
    {
        if (player == null)
        {
            var pc = FindFirstObjectByType<PlayerController>();
            if (pc != null) player = pc.transform;
            else return;
        }

        // Move opposite to player at parallax speed
        float offset = player.position.z * parallaxSpeed;
        transform.position = new Vector3(0, transform.position.y, startPosZ - offset);

        // Wrap: if too far, snap back
        if (Mathf.Abs(transform.position.z - startPosZ) > bgLength)
        {
            startPosZ = transform.position.z + bgLength * Mathf.Sign(startPosZ - transform.position.z);
        }
    }
}