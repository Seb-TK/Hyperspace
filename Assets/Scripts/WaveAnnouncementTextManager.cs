using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;


public class WaveAnnouncementTextManager : MonoBehaviour
{
    
    [SerializeField] private float waitTime;
    [SerializeField] private float fadeTime;
    [SerializeField] private GameObject waveAnnouncement;
    [SerializeField] private GameObject waveEnemyListTextObj;
    private TextMeshProUGUI waveAnnouncementText;
    private TextMeshProUGUI waveEnemyListText;

    void Awake()
    {
        waveAnnouncementText = waveAnnouncement.GetComponent<TextMeshProUGUI>();
        waveEnemyListText = waveEnemyListTextObj.GetComponent<TextMeshProUGUI>();
        EnemyManager.AnnounceWave += AnnounceWave;
        waveAnnouncementText.alpha = 0f;
        waveEnemyListText.alpha = 0f;
    }

    private void AnnounceWave(int wave)
    {
        StartCoroutine(AnnounceWaveCoroutine(wave));
    }


    IEnumerator AnnounceWaveCoroutine(int wave)
    {
        yield return new WaitUntil(() => UpgradeManager.isWaitingForUpgradeChoice == false);
        EnemyManager enemyManager = FindAnyObjectByType<EnemyManager>().GetComponent<EnemyManager>();
        List<EnemyDataObj>currentEnemies = enemyManager.currentEnemyList;
        waveAnnouncementText.SetText("Wave " + wave);
        String waveEnemyListNewtext = "Current Possible Enemies:\n";
        
        for(int i = 0; i < currentEnemies.Count; i++)
        {
            if(i > 0)
            {
                waveEnemyListNewtext += ", ";
            }
            waveEnemyListNewtext += currentEnemies[i].name;
        }
        waveEnemyListText.SetText(waveEnemyListNewtext);

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
            waveAnnouncementText.alpha = Mathf.Lerp(startValue, endValue, t);
            waveEnemyListText.alpha = Mathf.Lerp(startValue, endValue, t);
            yield return null;
        }
        waveAnnouncementText.alpha = endValue;
        waveEnemyListText.alpha = endValue;
        yield return null;
    }
}
