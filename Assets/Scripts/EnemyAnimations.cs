using System;
using System.Collections;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    private Renderer renderer;
    private bool isGettingHit;
    [SerializeField] private AnimationCurve damageAnimationCurve;
    [SerializeField] private float onHitColor = 99f;
    [SerializeField] private float onHitSaturation = 99f;
    [SerializeField] private float onHitValue = -99f;
    [SerializeField] private float hitAnimationTime;
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    public void getHit()
    {
        if(isGettingHit == false)
        {
            isGettingHit = true;
            Color currentColor = renderer.material.color;
            Color.RGBToHSV(currentColor, out float currentHue, out float currentSaturation, out float currentValue);
            onHitColor += currentHue;
            onHitSaturation += currentSaturation;
            onHitValue += currentValue;
            StartCoroutine(colorLerp(onHitColor, onHitSaturation, onHitValue, hitAnimationTime));
        
        }
    }

    private void setColor(float color, float saturation, float value)
    {
        renderer.material.color = Color.HSVToRGB(color, saturation, value);        
    }

    private IEnumerator colorLerp(float targetColor, float targetSaturation, float targetValue, float lengthInSeconds)
    {
        float startTime = Time.time;
        float endTime = Time.time + lengthInSeconds;
        Color startColorNonFloat = renderer.material.color;
        Color.RGBToHSV(startColorNonFloat, out float startColor, out float startSaturation, out float startValue);
        Debug.Log(startColorNonFloat);
        while (Time.time < endTime)
        {
            float timeInPercent = Mathf.InverseLerp(startTime, endTime, Time.time);
            
            float pointOnCurve = damageAnimationCurve.Evaluate(timeInPercent);

            float colorLerp = Mathf.Lerp(startColor, targetColor, pointOnCurve);
            float saturationLerp = Mathf.Lerp(startSaturation, targetSaturation, pointOnCurve);
            float valueLerp = Mathf.Lerp(startValue, targetValue, pointOnCurve);
            Debug.Log("startcolor: "+startColor);
            Debug.Log("startSaturation: "+startSaturation);
            Debug.Log("startValue: "+startValue);
            Debug.Log("targetcolor: "+targetColor);
            Debug.Log("targetSaturation: "+targetSaturation);
            Debug.Log("targetValue: "+targetValue);
            Debug.Log("time.time: "+ Time.time);
            Debug.Log("currentTime in percent: "+timeInPercent);
            Debug.Log("point on curve: "+pointOnCurve);
            Debug.Log("c: "+colorLerp);
            Debug.Log(" s: "+saturationLerp);
            Debug.Log(" v: "+valueLerp);
            renderer.material.color = Color.HSVToRGB(colorLerp, saturationLerp, valueLerp);
            Debug.Log("");

            yield return null;
        }
        Debug.Log("");
        isGettingHit = false;
    }
}
