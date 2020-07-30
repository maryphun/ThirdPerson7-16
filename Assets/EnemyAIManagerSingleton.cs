using UnityEngine;

public class EnemyAIManager : MonoBehaviour
{
    private static EnemyAIManager _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    //Rest of your class code

}