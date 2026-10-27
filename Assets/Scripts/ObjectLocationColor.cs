using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectLocationColor : MonoBehaviour
{
    private Renderer renderer;
    private float dimensionLocation;
    private WorldPlane worldPlane;
    private ObjectBehavior objectBehavior;
    private float collisionDistance;
    [SerializeField] private float colorMultiplier = 1f;
    [SerializeField] private float colorOffset = 0f;
    [SerializeField] private float saturation = 1f;
    [SerializeField] private float value = 1f;
    [SerializeField] private float valueCollisionMultiplier;
    [SerializeField] private bool isBlack;
    void Start()
    {
        renderer = GetComponent<Renderer>();
        if(isBlack)
        {
            renderer.material.color = Color.HSVToRGB(0,0,0);
            GetComponent<ObjectLocationColor>().enabled = false;
            return;
        }
        worldPlane = FindAnyObjectByType<WorldPlane>();
        objectBehavior = FindAnyObjectByType<ObjectBehavior>();
        collisionDistance = ObjectBehavior.CollisionDistance;
        colorMultiplier = 1;
        colorOffset = 0f;
        saturation = 0.8f;
        value = 1.5f;
        valueCollisionMultiplier = 0.5f;
        SetColor();
    }

    void Update()
    {
        SetColor();
    }

    void SetColor()
    {
        dimensionLocation = transform.position.z;
        float dimensionRange = worldPlane.dimensionRange;
        float trueDimensionLocation = dimensionLocation + dimensionRange;
        float trueDimensionRange = dimensionRange * 2;
        float dimensionLocationPercent = trueDimensionLocation / trueDimensionRange;    
        float targetColorValue = (dimensionLocationPercent * colorMultiplier) + colorOffset;
        targetColorValue = targetColorValue %1f;
        float tempValue = value;
        if(Mathf.Abs(transform.position.z - dimensionLocation) > collisionDistance)
        {
            tempValue *= valueCollisionMultiplier;
        }
        if(GetComponent<EnemyData>() != null)
        {
            Debug.Log("Mathf.Abs(transform.position.z - dimensionLocation): " + Mathf.Abs(transform.position.z - dimensionLocation));
            Debug.Log("transform.position.z " + transform.position.z);
            Debug.Log("dimensionLocation " + dimensionLocation);
        }
        renderer.material.color = Color.HSVToRGB(targetColorValue, saturation, tempValue);        

    }

}
