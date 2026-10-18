using System;
using JetBrains.Annotations;
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


    //used for a fix camera to find camera borders
    // public float[] FindBoundPos(float boundWidth, float boundHeight)
    // {
    //     Camera camera = Camera.main;
    //     float camOffset = -camera.transform.position.z;

    //     Vector3 topRightPoint = camera.ViewportToWorldPoint(
    //         new Vector3(
    //             boundWidth,
    //             boundHeight,
    //             camOffset
    //         ));
    //     float outputWidth = topRightPoint.x;
    //     float outputHeight = topRightPoint.y;
    //     float[] outputArray = {outputWidth, outputHeight};
    //     return outputArray;
    // }


    void Start()
    {
        savedPosition = transform.position;
        worldPlane = FindAnyObjectByType<WorldPlane>();
        playerPosBeforeTeleport = transform.position;
    }
    
    //execution order is sooner than default so no need to worry 
    // about concurrency errors
    void Update()
    {
        transform.position = savedPosition;
    }
    void LateUpdate()
    {
        EdgeTeleport();

        savedPosition = transform.position;

        //red is real one blue is fake imposter evil clone
        Vector3 point = transform.position;
        Debug.DrawRay(point, Vector3.right * 0.5f, Color.red);
        Debug.DrawRay(point, Vector3.up * 0.5f, Color.red);

        if(GetComponent<PlayerData>() == null)
        {
            EdgeWrap();
            EdgeWrapOnAxisZ();
        }
        
        point = transform.position;
        Debug.DrawRay(point, Vector3.right * 0.5f, Color.blue);
        Debug.DrawRay(point, Vector3.up * 0.5f, Color.blue);

    }

    private void EdgeWrap()
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
    private void EdgeWrapOnAxisZ()
    {
        dimensionLocation = worldPlane.dimensionLocation;
        dimensionRange = worldPlane.dimensionRange;
        if (transform.position.z < 0)
        {
            thisObjOnWhichSide = 1;
        }
        if (transform.position.z > 0)
        {
            thisObjOnWhichSide = -1;
        }
        float distanceToEdge = dimensionRange - Mathf.Abs(transform.position.z);
        float newPositionZ = (dimensionRange + distanceToEdge) * thisObjOnWhichSide;
        float difference = Mathf.Abs(transform.position.z - dimensionLocation);
        float newDifference = Mathf.Abs(newPositionZ - dimensionLocation);
        if(newDifference < difference)
        {
            Vector3 newPosition = transform.position;
            newPosition.z = newPositionZ;
            transform.position = newPosition;
        }
    }

    public static event Action OnPlayerTeleport;

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
