using UnityEngine;

public class BackgroundVisual : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private WorldPlane worldPlane;
    [SerializeField] private float saturationMulti;
    [SerializeField] private float valueMulti;
    Color color;


    // Update is called once per frame
    void Update()
    {
        color = worldPlane.GetComponent<ObjectLocationColor>().color;
        color = EditColor(color,saturationMulti,valueMulti);
        GetComponent<Renderer>().material.SetColor("_GridLines", color);
    }

    private Color EditColor(Color color, float saturationMulti, float valueMulti)
    {
        Color output;
        Color.RGBToHSV(color, out float h, out float s, out float v);
        s *= saturationMulti;
        v *= valueMulti;

        output = Color.HSVToRGB(h,s,v);
        return output;
    }

    
}
