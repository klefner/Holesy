using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitarySystem : MonoBehaviour
{
    const float PLANE_SPEED     = 22f;
    const float PLANE_ALTITUDE  = 22f;
    const float PARA_FALL_TIME  = 2.0f;
    const float SOLDIER_SPEED   = 2.2f;
    const float SOLDIER_RANGE   = 16f;
    const float SHOT_DAMAGE_PLR = 0.06f;
    const float SHOT_DAMAGE_AI  = 0.03f;
    const float SHOTS_PER_SEC   = 10f;
    const float RELOAD_TIME     = 1.0f;
    const int   BURST_COUNT     = 30;
    const float MIN_WAVE_INT    = 8f;
    const float MAX_WAVE_INT    = 10f;

    static readonly float[] WAVE_STARTS = { 0f, 40f, 70f, 100f };

    private bool  _running;
    private float _waveTimer;
    private int   _waveIndex;

    private readonly List<ActiveSoldier> _soldiers = new List<ActiveSoldier>();

    public void Begin()
    {
        _running   = true;
        _waveTimer = Random.Range(MIN_WAVE_INT, MAX_WAVE_INT);
        _waveIndex = 0;
    }

    public void Stop()
    {
        _running = false;
        StopAllCoroutines();
    }

    void Update()
    {
        if (!_running) return;

        if (GameManager.Instance == null) return;
        float elapsed = GameManager.GAME_DURATION - GameManager.Instance.TimeRemaining;
        for (int i = WAVE_STARTS.Length - 1; i >= 0; i--)
        {
            if (elapsed >= WAVE_STARTS[i]) { _waveIndex = i; break; }
        }

        _waveTimer -= Time.deltaTime;
        if (_waveTimer <= 0f)
        {
            _waveTimer = Random.Range(MIN_WAVE_INT, MAX_WAVE_INT);
            StartCoroutine(SpawnWave());
        }

        for (int i = _soldiers.Count - 1; i >= 0; i--)
        {
            if (!_soldiers[i].Update(Time.deltaTime, _waveIndex))
                _soldiers.RemoveAt(i);
        }
    }

    IEnumerator SpawnWave()
    {
        float half = GameManager.HALF;
        bool  side = Random.value < 0.5f;
        float coord = Random.Range(-half, half);
        float dir   = Random.value < 0.5f ? 1f : -1f;

        Vector3 start, end;
        if (side)
        {
            start = new Vector3(-half - 20f, PLANE_ALTITUDE, coord);
            end   = new Vector3( half + 20f, PLANE_ALTITUDE, coord);
        }
        else
        {
            start = new Vector3(coord, PLANE_ALTITUDE, -half - 20f);
            end   = new Vector3(coord, PLANE_ALTITUDE,  half + 20f);
        }
        if (dir < 0f) { var tmp = start; start = end; end = tmp; }

        StartCoroutine(FlyPlane(start, end));
        yield return null;
    }

    IEnumerator FlyPlane(Vector3 start, Vector3 end)
    {
        var planeGO = CreatePlaneMesh(start);
        float dist  = Vector3.Distance(start, end);
        float time  = dist / PLANE_SPEED;
        float t     = 0f;
        int   dropsDone = 0;
        float nextDrop  = Random.Range(0.3f, 0.6f);

        while (t < 1f)
        {
            t += Time.deltaTime / time;
            planeGO.transform.position = Vector3.Lerp(start, end, t);
            planeGO.transform.rotation = Quaternion.LookRotation((end - start).normalized);

            if (_waveIndex >= 1 && t > nextDrop && dropsDone < 3)
            {
                dropsDone++;
                nextDrop += Random.Range(0.15f, 0.25f);
                int soldiers = Random.Range(4, 9);
                StartCoroutine(DropParatrooper(planeGO.transform.position, soldiers));
            }

            yield return null;
        }

        Destroy(planeGO);
    }

    IEnumerator DropParatrooper(Vector3 dropPos, int soldierCount)
    {
        for (int i = 0; i < soldierCount; i++)
        {
            Vector3 landPos = dropPos + new Vector3(
                Random.Range(-12f, 12f), 0f, Random.Range(-12f, 12f));
            landPos.y = 0f;
            landPos.x = Mathf.Clamp(landPos.x, -GameManager.HALF + 3f, GameManager.HALF - 3f);
            landPos.z = Mathf.Clamp(landPos.z, -GameManager.HALF + 3f, GameManager.HALF - 3f);
            StartCoroutine(AnimateParatrooper(dropPos, landPos));
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator AnimateParatrooper(Vector3 dropPos, Vector3 landPos)
    {
        var paraGO = CreateParatrooperMesh(dropPos);
        float t    = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / PARA_FALL_TIME;
            float y = Mathf.Lerp(PLANE_ALTITUDE, 0f, Mathf.Sqrt(t));
            paraGO.transform.position = new Vector3(
                Mathf.Lerp(dropPos.x, landPos.x, t),
                y,
                Mathf.Lerp(dropPos.z, landPos.z, t));
            yield return null;
        }

        Destroy(paraGO);
        SpawnSoldier(landPos);
    }

    void SpawnSoldier(Vector3 pos)
    {
        var go = CreateSoldierMesh(pos);
        _soldiers.Add(new ActiveSoldier(go, pos));
    }

    // ── Soldier logic ─────────────────────────────────────────────────────────

    class ActiveSoldier
    {
        readonly GameObject _go;
        Vector3  _pos;
        HoleBase _target;
        float    _shootTimer;
        int      _burstLeft;
        float    _reloadTimer;
        bool     _reloading;

        public ActiveSoldier(GameObject go, Vector3 pos)
        {
            _go  = go;
            _pos = pos;
        }

        public bool Update(float dt, int waveIndex)
        {
            if (_go == null) return false;
            var gm = GameManager.Instance;
            if (gm.State != GameManager.GameState.Playing) return false;

            _target = NearestHole();
            if (_target == null) return true;

            float dist = XZDist(_pos, _target.transform.position);

            if (dist > SOLDIER_RANGE * 0.7f)
            {
                Vector3 dir = (_target.transform.position - new Vector3(_pos.x, 0f, _pos.z)).normalized;
                _pos += dir * SOLDIER_SPEED * dt;
                _pos.y = 0f;
                _go.transform.position = _pos;
            }

            if (dist <= SOLDIER_RANGE)
            {
                if (_reloading)
                {
                    _reloadTimer -= dt;
                    if (_reloadTimer <= 0f) { _reloading = false; _burstLeft = BURST_COUNT; }
                }
                else
                {
                    _shootTimer -= dt;
                    if (_shootTimer <= 0f)
                    {
                        _shootTimer = 1f / SHOTS_PER_SEC;
                        TryShoot(dist, waveIndex);
                        _burstLeft--;
                        if (_burstLeft <= 0)
                        {
                            _reloading   = true;
                            _reloadTimer = RELOAD_TIME;
                        }
                    }
                }
            }

            return true;
        }

        void TryShoot(float dist, int waveIndex)
        {
            float hitChance = dist < 6f ? 0.6f : dist < 12f ? 0.4f : 0.2f;
            if (Random.value > hitChance) return;

            float mult   = waveIndex >= 2 ? 1.25f : 1.0f;
            float baseDmg = _target.IsPlayer ? SHOT_DAMAGE_PLR : SHOT_DAMAGE_AI;
            _target.TakeDamage(baseDmg * mult);
        }

        HoleBase NearestHole()
        {
            HoleBase best = null;
            float bestDist = float.MaxValue;
            foreach (var h in GameManager.Instance.AllHoles)
            {
                if (!h.Alive) continue;
                float d = XZDist(_pos, h.transform.position);
                if (d < bestDist) { bestDist = d; best = h; }
            }
            return best;
        }

        static float XZDist(Vector3 a, Vector3 b)
        {
            float dx = a.x - b.x, dz = a.z - b.z;
            return Mathf.Sqrt(dx * dx + dz * dz);
        }
    }

    // ── Mesh helpers ──────────────────────────────────────────────────────────

    static GameObject CreatePlaneMesh(Vector3 pos)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = "Plane";
        go.transform.position   = pos;
        go.transform.localScale = new Vector3(2f, 0.5f, 6f);
        SetURPColor(go, new Color(0.6f, 0.6f, 0.7f));
        Destroy(go.GetComponent<Collider>());
        return go;
    }

    static GameObject CreateParatrooperMesh(Vector3 pos)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        go.name = "Paratrooper";
        go.transform.position   = pos;
        go.transform.localScale = Vector3.one * 0.5f;
        SetURPColor(go, new Color(0.25f, 0.35f, 0.25f));
        Destroy(go.GetComponent<Collider>());
        return go;
    }

    static GameObject CreateSoldierMesh(Vector3 pos)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        go.name = "Soldier";
        go.transform.position   = pos + Vector3.up * 0.5f;
        go.transform.localScale = new Vector3(0.4f, 0.8f, 0.4f);
        SetURPColor(go, new Color(0.30f, 0.40f, 0.25f));
        Destroy(go.GetComponent<Collider>());
        return go;
    }

    static void SetURPColor(GameObject go, Color c)
    {
        var mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.SetColor("_BaseColor", c);
        mat.SetFloat("_Smoothness", 0.1f);
        go.GetComponent<Renderer>().material = mat;
    }
}
