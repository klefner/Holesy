using UnityEngine;

// Reads player input and drives the associated HoleBase.
public class PlayerHole : MonoBehaviour
{
    public HoleBase Hole { get; set; }

    void Update()
    {
        if (Hole == null) return;
        if (GameManager.Instance == null || GameManager.Instance.State != GameManager.GameState.Playing) return;

        // Mouse target – project cursor onto the ground plane (Y = 0)
        Vector3 mouseTarget = GetMouseGroundPoint();

        // WASD / arrow-key direction override
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        bool  kb = Mathf.Abs(h) > 0.05f || Mathf.Abs(v) > 0.05f;

        if (kb)
        {
            // Push target in the pressed direction; distance = 12 so MoveTowards
            // reaches full speed without overshooting.
            Vector3 dir    = new Vector3(h, 0f, v).normalized;
            Vector3 target = Hole.transform.position + dir * 12f;
            Hole.SetTargetPosition(target);
        }
        else
        {
            Hole.SetTargetPosition(mouseTarget);
        }
    }

    Vector3 GetMouseGroundPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var ground = new Plane(Vector3.up, Vector3.zero);
        if (ground.Raycast(ray, out float dist))
            return ray.GetPoint(dist);

        // Fallback: stay in place
        return Hole.transform.position;
    }
}
