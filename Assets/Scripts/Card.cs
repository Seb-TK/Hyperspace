using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int cardSpot;
    [SerializeField] float rotationAnimationAmount;
    [SerializeField] float locationOffsetX;
    [SerializeField] float locationOffsetY;
    [SerializeField] float locationOffsetZ;
    [SerializeField] float animationDuration;
    [SerializeField] float waitTime;
    [SerializeField] float leaveSpeed;
    [SerializeField] StatCard cardData;
    public static bool wasChosen;
    private bool canBeChosen;
    
    void Start()
    {
        StartCoroutine(SpawnAnimation(cardSpot));
    }
    void Update()
    {
        if(wasChosen == true)
        {
            StartCoroutine(LeavingAnimation());
        }
    }

    IEnumerator SpawnAnimation(int cardSpot)
    {
        yield return new WaitForSeconds(cardSpot * waitTime);
        transform.Rotate(0,rotationAnimationAmount,0);
        Quaternion startingRotation = transform.rotation;
        Vector3 startingPos = transform.position;
        Vector3 targetPos = new Vector3(-locationOffsetX, locationOffsetY, locationOffsetZ);
        targetPos.x += locationOffsetX * cardSpot;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime/animationDuration;
            float angle = Mathf.Lerp(0f,rotationAnimationAmount, t);
            transform.rotation = startingRotation * Quaternion.Euler(0f, angle, 0f);
            transform.position = Vector3.Slerp(startingPos, targetPos, t);
            yield return null;
        }
        transform.rotation = Quaternion.identity;
        transform.position = targetPos;
        canBeChosen = true;
    }
    void OnMouseOver()
    {
        if(canBeChosen & Input.GetMouseButtonDown(0))
        {
            chooseCard();
        }
    }
    void chooseCard()
    {
        wasChosen = true;
        SelectCard();
        SelectedAnimation();
    }
    void SelectCard()
    {
        StartCoroutine(SelectedAnimation());
        CardManager cardManager = FindAnyObjectByType<CardManager>();
        cardManager.NewCardAdded(cardData);
    }
    IEnumerator SelectedAnimation()
    {
        for(int i = 0; i < 1000; i++)
        {
            transform.position += new Vector3(0,0,leaveSpeed);
            yield return null;
        }
        yield return null;
        Destroy(gameObject);
    }

    IEnumerator LeavingAnimation()
    {
        for(int i = 0; i < 1000; i++)
        {
            transform.position -= new Vector3(0,0,leaveSpeed);
            yield return null;
        }
        yield return null;
        Destroy(gameObject);
    }
    
}
