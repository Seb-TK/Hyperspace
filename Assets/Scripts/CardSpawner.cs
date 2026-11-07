using System.Collections;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    [SerializeField] private GameObject card;
    public void SpawnCards()
    {
        for(int i = 0; i < 3; i++)
        {
            Instantiate(card);
            card.GetComponent<Card>().cardSpot = i;
        }
    }
}
