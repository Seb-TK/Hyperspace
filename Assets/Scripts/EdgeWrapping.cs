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
            EdgeWrap();
            EdgeWrapOnAxisZ();
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

    private void EdgeWrap()
    {   


        GameObject player = FindAnyObjectByType<PlayerData>().gameObject;
        Vector3 playerTransform = player.transform.position;
        float mapWidth = worldPlane.transform.localScale.x;
        float mapHeight = worldPlane.transform.localScale.y;


        //find distance between current
        //compare to distance of flipped version
        //what ever is closest pick?

        
        
        float distanceToPlayer = transform.position.x - playerTransform.x;
        float[] newPositions = new float[2];
        float mapLength = mapWidth;
        if(distanceToPlayer >= mapLength / 2)
        {
            
            float newPosX = transform.position.x;
            float newPosY = transform.position.y;

            //switch to flipped
            if(transform.position.x > 0)
            {
                //means that you need to subtract the difference
                //between the location of object
                // and positive edge
                //from negative edge
                float distanceToEdgeX = (mapLength / 2) - transform.position.x;
                newPosX = (-mapLength / 2) - distanceToEdgeX;
            }
            else
            {
                //means you need to add 
                // the difference between
                //bottom width and position
                //to the top edge
                float distanceToEdgeX = (mapWidth / 2) + transform.position.x;
                newPosX = mapWidth / 2 + Mathf.Abs(distanceToEdgeX);
            }
            transform.position = new Vector3(newPosX, newPosY, transform.position.z);
        }




        // Vector3 distance = transform.position - playerPos;
        // distance.x = Mathf.Repeat(distance.x + mapWidth / 2f, mapWidth);
        // distance.y = Mathf.Repeat(distance.y + mapHeight / 2f, mapHeight);
        // distance.x -= mapWidth / 2f;
        // distance.y -= mapHeight / 2f;
        // distance += playerPos;
        
        // Vector3 newPosition = transform.position;
        // newPosition.x = distance.x;
        // newPosition.y = distance.y;
        // transform.position = newPosition;

        
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
