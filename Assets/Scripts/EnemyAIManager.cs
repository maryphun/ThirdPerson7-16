using UnityEngine;
public enum Transition
{
    IDLE,
    WANDER,
    CHASE,
    DEATH
}

public class EnemyAIManager : Singleton<EnemyAIManager>
{
    public ThirdPersonControl player;
    public string transitionParameter = "State";
}