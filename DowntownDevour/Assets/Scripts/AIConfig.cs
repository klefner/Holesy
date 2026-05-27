using UnityEngine;

[System.Serializable]
public class AIConfig
{
    public string Name;
    public Color  HoleColor;

    // Personality weights
    public float Greed;       // preference for high-value objects
    public float Laziness;    // reluctance to change targets
    public float Aggression;  // willingness to chase other holes
    public float WanderBias;  // chance to just wander

    // Timing
    public float DecisionMin;
    public float DecisionMax;

    // Perception
    public float VisionRange;    // Mathf.Infinity = omniscient (Gulp)
    public float ReactionDelay;  // seconds before acting on a decision

    public static AIConfig[] Defaults() => new[]
    {
        new AIConfig
        {
            Name = "Void", HoleColor = new Color(0.9f, 0.15f, 0.15f),
            Greed = 1.4f, Laziness = 0.6f, Aggression = 0.9f, WanderBias = 0.2f,
            DecisionMin = 0.5f, DecisionMax = 1.0f,
            VisionRange = 45f, ReactionDelay = 0.45f
        },
        new AIConfig
        {
            Name = "Maw", HoleColor = new Color(0.95f, 0.85f, 0.10f),
            Greed = 0.7f, Laziness = 1.3f, Aggression = 0.5f, WanderBias = 0.5f,
            DecisionMin = 0.8f, DecisionMax = 1.5f,
            VisionRange = 35f, ReactionDelay = 0.6f
        },
        new AIConfig
        {
            Name = "Gulp", HoleColor = new Color(0.20f, 0.80f, 0.25f),
            Greed = 1.0f, Laziness = 0.9f, Aggression = 1.4f, WanderBias = 0.1f,
            DecisionMin = 0.3f, DecisionMax = 0.7f,
            VisionRange = float.PositiveInfinity, ReactionDelay = 0f
        }
    };
}
