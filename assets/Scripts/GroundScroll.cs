using UnityEngine;

/// <summary>
/// Ground follows player Z — infinite track illusion. Large plane repositions ahead.
/// </summary>
public class GroundScroll : MonoBehaviour
{
    private Transform player;
    private float groundLength;

    void Start()
    {
        transform.localScale = new Vector3(2f, 1f, 50f);
        groundLength = GetComponent<Renderer>().bounds.size.z;
    }

    void Update()
    {
        if (player == null)
        {
            var pc = FindFirstObjectByType<PlayerController>();
            if (pc != null) player = pc.transform;
            else return;
        }

        float playerZ = player.position.z;
        float step = groundLength / 2f;
        float groundZ = Mathf.Round(playerZ / step) * step;
        transform.position = new Vector3(0, 0, groundZ);
    }
}