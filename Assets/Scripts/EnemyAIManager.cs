using UnityEngine;
public enum Transition
{
    IDLE,
    WANDER,
    CHASE
}

public class EnemyAIManager : Singleton<EnemyAIManager>
{
    public ThirdPersonControl player;
    EnemyAIManager lastActiveCanvas;
    public string transitionParameter = "State";
    protected override void Awake()
    {
        base.Awake();
    }
}