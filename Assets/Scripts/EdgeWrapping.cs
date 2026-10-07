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
    int playerOnWhichSide;
    int thisObjOnWhichSide;
    float newPositionZ;

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
        playerOnWhichSide = 0;
        thisObjOnWhichSide = 0;
        float dimensionEdgeThresholdPercent = 0.5f;
        float dimensionEdgeThreshold = dimensionRange * dimensionEdgeThresholdPercent;

        if (dimensionLocation > dimensionEdgeThreshold)
        {
            playerOnWhichSide = 1;
        }
        if (transform.position.z > dimensionEdgeThreshold)
        {
            thisObjOnWhichSide = 1;
        }

        if (dimensionLocation < -dimensionEdgeThreshold)
        {
            playerOnWhichSide = -1;
        }
        if (transform.position.z < -dimensionEdgeThreshold)
        {
            thisObjOnWhichSide = -1;
        }

        if(playerOnWhichSide == 0 | thisObjOnWhichSide == 0)
        {
            return;

        } else if(playerOnWhichSide != thisObjOnWhichSide)
        {

            float difference = dimensionRange - Mathf.Abs(transform.position.z);
            newPositionZ = (dimensionRange + difference) * playerOnWhichSide;
            
            Vector3 newPosition = transform.position;
            newPosition.z = newPositionZ;
            transform.position = newPosition;
        }

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
