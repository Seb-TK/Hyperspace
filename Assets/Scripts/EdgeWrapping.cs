using System;
using UnityEngine;

public class EdgeWrapping : MonoBehaviour
{
    [SerializeField] private float boundWidth;
    [SerializeField] private float boundHeight;
    private float boundWidthPos;
    private float boundHeightPos;
    private GameObject visualMesh;
    private float cloneOffsetWidth;
    private float cloneOffsetHeight;

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
    }

    void LateUpdate()
    {

        EdgeTeleport();
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
