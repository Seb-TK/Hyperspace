using System;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class EdgeWrapping : MonoBehaviour
{
    private Vector3 savedPosition;
    private WorldPlane worldPlane;
    private float dimensionLocation;
    private float dimensionRange;
    private int thisObjOnWhichSide;
    public Vector3 playerPosBeforeTeleport;



    void Start()
    {
        savedPosition = transform.position;
        worldPlane = FindAnyObjectByType<WorldPlane>();
        playerPosBeforeTeleport = transform.position;
    }

    // issue is that if you make the edge wrap alg thing actually tp you to the other side, the edge
    // teleport will cause an issue. the idea is that it needs to be for everything even bullets and stuff
    //
    
    //execution order is sooner than default so this runs first
    void Update()
    {
        transform.position = savedPosition;
        if (GetComponent<EnemyMovement>() != null)
        {
            Debug.Log("this runs first");
            Debug.Log("in edge wrapping regular update my position is: " + transform.position);
        }
        

    }
    void LateUpdate()
    {
        // if (GetComponent<EnemyMovement>() != null)
        // {
        //     Debug.Log("this runs second");
        //     Debug.Log("in edge wrapping late update my starting positon is: " + transform.position);
        // }
        
        
        EdgeTeleport();

        savedPosition = transform.position;

        //red is real one blue is fake imposter evil clone
        Vector3 point = transform.position;
        Debug.DrawRay(point, Vector3.right * 0.5f, Color.red);
        Debug.DrawRay(point, Vector3.up * 0.5f, Color.red);

        if(GetComponent<PlayerData>() == null)
        {
            EdgeWrapAlg();
            
            Vector3 newPosition = transform.position;
            newPosition.z = EdgeWrapOnAnyAxis(transform.position.z, dimensionLocation, dimensionRange);
            transform.position = newPosition;
        }
        
        point = transform.position;
        Debug.DrawRay(point, Vector3.right * 0.5f, Color.blue);
        Debug.DrawRay(point, Vector3.up * 0.5f, Color.blue);

        // if (GetComponent<EnemyMovement>() != null)
        // {
        //     Debug.Log("This runs third");
        //     Debug.Log("in edge wrapping late update my ending positon is: " + transform.position);
        // }
        
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
        transform.position = newPosition;

        
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

        if(transform.position.x > boundWidthPos)
        {
            newPos.x = -boundWidthPos;
        }
        if(transform.position.x < -boundWidthPos)
        {
            newPos.x = boundWidthPos;
        }
        if(transform.position.y > boundHeightPos)
        {
            newPos.y = -boundHeightPos;
        }
        if(transform.position.y < -boundHeightPos)
        {
            newPos.y = boundHeightPos;
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
