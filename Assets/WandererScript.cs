using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandererScript : MonoBehaviour
{
    [SerializeField]
    EnemyProperties mainScript;
    [SerializeField]
    private GameObject[] removeAfterDead;
    [Range(0.0f, 10.0f), SerializeField]
    private float delay;

    public void Start()
    {
        mainScript.func = Funtion;
    }

    public void Funtion()
    {
        // this is wanderer specific code that only work for him.
        for (int i = 0; i < removeAfterDead.Length; i++)
        {
            Destroy(removeAfterDead[i], delay);
        }
    }
}
