using UnityEngine;

// Smooth overhead-angled follow camera locked to the player hole.
public class GameCamera : MonoBehaviour
{
    public Transform Target { get; set; }

    // Fixed offset in world space (no player control – matches briefing requirement)
    private static readonly Vector3 OFFSET = new Vector3(0f, 52f, -28f);

    void LateUpdate()
    {
        if (Target == null) return;

        Vector3 desired = Target.position + OFFSET;
        transform.position = Vector3.Lerp(transform.position, desired, Time.deltaTime * 5f);
        transform.LookAt(Target.position + new Vector3(0f, 1f, 0f));
    }
}
