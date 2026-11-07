using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] List<StatCard> cardList;
    public void NewCardAdded(StatCard addedCard)
    {
        cardList.Add(addedCard);
        
    }
}
