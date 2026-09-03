using UnityEngine;

public class EdgeWrapping : MonoBehaviour
{
    [SerializeField] private float boundWidth;
    [SerializeField] private float boundHeight;
    private Vector3 topRightPoint;
    private Camera camera;
    private float camOffset;
    private float boundWidthPos;
    private float boundHeightPos;
    private GameObject visualMesh;
    private float cloneOffsetWidth;
    private float cloneOffsetHeight;

    private void FindBoundPos()
    {
        camera = Camera.main;
        camOffset = -camera.transform.position.z;

        topRightPoint = camera.ViewportToWorldPoint(
            new Vector3(
                boundWidth,
                boundHeight,
                camOffset
            ));

        boundWidthPos = topRightPoint.x;
        boundHeightPos = topRightPoint.y;
    }

    void Start()
    {
        FindBoundPos();
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
