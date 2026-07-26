using UnityEngine;

/// <summary>
/// Camera follows player from behind+above. GDD: environment-track-visual-theme.md (parallax-like).
/// </summary>
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -8);
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null)
        {
            var player = FindFirstObjectByType<PlayerController>();
            if (player != null) target = player.transform;
            else return;
        }

        Vector3 desired = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.forward * 5);
    }
}