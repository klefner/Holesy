using UnityEngine;

// Reads player input and drives the associated HoleBase.
// PC:     mouse cursor position → raycast to ground → hole moves toward it.
// Mobile: floating joystick — touch anywhere, drag to steer.
//         The origin snaps to the first-touch point so the thumb never
//         needs to reach a fixed corner.
// WASD / arrows override both for keyboard play.
public class PlayerHole : MonoBehaviour
{
    public HoleBase Hole { get; set; }

    private Vector2 _touchOrigin;
    private bool    _touching;
    private bool    _hadTouch;

    // Drag distance (in screen pixels) that equals full speed.
    // 10% of screen height works across phone sizes.
    private float MaxDragPx => Screen.height * 0.10f;

    void Update()
    {
        if (Hole == null) return;
        if (GameManager.Instance == null || GameManager.Instance.State != GameManager.GameState.Playing) return;

        // ── Mobile: floating joystick ─────────────────────────────────────────
        if (Input.touchCount > 0)
        {
            _hadTouch = true;
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                _touchOrigin = t.position;
                _touching    = true;
            }

            if (_touching)
            {
                if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
                {
                    _touching = false;
                    Hole.SetTargetPosition(Hole.transform.position); // stop
                }
                else
                {
                    Vector2 delta = t.position - _touchOrigin;
                    float   mag   = delta.magnitude;
                    if (mag > 2f)
                    {
                        float   pct = Mathf.Clamp01(mag / MaxDragPx);
                        Vector3 dir = ScreenDeltaToWorld(delta / mag); // delta / mag = normalized
                        Hole.SetTargetPosition(Hole.transform.position + dir * 60f * pct);
                    }
                    else
                    {
                        Hole.SetTargetPosition(Hole.transform.position); // thumb resting, stop
                    }
                }
            }
            return;
        }

        // Touch lifted this frame — stop before falling through to mouse
        if (_hadTouch)
        {
            _hadTouch = false;
            _touching = false;
            Hole.SetTargetPosition(Hole.transform.position);
            return;
        }

        // ── WASD / arrow keys ─────────────────────────────────────────────────
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

    // Converts a normalised screen-space 2D direction into world XZ,
    // accounting for the camera's orientation so up-swipe = forward in-game.
    Vector3 ScreenDeltaToWorld(Vector2 dir2d)
    {
        Transform cam   = Camera.main.transform;
        Vector3   right = cam.right;   right.y = 0f; right.Normalize();
        Vector3   fwd   = cam.forward; fwd.y   = 0f; fwd.Normalize();
        return (right * dir2d.x + fwd * dir2d.y).normalized;
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
