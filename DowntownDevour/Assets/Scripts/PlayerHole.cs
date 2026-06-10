using UnityEngine;

// Reads player input and drives the associated HoleBase.
// PC:     mouse cursor position → raycast to ground → hole moves toward it
// Mobile: first touch position  → same raycast      → identical feel
// WASD / arrows override both for keyboard play.
public class PlayerHole : MonoBehaviour
{
    public HoleBase Hole { get; set; }

    void Update()
    {
        if (Hole == null) return;
        if (GameManager.Instance == null || GameManager.Instance.State != GameManager.GameState.Playing) return;

        // Touch input (mobile browser / phone)
        if (Input.touchCount > 0)
        {
            Hole.SetTargetPosition(ScreenToGround(Input.GetTouch(0).position));
            return;
        }

        // WASD / arrow keys
        float h  = Input.GetAxisRaw("Horizontal");
        float v  = Input.GetAxisRaw("Vertical");
        bool  kb = Mathf.Abs(h) > 0.05f || Mathf.Abs(v) > 0.05f;

        if (kb)
        {
            Vector3 dir = new Vector3(h, 0f, v).normalized;
            Hole.SetTargetPosition(Hole.transform.position + dir * 12f);
        }
        else
        {
            Hole.SetTargetPosition(ScreenToGround(Input.mousePosition));
        }
    }

    Vector3 ScreenToGround(Vector2 screenPos)
    {
        Ray   ray    = Camera.main.ScreenPointToRay(screenPos);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        if (ground.Raycast(ray, out float dist))
            return ray.GetPoint(dist);
        return Hole.transform.position;
    }
}

