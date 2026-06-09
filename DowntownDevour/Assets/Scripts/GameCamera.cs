using UnityEngine;

// Diablo-style isometric follow camera. Sits high and behind the target,
// looking down at roughly 57 degrees — matching the Diablo II / III camera angle.
public class GameCamera : MonoBehaviour
{
    public Transform Target { get; set; }

    // Camera rides at this offset from the target in world space.
    // (0, 45, -30) → arctan(45/30) ≈ 56°  above horizontal.
    private static readonly Vector3 OFFSET = new Vector3(0f, 45f, -30f);
    private const float SMOOTH = 6f;
    private const float FOV    = 45f;

    void Start()
    {
        var cam = GetComponent<Camera>();
        if (cam == null) return;
        cam.fieldOfView     = FOV;
        cam.farClipPlane    = 600f;
        // Deep void behind the hole — near-black with slight purple cast.
        cam.backgroundColor = new Color(0.01f, 0.005f, 0.02f);
    }

    void LateUpdate()
    {
        if (Target == null) return;
        Vector3 desired    = Target.position + OFFSET;
        transform.position = Vector3.Lerp(transform.position, desired, SMOOTH * Time.deltaTime);
        transform.LookAt(Target.position + Vector3.up * 0.5f);
    }
}
