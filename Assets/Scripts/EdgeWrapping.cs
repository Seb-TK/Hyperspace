using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

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
    private int thisObjOnWhichSide;
    public bool ignoreEdgeWrap = false;


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
    }
    
    //execution order is sooner than default so no need to worry 
    // about concurrency errors
    void Update()
    {
        transform.position = savedPosition;
    }
    void LateUpdate()
    {
        if (!ignoreEdgeWrap)
        {
            EdgeTeleport();
        }
        savedPosition = transform.position;
        if(GetComponent<PlayerData>() == null)
        {
            EdgeWrap();
            EdgeWrapOnAxisZ();
        }
    }

    private void EdgeWrap()
    {
        GameObject player = FindAnyObjectByType<PlayerData>().gameObject;
        float playerPosX = player.transform.position.x;
        float playerPosY = player.transform.position.y;
        float mapWidthX = worldPlane.transform.localScale.x;
        float mapWidthY = worldPlane.transform.localScale.y;
        float mapRadiusX = mapWidthX / 2f;
        float mapRadiusY = mapWidthY / 2f;
        float distanceX;
        float distanceY;

        distanceX = this.transform.position.x - playerPosX;
        distanceY = this.transform.position.y - playerPosY;
        distanceX = distanceX - mapRadiusX;
        distanceY = distanceY - mapRadiusY;
        distanceX = Mathf.Repeat(distanceX, mapRadiusX);
        distanceY = Mathf.Repeat(distanceY, mapRadiusY);
        distanceX = distanceX + playerPosX;
        distanceY = distanceY + playerPosY;

        Vector3 newPosition = transform.position;
        newPosition.x = distanceX;
        newPosition.y = distanceY;
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
