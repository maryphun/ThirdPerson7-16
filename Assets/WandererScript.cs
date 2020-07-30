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
    [SerializeField]
    private float control;
    [SerializeField]
    private Renderer renderer;

    public void Start()
    {
        mainScript.func = Funtion;
        control = -1.0f;
        enabled = false;
    }

    public void Funtion()
    {
        enabled = true;
        // this is wanderer specific code that only work for him.
        for (int i = 0; i < removeAfterDead.Length; i++)
        {
            Destroy(removeAfterDead[i], delay);
        }
    }

    public void Update()
    {
        control += 0.005f;
        if (renderer.material.HasProperty("control"))
        {
            Debug.Log(control);
            renderer.material.SetFloat("control" ,control);
        }
    }


}
