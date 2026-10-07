using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectLocationColor : MonoBehaviour
{
    private Renderer renderer;
    private float dimensionLocation;
    private WorldPlane worldPlane;
    [SerializeField]private float colorMultiplier = 1f;
    [SerializeField]private float colorOffset = 0f;
    [SerializeField]private float saturation = 1f;
    [SerializeField]private float value = 1f;
    void Awake()
    {
        worldPlane = FindAnyObjectByType<WorldPlane>();
        renderer = GetComponent<Renderer>();

        colorMultiplier = worldPlane.dimensionRange;
        colorOffset = 0f;
        saturation = 0.8f;
        value = 1.5f;
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
        renderer.material.color = Color.HSVToRGB(targetColorValue, saturation, value);        
    }

}
