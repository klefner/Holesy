using UnityEngine;

// AI brain for one hole.  Drives the HoleBase movement API.
public class AIHole : MonoBehaviour
{
    public HoleBase Hole   { get; set; }
    public AIConfig Config { get; set; }

    private enum AIState { Wander, ChaseObject, ChaseHole, Flee }
    private AIState _state = AIState.Wander;

    // Decision timer
    private float _decisionTimer;
    private float _pendingDelay;

    // Current target positions (already chosen, pending reaction delay)
    private Vector3 _pendingTarget;
    private bool    _hasPendingTarget;

    // Wander target
    private Vector3 _wanderTarget;

    void Start()
    {
        _decisionTimer = Random.Range(Config.DecisionMin, Config.DecisionMax);
        _wanderTarget  = RandomWanderPoint();
    }

    void Update()
    {
        if (Hole == null || Config == null) return;
        if (!Hole.Alive) return;
        if (GameManager.Instance == null || GameManager.Instance.State != GameManager.GameState.Playing) return;

        _decisionTimer -= Time.deltaTime;
        if (_decisionTimer <= 0f)
        {
            MakeDecision();
            _decisionTimer = Random.Range(Config.DecisionMin, Config.DecisionMax);
        }

        // Apply pending target after reaction delay
        if (_hasPendingTarget)
        {
            _pendingDelay -= Time.deltaTime;
            if (_pendingDelay <= 0f)
            {
                Hole.SetTargetPosition(_pendingTarget);
                _hasPendingTarget = false;
            }
        }

        // Adjust move speed for flee state
        Hole.SetMoveSpeed(_state == AIState.Flee ? GameManager.AI_FLEE_SPEED : GameManager.AI_SPEED);
    }

    void MakeDecision()
    {
        var gm = GameManager.Instance;

        // 1 – Check for threat: a bigger hole nearby
        HoleBase threat = FindThreat();
        if (threat != null)
        {
            _state = AIState.Flee;
            ScheduleTarget(FleeFrom(threat.transform.position));
            return;
        }

        // 2 – Wandering bias chance
        if (Random.value < Config.WanderBias)
        {
            _state = AIState.Wander;
            _wanderTarget = RandomWanderPoint();
            ScheduleTarget(_wanderTarget);
            return;
        }

        // 3 – Aggression: chase a smaller hole?
        if (Random.value < Config.Aggression * 0.5f)
        {
            HoleBase prey = FindPrey();
            if (prey != null)
            {
                _state = AIState.ChaseHole;
                ScheduleTarget(prey.transform.position);
                return;
            }
        }

        // 4 – Chase a consumable object
        ConsumableObject target = FindBestObject();
        if (target != null)
        {
            _state = AIState.ChaseObject;
            ScheduleTarget(target.transform.position);
            return;
        }

        // 5 – Default wander
        _state = AIState.Wander;
        _wanderTarget = RandomWanderPoint();
        ScheduleTarget(_wanderTarget);
    }

    void ScheduleTarget(Vector3 pos)
    {
        _pendingTarget    = pos;
        _pendingDelay     = Config.ReactionDelay;
        _hasPendingTarget = true;

        // Zero reaction delay = omniscient, apply immediately
        if (Config.ReactionDelay <= 0f)
        {
            Hole.SetTargetPosition(pos);
            _hasPendingTarget = false;
        }
    }

    // ── Target selection ──────────────────────────────────────────────────────

    HoleBase FindThreat()
    {
        float myR = Hole.Radius;
        foreach (var h in GameManager.Instance.AllHoles)
        {
            if (h == Hole || !h.Alive) continue;
            if (h.Radius <= myR * 1.02f) continue;  // not big enough to be a threat

            float dist = XZDist(Hole.transform.position, h.transform.position);
            if (dist < (Config.VisionRange == float.PositiveInfinity ? 9999f : Config.VisionRange))
                return h;
        }
        return null;
    }

    HoleBase FindPrey()
    {
        HoleBase best = null;
        float    bestScore = float.MinValue;
        float    myR = Hole.Radius;

        foreach (var h in GameManager.Instance.AllHoles)
        {
            if (h == Hole || !h.Alive) continue;
            if (h.Radius >= myR * 0.98f) continue;  // can't eat it

            float dist = XZDist(Hole.transform.position, h.transform.position);
            float range = Config.VisionRange == float.PositiveInfinity ? 9999f : Config.VisionRange;
            if (dist > range) continue;

            float score = h.Score * Config.Aggression - dist * Config.Laziness;
            if (score > bestScore) { bestScore = score; best = h; }
        }
        return best;
    }

    ConsumableObject FindBestObject()
    {
        ConsumableObject best = null;
        float bestScore = float.MinValue;

        foreach (var obj in GameManager.Instance.AllObjects)
        {
            if (obj.IsConsumed || obj.Size > Hole.Radius) continue;

            float dist = XZDist(Hole.transform.position, obj.transform.position);
            float range = Config.VisionRange == float.PositiveInfinity ? 9999f : Config.VisionRange;
            if (dist > range) continue;

            float score = obj.Value * Config.Greed - dist * Config.Laziness;
            if (score > bestScore) { bestScore = score; best = obj; }
        }
        return best;
    }

    Vector3 FleeFrom(Vector3 threat)
    {
        Vector3 away = (Hole.transform.position - threat).normalized;
        return Hole.transform.position + away * 30f;
    }

    Vector3 RandomWanderPoint()
    {
        float half = GameManager.HALF - 10f;
        return new Vector3(Random.Range(-half, half), 0f, Random.Range(-half, half));
    }

    static float XZDist(Vector3 a, Vector3 b)
    {
        float dx = a.x - b.x, dz = a.z - b.z;
        return Mathf.Sqrt(dx * dx + dz * dz);
    }
}
