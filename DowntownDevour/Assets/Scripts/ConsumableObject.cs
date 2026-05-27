using UnityEngine;

public enum ObjectCategory { Person, Car, Tree, Building, Prop }

// Any city object that can be swallowed by a hole.
public class ConsumableObject : MonoBehaviour
{
    public float          Size     { get; private set; }
    public int            Tier     { get; private set; }
    public float          Value    { get; private set; }
    public ObjectCategory Category { get; private set; }
    public bool           IsConsumed { get; private set; }

    private bool    _falling;
    private Vector3 _fallDirection;
    private float   _fallVel;
    private float   _spinVel;
    private float   _scaleVel = 0f;
    private Vector3 _startScale;

    const float FALL_GRAVITY = 18f;
    const float SHRINK_TIME  = 0.45f;

    public void Init(float size, int tier, float value, ObjectCategory category)
    {
        Size     = size;
        Tier     = tier;
        Value    = value;
        Category = category;
        _startScale = transform.localScale;
    }

    void Update()
    {
        if (!_falling) return;

        // Spin and fall
        transform.Rotate(Vector3.up, _spinVel * Time.deltaTime, Space.World);
        _fallVel += FALL_GRAVITY * Time.deltaTime;
        transform.position += Vector3.down * (_fallVel * Time.deltaTime);

        // Shrink toward zero
        _scaleVel += Time.deltaTime / SHRINK_TIME;
        float t = Mathf.Clamp01(_scaleVel);
        transform.localScale = Vector3.Lerp(_startScale, Vector3.zero, t);

        if (t >= 1f)
            Destroy(gameObject);
    }

    public void MarkConsumed(HoleBase hole)
    {
        if (IsConsumed) return;
        IsConsumed = true;
        _falling   = true;
        _spinVel   = (Random.value - 0.5f) * 360f;
        _fallVel   = 0f;
        _scaleVel  = 0f;

        // Remove from manager list (objects are never re-added after consume)
        GameManager.Instance.AllObjects.Remove(this);
    }
}
