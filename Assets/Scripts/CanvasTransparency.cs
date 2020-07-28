using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasTransparency : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float aValue = 1;

    public CanvasGroup trans;
    public TextMeshProUGUI textmesh;
    public TextMeshProUGUI keyInputTextMesh;

    private float originalAlpha;
    private float targetAlpha;
    private float lerpRate;
    private float lerp;
    void Start()
    {
        trans.alpha = aValue;
    }

    private void Update()
    {
        lerp += lerpRate;
        trans.alpha = Mathf.Lerp(originalAlpha, targetAlpha, lerp);
    }

    public void SetAlpha(float alpha, float delay)
    {
        originalAlpha = trans.alpha;
        targetAlpha = alpha;
        lerpRate = delay;
        lerp = 0.0f;
    }

    public void SetTextMesh(string text)
    {
        textmesh.SetText(text);
    }

    public void SetTextColor(Color newcolor)
    {
        textmesh.color = newcolor;
    }

    public void SetKeyInputTextMesh(string text)
    {
        keyInputTextMesh.SetText(text);
    }
}
