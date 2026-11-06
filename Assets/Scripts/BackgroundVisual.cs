using System;
using UnityEngine;

public class BackgroundVisual : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private WorldPlane worldPlane;
    [SerializeField] private float saturationMulti;
    [SerializeField] private float valueMulti;
    [SerializeField] private ColorChoices BackgroundColorChoice;
    [SerializeField] private ColorChoices LineColorChoice;
    Color color;
    private string[] colors;

    void Start()
    {
        colors = new string[] {"BLACK","SOLID_COLOR","GRADIENT"};
        mat = GetComponent<Renderer>().material;
    }
    // Update is called once per frame
    void Update()
    {
        UpdateColor();
        UpdateBackgroundChoice("_BACKGROUND_COLOR", (int)BackgroundColorChoice);
        UpdateBackgroundChoice("_LINE_COLOR", (int)LineColorChoice);
    }

    private enum ColorChoices
    {
        Black = 0,
        SolidColor = 1,
        Gradient = 2
    }
    private void UpdateBackgroundChoice(string enumName, int choice)
    {
        for(int i = 0; i < System.Enum.GetValues(typeof(ColorChoices)).Length; i++)
        {
            mat.DisableKeyword(enumName + "_" + colors[i]);
        }
        mat.EnableKeyword(enumName + "_" + colors[choice]);
    }

    private void UpdateColor()
    {
        color = worldPlane.GetComponent<ObjectLocationColor>().color;
        color = EditColor(color,saturationMulti,valueMulti);
        mat.SetColor("_GridLines", color);
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
