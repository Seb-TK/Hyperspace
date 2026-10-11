using System;
using UnityEngine;

public class EdgeWrapping : MonoBehaviour
{
    [SerializeField] private float boundWidth;
    [SerializeField] private float boundHeight;
    private float boundWidthPos;
    private float boundHeightPos;
    private GameObject visualMesh;
    private Vector3 savedPosition;
    private WorldPlane worldPlane;
    private float dimensionLocation;
    private float dimensionRange;
    int thisObjOnWhichSide;

    public float[] FindBoundPos(float boundWidth, float boundHeight)
    {
        Camera camera = Camera.main;
        float camOffset = -camera.transform.position.z;

        Vector3 topRightPoint = camera.ViewportToWorldPoint(
            new Vector3(
                boundWidth,
                boundHeight,
                camOffset
            ));
        float outputWidth = topRightPoint.x;
        float outputHeight = topRightPoint.y;
        float[] outputArray = {outputWidth, outputHeight};
        return outputArray;
    }

    void Start()
    {
        float[] boundArray = FindBoundPos(boundWidth, boundHeight);
        boundWidthPos = boundArray[0];
        boundHeightPos = boundArray[1];
        savedPosition = transform.position;
        worldPlane = FindAnyObjectByType<WorldPlane>();
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
        if(GetComponent<PlayerData>() == null)
        {
            AppearOnOppositeDimensionEdge();
        }
    }

    private void AppearOnOppositeDimensionEdge()
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
        //first you find the distance to the edge by subtracting the current distance
        // from the total range (halfed technically, the maxed range)
        //distance to edge
        //then you find your new position
        //you find the total distance and add the distance to the edge
        //this finds how far out you'd be from the edge
        //then you multiply it by the side that the object would be on
        // the idea is that you're seeing how far away the object is from the edge
        //then putting it that distance outside of the opposite edge
        //since that'd be where it would be
        //now you find the difference between the distance between the object
        //and the player as well as the new location and the player
        // then you just see if the new difference is smaller which would mean that the
        //new location is closer so you'd switch to it
        //weirdly though, the new distance is always the same distance?
        //???


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



        Debug.Log("new difference "+newDifference);
        Debug.Log("difference "+difference);
        Debug.Log("z pos "+transform.position.z);
        Debug.Log("new pos "+newPositionZ);
        Debug.Log("dimensionLocation "+dimensionLocation);
        Debug.Log("");
        Debug.Log("");
    }

    private void CloneOnEdge()
    {
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

        transform.position = newPos;
    }

    private void EdgeTeleport()
    {
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

        transform.position = newPos;
    }
}
