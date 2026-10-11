using System.Security.Cryptography;
using UnityEditor.ShaderGraph.Internal;
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

    public static float CollisionDistance = 0.3f;
    void Start()
    {

        gameManager = FindAnyObjectByType<GameManager>().gameObject;
        worldPlaneGameObject = gameManager.GetComponent<GameManager>().worldPlane;
        visualMesh = transform.GetChild(0).gameObject;

        objectHitBoxWidth = 5;
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.size = new Vector3(
            collider.size.x,
            collider.size.y,
            objectHitBoxWidth
        );
        displayObjectOnPlane();
    }

    void LateUpdate()
    {
        displayObjectOnPlane();
    }

    public bool collision(float ZPositionOne, float ZPositionTwo)
    {
        if(Mathf.Abs(ZPositionOne - ZPositionTwo) <= CollisionDistance)
        {
            return true;
        } else
        {
            return false;
        }

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
        
        Vector3 visualMeshScale = visualMesh.transform.localScale;

        if(distanceToPlane != 0)
        {
            visualMeshScale =  Vector3.one / (distanceToPlane + 1);
            visualMeshScale *= sizeChangeMultiplier;
            visualMeshScale += Vector3.one * sizeChangeOffset;
        }
        else
        {
            visualMeshScale =  Vector3.one;
            visualMeshScale *= sizeChangeMultiplier;
            visualMeshScale += Vector3.one * sizeChangeOffset;
        }

        visualMesh.transform.position = transform.position;
        visualMeshScale = new Vector3(
            visualMeshScale.x,
            visualMeshScale.y,
            1
        );
        if(visualMeshScale.x <= 0)
        {
            visualMeshScale = Vector3.zero;
        }
        visualMesh.transform.localScale = visualMeshScale;
    }
}
