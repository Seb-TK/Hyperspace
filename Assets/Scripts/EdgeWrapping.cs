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



    void Start()
    {
        actualPosition = transform.position;
        closestPosition = transform.position;
        worldPlane = FindAnyObjectByType<WorldPlane>();
        playerPosBeforeTeleport = transform.position;
    }

    // issue is that if you make the edge wrap alg thing actually tp you to the other side, the edge
    // teleport will cause an issue. the idea is that it needs to be for everything even bullets and stuff
    //
    
    //execution order is sooner than default so this runs first
    void Update()
    {
        transform.position = actualPosition;
        // if (GetComponent<EnemyMovement>() != null)
        // {
        //     Debug.Log("this runs first");
        //     Debug.Log("in edge wrapping regular update my position is: " + transform.position);
        // }
    }

    // refactor so that you dont have to write closest position in enemy movement 
    // and you can just do the default, do this by having it be in the closest position then 
    //when it comes back to edge wrapping you save the changes to the actual position and
    // do edge teleport
    void LateUpdate()
    {
        EdgeTeleport();
        actualPosition = transform.position;

        //blue is real one red is fake imposter evil clone
        Vector3 point = actualPosition;
        Debug.DrawRay(point, Vector3.right * 0.5f, Color.blue);
        Debug.DrawRay(point, Vector3.up * 0.5f, Color.blue);

        if(GetComponent<PlayerData>() == null)
        {
            // EdgeWrapAlg();
            // transform.position = closestPosition;

            GameObject player = FindAnyObjectByType<PlayerData>().gameObject;
            Vector3 playerPos = player.transform.position;
            float mapWidth = worldPlane.transform.localScale.x / 2;
            float mapHeight = worldPlane.transform.localScale.y / 2;
            Vector3 newPosition = transform.position;
            newPosition.x = EdgeWrapOnAnyAxis(transform.position.x,playerPos.x,mapWidth);
            newPosition.y = EdgeWrapOnAnyAxis(transform.position.y,playerPos.y,mapHeight);
            newPosition.z = EdgeWrapOnAnyAxis(transform.position.z, dimensionLocation, dimensionRange);
            closestPosition = newPosition;
            transform.position = closestPosition;
        }
        point = closestPosition;
        Debug.DrawRay(point, Vector3.right * 0.5f, Color.red);
        Debug.DrawRay(point, Vector3.up * 0.5f, Color.red);
    }

    private void EdgeWrapAlg()
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
    private float EdgeWrapOnAnyAxis(float position, float playerPos, float range)
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

        if(transform.position != newPos & GetComponent<EnemyMovement>() != null)
        {
            Debug.Log("teleport");
        }
        
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
