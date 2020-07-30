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
    private float shaderSpeed = 0.005f;
    [SerializeField]
    private Color fadeColor = Color.red;
    [SerializeField]
    private Renderer renderer;

    private float control;
    private IEnumerator coroutine;

    public void Start()
    {
        mainScript.func = Funtion;
        control = -1.0f;
        enabled = false;
    }

    public void Funtion()
    {
        coroutine = StartShaderAfter(delay);
        StartCoroutine(coroutine);
        // this is wanderer specific code that only work for him.
        for (int i = 0; i < removeAfterDead.Length; i++)
        {
            Destroy(removeAfterDead[i], delay);
        }
    }

    public void Update()
    {
        control += shaderSpeed;
        if (renderer.material.HasProperty("control"))
        {
            renderer.material.SetFloat("control" ,control);
        }

        if (control >= 0.5f)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator StartShaderAfter(float time)
    {
        yield return new WaitForSeconds(time);

        if (renderer.material.HasProperty("edgecolor"))
        {
            renderer.material.SetColor("edgecolor", fadeColor);
        }

        enabled = true;
    }
}
