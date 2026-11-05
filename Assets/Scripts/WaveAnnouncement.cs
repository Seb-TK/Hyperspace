using System.Collections;
using TMPro;
using UnityEngine;

public class WaveAnnouncement : MonoBehaviour
{
    TextMeshProUGUI text;
    [SerializeField] private float waitTime;
    [SerializeField] private float fadeTime;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        EnemyManager.AnnounceWave += AnnounceWave;
        text.alpha = 0f;
    }
    void Update()
    {
        // Debug.Log(text.alpha);
    }

    private void AnnounceWave(int wave)
    {
        text.SetText("Wave " + wave);
        StartCoroutine(AnnounceWaveCoroutine());
    }

    IEnumerator AnnounceWaveCoroutine()
    {
        yield return StartCoroutine(FadeText(0f,1f,fadeTime));
        yield return new WaitForSeconds(waitTime);
        yield return StartCoroutine(FadeText(1f,0f,fadeTime));
    }

    IEnumerator FadeText(float startValue, float endValue, float length)
    {
        float t = 0;
        while(t < 1)
        {
            t += Time.deltaTime / length;
            text.alpha = Mathf.Lerp(startValue, endValue, t);
            yield return null;
        }
        text.alpha = t;
        yield return null;
    }
}
