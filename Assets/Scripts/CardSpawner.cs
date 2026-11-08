using System.Collections;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    [SerializeField] private GameObject card;
    public void SpawnCards()
    {
        for(int i = 0; i < 3; i++)
        {
            Camera cam = Camera.main;
            Instantiate(card, cam.transform);
            card.GetComponent<Card>().cardSpot = i;
        }
    }
}
