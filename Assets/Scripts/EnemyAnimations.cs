using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyAnimations : MonoBehaviour
{
    private Renderer renderer;
    [SerializeField] private AnimationCurve damageAnimationCurve;
    [SerializeField] private float onHitColor = 99f;
    [SerializeField] private float onHitSaturation = 99f;
    [SerializeField] private float onHitValue = -99f;
    [SerializeField] private float hitAnimationTime;
    Color currentColor;
    void Start()
    {
        renderer = GetComponent<Renderer>();
        currentColor = renderer.material.color;
    }

    public void GetHit()
    {
        Color.RGBToHSV(currentColor, out float currentHue, out float currentSaturation, out float currentValue);
        
        float inputColor = onHitColor + currentHue;
        float inputSaturation = onHitSaturation + currentSaturation;
        float inputValue = onHitValue + currentValue;
        
        StartCoroutine(ColorLerp(inputColor, inputSaturation, inputValue, hitAnimationTime));
    }

    private IEnumerator ColorLerp(float targetColor, float targetSaturation, float targetValue, float lengthInSeconds)
    {
        float startTime = Time.time;
        float endTime = Time.time + lengthInSeconds;
        Color startColorNonFloat = renderer.material.color;
        Color.RGBToHSV(startColorNonFloat, out float startColor, out float startSaturation, out float startValue);
        while (Time.time < endTime)
        {
            float timeInPercent = Mathf.InverseLerp(startTime, endTime, Time.time);
            
            float pointOnCurve = damageAnimationCurve.Evaluate(timeInPercent);

            float colorLerp = Mathf.Lerp(startColor, targetColor, pointOnCurve);
            float saturationLerp = Mathf.Lerp(startSaturation, targetSaturation, pointOnCurve);
            float valueLerp = Mathf.Lerp(startValue, targetValue, pointOnCurve);
            renderer.material.color = Color.HSVToRGB(colorLerp, saturationLerp, valueLerp);
            yield return null;
        }
    }
}
