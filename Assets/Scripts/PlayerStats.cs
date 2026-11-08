using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Drawing;
using Unity.VisualScripting;
using System;

public class PlayerStats : MonoBehaviour
{
    [System.Serializable]
    public class StatCard
    {
        public CardManager.StatTypes statType;
        public string Rarity;
        public float Value;
    }
    [System.Serializable]
    public class Modifier
    {
        public CardManager.StatTypes statType;
        public float Value;
    }

    public static List<StatCard> CurrentCards = new List<StatCard>();
    [SerializeField] private List<StatCard> inspectorDisplayCurrentCards = new List<StatCard>();
    public static Dictionary<CardManager.StatTypes,float> CurrentModifiers = new Dictionary<CardManager.StatTypes,float>();
    [SerializeField] private List<Modifier> startingModifiers;
    [SerializeField] private List<Modifier> currentModifiersView;

    void Start()
    {
        for(int i = 0; i < Enum.GetValues(typeof(CardManager.StatTypes)).Length; i++)
        {
            CurrentModifiers.Add((CardManager.StatTypes)i, 1);
        }
        Debug.Log(CurrentModifiers.Count);
        inspectorDisplayCurrentCards = new List<StatCard>(CurrentCards);
        foreach(var pair in CurrentModifiers)
        {
            Modifier newModifier = new Modifier();
            newModifier.statType = pair.Key;
            newModifier.Value = pair.Value;
            currentModifiersView.Add(newModifier);
        }
    }

    void Update()
    {
        
    }

}
