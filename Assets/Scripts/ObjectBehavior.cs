using UnityEngine;

public class ObjectBehavior : MonoBehaviour
{
    
    private GameObject gameManager;
    public float objectWidth;
    public float objectHitBoxWidth;
    public float sizeChangeMultiplier;
    public float sizeChangeOffset;
    private GameObject worldPlaneGameObject;
    private GameObject visualMesh;
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>().gameObject;
        worldPlaneGameObject = gameManager.GetComponent<GameManager>().worldPlane;
        visualMesh = transform.GetChild(0).gameObject;

        BoxCollider collider = GetComponent<BoxCollider>();
        collider.size = new Vector3(
            collider.size.x,
            collider.size.y,
            objectHitBoxWidth
        );

    }

    void Update()
    {
        displayObjectOnPlane();
    }

    void displayObjectOnPlane()
    {
        Vector3 worldPlaneNormal = worldPlaneGameObject.transform.forward;
        Plane worldPlaneObject = new Plane(
            worldPlaneNormal,
            worldPlaneGameObject.transform.position
        );
        
        float distanceToPlane = worldPlaneObject.GetDistanceToPoint(transform.position);
        distanceToPlane = Mathf.Abs(distanceToPlane);
        
        if(distanceToPlane != 0)
        {
            visualMesh.transform.localScale =  Vector3.one / (distanceToPlane + 1);
            visualMesh.transform.localScale *= sizeChangeMultiplier;
            visualMesh.transform.localScale += Vector3.one * sizeChangeOffset;
        }
        else
        {
            visualMesh.transform.localScale =  Vector3.one;
            visualMesh.transform.localScale *= sizeChangeMultiplier;
            visualMesh.transform.localScale += Vector3.one * sizeChangeOffset;
        }

        visualMesh.transform.position = transform.position;
        visualMesh.transform.localScale = new Vector3(
            visualMesh.transform.localScale.x,
            visualMesh.transform.localScale.y,
            1
        );
        if(visualMesh.transform.localScale.x <= 0)
        {
            visualMesh.transform.localScale = Vector3.zero;
        }

    }
}
