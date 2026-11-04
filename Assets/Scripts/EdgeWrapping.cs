using System;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class EdgeWrapping : MonoBehaviour
{
    private WorldPlane worldPlane;
    private float dimensionLocation;
    private float dimensionRange;
    private int thisObjOnWhichSide;
    public Vector3 playerPosBeforeTeleport;
    public Vector3 closestPosition;
    public Vector3 actualPosition;
    private Vector3 cachePosition;



    void Start()
    {
        actualPosition = transform.position;
        closestPosition = transform.position;
        worldPlane = FindAnyObjectByType<WorldPlane>();
        playerPosBeforeTeleport = transform.position;
    }
    
    //execution order is sooner than default so this runs first among the last ones
    void LateUpdate()
    {
        if(GetComponent<PlayerData>() == null)
        {
            UpdateActualPosition();
            EdgeTeleport();
            actualPosition = transform.position;
            EdgeWrapAlg();
            cachePosition = transform.position;
        }
        else
        {
            EdgeTeleport();
        }
    }

    private void UpdateActualPosition()
    {
        Vector3 delta = transform.position - cachePosition;
        actualPosition += delta;
    }

    private void EdgeWrapAlg()
    { 
        GameObject player = FindAnyObjectByType<PlayerData>().gameObject;
        Vector3 playerPos = player.transform.position;
        float mapWidth = worldPlane.transform.localScale.x / 2;
        float mapHeight = worldPlane.transform.localScale.y / 2;
        Vector3 newPosition = transform.position;
        newPosition.x = EdgeWrapOnAxis(transform.position.x,playerPos.x,mapWidth);
        newPosition.y = EdgeWrapOnAxis(transform.position.y,playerPos.y,mapHeight);
        newPosition.z = EdgeWrapOnAxis(transform.position.z, dimensionLocation, dimensionRange);
        closestPosition = newPosition;
        transform.position = closestPosition;
    }

    private void EdgeWrapAlgRepeat()
    {   
        GameObject player = FindAnyObjectByType<PlayerData>().gameObject;
        Vector3 playerPos = player.transform.position;
        float mapWidth = worldPlane.transform.localScale.x;
        float mapHeight = worldPlane.transform.localScale.y;

        Vector3 distance = transform.position - playerPos;
        distance.x = Mathf.Repeat(distance.x + mapWidth / 2f, mapWidth);
        distance.y = Mathf.Repeat(distance.y + mapHeight / 2f, mapHeight);
        distance.x -= mapWidth / 2f;
        distance.y -= mapHeight / 2f;
        distance += playerPos;
        
        Vector3 newPosition = transform.position;
        newPosition.x = distance.x;
        newPosition.y = distance.y;
        closestPosition = newPosition;
    }
    private float EdgeWrapOnAxis(float position, float playerPos, float range)
    {
        if (position < 0)
        {
            thisObjOnWhichSide = 1;
        }
        if (position > 0)
        {
            thisObjOnWhichSide = -1;
        }
        float distanceToEdge = range - Mathf.Abs(position);
        float newPosition = (range + distanceToEdge) * thisObjOnWhichSide;
        float difference = Mathf.Abs(position - playerPos);
        float newDifference = Mathf.Abs(newPosition - playerPos);
        if(newDifference < difference)
        {
            return newPosition;
        } else {
            return position;
        }
    }

    public event Action OnPlayerTeleport;

    private void EdgeTeleport()
    {
        float boundWidthPos = worldPlane.boundWidth;
        float boundHeightPos = worldPlane.boundHeight;
        Vector3 newPos = transform.position;

        newPos.x = Mathf.Repeat(newPos.x + boundWidthPos, 2 * boundWidthPos);
        newPos.y = Mathf.Repeat(newPos.y + boundHeightPos, 2 * boundHeightPos);
        newPos.x -= boundWidthPos;
        newPos.y -= boundHeightPos;
        
        if(transform.position != newPos & GetComponent<PlayerData>() != null)
        {
            playerPosBeforeTeleport = transform.position;
            transform.position = newPos;
            OnPlayerTeleport?.Invoke();
        }
        else
        {
            transform.position = newPos;
        }

        
    }
}
