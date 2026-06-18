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
        // Daytime sky blue — shows through the stencil hole and at the world edge.
        cam.backgroundColor = new Color(0.52f, 0.68f, 0.90f);
    }

    void LateUpdate()
    {
        if (Target == null) return;
        Vector3 desired    = Target.position + OFFSET;
        transform.position = Vector3.Lerp(transform.position, desired, SMOOTH * Time.deltaTime);
        transform.LookAt(Target.position + Vector3.up * 0.5f);
    }

    // Immediately frame the target with no smoothing — used to set up the
    // static title-screen view before gameplay (and time) starts.
    public void SnapToTarget()
    {
        if (Target == null) return;
        transform.position = Target.position + OFFSET;
        transform.LookAt(Target.position + Vector3.up * 0.5f);
    }
}
