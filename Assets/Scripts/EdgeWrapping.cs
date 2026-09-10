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

    public void FindBoundPos(float boundWidthPos, float boundHeightPos, float boundWidth, float boundHeight)
    {
        camera = Camera.main;
        camOffset = -camera.transform.position.z;

        topRightPoint = camera.ViewportToWorldPoint(
            new Vector3(
                this.boundWidth,
                this.boundHeight,
                camOffset
            ));

        this.boundWidthPos = topRightPoint.x;
        this.boundHeightPos = topRightPoint.y;
    }

    void Start()
    {
        FindBoundPos(boundWidthPos, boundHeightPos, boundWidth, boundHeight);
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
