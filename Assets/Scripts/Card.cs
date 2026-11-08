using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Card : MonoBehaviour
{
    public int cardSpot;
    [SerializeField] private float rotationAnimationAmount;
    [SerializeField] private float startAnimationEndingPosChangeX;
    [SerializeField] private float startAnimationEndingPosChangeY;
    [SerializeField] private float startAnimationEndingPosChangeZ;
    [SerializeField] private float startingAnimationPosX;
    [SerializeField] private float startingAnimationPosY;
    [SerializeField] private float startingAnimationPosZ;
    [SerializeField] private float animationDuration;
    [SerializeField] private float waitTime;
    [SerializeField] private float leaveSpeed;
    [SerializeField] private float canBeChosenWaitTime;
    [SerializeField] private float cardEndingPosOffsetPerCard;
    [SerializeField] private float cardStartPosOffsetPerCard;
    [SerializeField] private TextMeshPro titleText;
    [SerializeField] private TextMeshPro rarityText;
    [SerializeField] private TextMeshPro descriptionTextObj;
    public static bool aCardWasChosen;
    private bool canBeChosen;
    public static bool isWaitingForCanBeChosen;
    private bool thisCardWasChosen;
    private PlayerStats.StatCard cardData;
    private bool leaveAnimationAlreadyActivated;

    
    void Start()
    {
        transform.localPosition = new Vector3(startingAnimationPosX,startingAnimationPosY,startingAnimationPosZ);
        thisCardWasChosen = false;
        isWaitingForCanBeChosen = false;
        aCardWasChosen = false;
        leaveAnimationAlreadyActivated = false;
        StartCoroutine(SpawnAnimation(cardSpot));
        MakeCardData();
        SetCardText();
    }

    void MakeCardData()
    {
        int statTypesLength = Enum.GetValues(typeof(CardManager.StatTypes)).Length;
        cardData = new PlayerStats.StatCard();
        cardData.statType = (CardManager.StatTypes)Random.Range(0, statTypesLength);
        cardData = new PlayerStats.StatCard();
        int rarity = GetRarity();
        cardData.Rarity = CardManager.Rarities[rarity];
        cardData.Value = CardManager.RarityValues[rarity];
    }

    private void SetCardText()
    {
        string titleTextString = cardData.statType.ToString();
        string output = "";
        output += titleTextString[0];
        for(int i= 1; i < titleTextString.Length; i++)
        {
            if (!char.IsLower(titleTextString[i]))
            {
                output += " ";
            }
            output += titleTextString[i];
        }
        titleText.SetText(output);
        rarityText.SetText(cardData.Rarity);
        string descriptionText = "Increases " + output + " by " + cardData.Value + "%";
        descriptionTextObj.SetText(descriptionText);
    }

    private int GetRarity()
    {
        cardData = new PlayerStats.StatCard();
        List<int> weights= CardManager.RarityWeights;
        int totalWeight = 0;
        for(int i = 0; i < weights.Count; i++)
        {
            totalWeight += weights[i];
        }
        int random = Random.Range(0,totalWeight);
        int result = 0;
        for(int i = 0; i < weights.Count; i++)
        {
            if(random < weights[i])
            {
                result = i;
                break;
            }
            random -= weights[i];
        }
        return result;
    }

    void Update()
    {
        if (!leaveAnimationAlreadyActivated)
        {
                if(aCardWasChosen == true)
            {
                if(thisCardWasChosen == false)
                {
                    StartCoroutine(LeavingAnimation());
                    leaveAnimationAlreadyActivated = true;
                }
            }
        }
    }
    IEnumerator SpawnAnimation(int cardSpot)
    {
        yield return new WaitForSeconds(cardSpot * waitTime);
        Vector3 startPos = transform.localPosition;
        startPos.x -= cardStartPosOffsetPerCard;
        startPos.x += cardStartPosOffsetPerCard * cardSpot;
        transform.localPosition = startPos;
        Vector3 posChange = new Vector3(startAnimationEndingPosChangeX, startAnimationEndingPosChangeY, startAnimationEndingPosChangeZ);
        posChange.x -= cardEndingPosOffsetPerCard;
        posChange.x += cardEndingPosOffsetPerCard * cardSpot;
        yield return StartCoroutine(MoveAnimation(posChange, animationDuration));
        StartCoroutine(WaitForCanBeChosen());
    }

    IEnumerator SpawnAnimationWithRotation(int cardSpot)
    {
        yield return new WaitForSeconds(cardSpot * waitTime);
        transform.Rotate(0,rotationAnimationAmount,0);
        Quaternion startingRotation = transform.localRotation;
        Vector3 startingPos = transform.localPosition;
        Vector3 targetPos = new Vector3(-startAnimationEndingPosChangeX, startAnimationEndingPosChangeY, startAnimationEndingPosChangeZ);
        targetPos.x += startAnimationEndingPosChangeX * cardSpot;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime/animationDuration;
            float angle = Mathf.Lerp(0f,rotationAnimationAmount, t);
            transform.localRotation = startingRotation * Quaternion.Euler(0f, angle, 0f);
            transform.localPosition = Vector3.Slerp(startingPos, targetPos, t);
            yield return null;
        }
        transform.localRotation = Quaternion.identity;
        transform.localPosition = targetPos;
        StartCoroutine(WaitForCanBeChosen());
    }
    IEnumerator WaitForCanBeChosen()
    {
        if(isWaitingForCanBeChosen)
        {
            yield return new WaitUntil(() => isWaitingForCanBeChosen == false);
            canBeChosen = true;
        }
        else
        {
            isWaitingForCanBeChosen = true;
            yield return new WaitForSeconds(canBeChosenWaitTime);
            canBeChosen = true;
            isWaitingForCanBeChosen = false;
        }
    }
    void OnMouseOver()
    {
        if(canBeChosen & Input.GetMouseButtonDown(0))
        {
            thisCardWasChosen = true;
            aCardWasChosen = true;
            canBeChosen = false;
            StartCoroutine(SelectedAnimation());
        }
    }
    IEnumerator SelectedAnimation()
    {
        leaveAnimationAlreadyActivated = true;
        Vector3 leaveMoveAmount = new Vector3(0,10,0);
        yield return StartCoroutine(MoveAnimation(leaveMoveAmount, leaveSpeed));
        AddCard();
        Destroy(gameObject);
        yield return null;
    }

    private void AddCard()
    {
        PlayerStats.CurrentCards.Add(cardData);
        PlayerStats.CurrentModifiers[cardData.statType] += cardData.Value / 100;
    }

    IEnumerator LeavingAnimation()
    {
        Vector3 leaveMoveAmount = new Vector3(0,-10,0);
        yield return StartCoroutine(MoveAnimation(leaveMoveAmount, leaveSpeed));
        Destroy(gameObject);
    }

    IEnumerator MoveAnimation(Vector3 moveAmount, float duration)
    {
        Vector3 startingPos = transform.localPosition;
        Vector3 endPos = transform.localPosition + moveAmount;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime/duration;
            transform.localPosition = Vector3.Slerp(startingPos, endPos, t);
            yield return null;
        }
    }
    
}
