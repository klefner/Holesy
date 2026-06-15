using System.Collections.Generic;
using UnityEngine;

// Drives a car along its assigned lane in the city road grid during Evening/Night.
// Cars move at constant speed, wrap at world edges, and crash when too close to
// another car that's heading toward them or directly ahead.
public class CarDriver : MonoBehaviour
{
    public Vector3          DriveDir;    // normalised world-space direction
    public float            Speed = 9f;
    public ConsumableObject Consumable;  // detected when the hole eats us

    static readonly List<CarDriver> _all = new List<CarDriver>();

    bool      _crashed;
    Rigidbody _rb;

    const float CHECK_DIST = 6f;
    const float CRASH_ARC  = 38f;

    void OnEnable()  { _all.Add(this);    }
    void OnDisable() { _all.Remove(this); }
    void OnDestroy() { _all.Remove(this); }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb != null) _rb.isKinematic = true;
    }

    void Update()
    {
        if (_crashed || (Consumable != null && Consumable.IsConsumed)) return;

        var gm = GameManager.Instance;
        if (gm == null || gm.State != GameManager.GameState.Playing) return;

        bool nightTime = gm.CurrentTimeOfDay == GameManager.TimeOfDay.Evening ||
                         gm.CurrentTimeOfDay == GameManager.TimeOfDay.Night;
        if (!nightTime) return;

        // Look ahead for other active cars
        Vector3 myPos = transform.position;
        foreach (var other in _all)
        {
            if (other == this || other._crashed) continue;
            if (other.Consumable != null && other.Consumable.IsConsumed) continue;

            Vector3 delta = other.transform.position - myPos;
            float   dist  = delta.magnitude;
            if (dist < 0.01f || dist > CHECK_DIST * 1.6f) continue;

            float dot = Vector3.Dot(DriveDir, delta.normalized);
            if (dot > 0.58f && dist < CHECK_DIST)
            {
                Crash();
                other.Crash();
                return;
            }
        }

        // Advance
        transform.position += DriveDir * Speed * Time.deltaTime;

        // Wrap at world boundary so cars continuously circulate
        Vector3 p    = transform.position;
        float   half = GameManager.HALF - 5f;
        if      (DriveDir.x >  0.5f && p.x >  half) p.x = -half;
        else if (DriveDir.x < -0.5f && p.x < -half) p.x =  half;
        if      (DriveDir.z >  0.5f && p.z >  half) p.z = -half;
        else if (DriveDir.z < -0.5f && p.z < -half) p.z =  half;
        transform.position = p;
    }

    void Crash()
    {
        if (_crashed) return;
        _crashed = true;
        transform.Rotate(0f, Random.Range(-CRASH_ARC, CRASH_ARC), 0f, Space.World);
        if (Random.value < 0.55f)
            gameObject.AddComponent<CarFire>();
    }
}
