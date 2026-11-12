using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Drawing;
using Unity.VisualScripting;
using System;

public class PlayerStats : MonoBehaviour
{   
    public static List<StatCard> CurrentCards = new List<StatCard>();
    [SerializeField] private List<StatCard> inspectorDisplayCurrentCards = new List<StatCard>();
    public static Dictionary<CrdMngr.Stats,float> Mods = new Dictionary<CrdMngr.Stats,float>();
    [SerializeField] private List<Modifier> startingModifiers;
    [SerializeField] private List<Modifier> currentModifiersView;

    [System.Serializable]
    public class StatCard
    {
        public CardType cardType;
        public PlayerData.WeaponType weaponType;
        public CrdMngr.Stats statType;
        public string Rarity;
        public float Value;
    }

    public enum CardType
    {
        Stat,
        Weapon,
        Ability
    }

    [System.Serializable]
    public class Modifier
    {
        public CrdMngr.Stats statType;
        public float Value;
    }

    public void ResetStatics()
    {
        CurrentCards.Clear();
        Mods.Clear();
    }

    void Start()
    {
        Mods.Clear();
        currentModifiersView.Clear();
        for(int i = 0; i < Enum.GetValues(typeof(CrdMngr.Stats)).Length; i++)
        {
            Mods.Add((CrdMngr.Stats)i, 1);
        }
    }

    void Update()
    {
        currentModifiersView.Clear();
        foreach(var pair in Mods)
            {
            Modifier newModifier = new Modifier();
            newModifier.statType = pair.Key;
            newModifier.Value = pair.Value;
            currentModifiersView.Add(newModifier);
        }
    }
}
