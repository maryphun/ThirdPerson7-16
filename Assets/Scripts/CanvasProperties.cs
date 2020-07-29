using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasProperties : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float aValue = 1;

    public CanvasGroup trans;
    public TextMeshProUGUI[] textmesh;

    private float originalAlpha;
    private float targetAlpha;
    private float lerpRate;
    private float lerp;
    private IEnumerator coroutine;

    void Start()
    {
        trans.alpha = aValue;
        enabled = false;
    }

    private void Update()
    {
        //if the alpha lerp is not done yet
        if (trans.alpha != targetAlpha)
        {
            lerp += lerpRate;
            trans.alpha = Mathf.Lerp(originalAlpha, targetAlpha, lerp);
        }
    }

    public void SetAlpha(float alpha, float incrementSpeed)
    {
        originalAlpha = trans.alpha;
        targetAlpha = alpha;
        lerpRate = incrementSpeed;
        lerp = 0.0f;
        enabled = true;
    }

    public void SetAlpha(float alpha, float incrementSpeed, float stayTime)
    {
        originalAlpha = trans.alpha;
        targetAlpha = alpha;
        lerpRate = incrementSpeed;
        lerp = 0.0f;
        coroutine = DelayFadeOut(stayTime);
        StopAllCoroutines();
        StartCoroutine(coroutine);
        enabled = true;
    }

    public void SetTextMesh(int index ,string text)
    {
        textmesh[index].SetText(text);
    }

    public void SetTextColor(int index, Color newcolor)
    {
        textmesh[index].color = newcolor;
    }

    private IEnumerator DelayFadeOut(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //reset
        originalAlpha = trans.alpha;
        targetAlpha = 0.0f;
        lerp = 0.0f;
    }
}
