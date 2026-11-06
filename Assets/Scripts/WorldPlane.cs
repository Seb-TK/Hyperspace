using UnityEngine;

public class WorldPlane : MonoBehaviour
{
    [SerializeField] private float dimensionMoveSpeedButtons;
    [SerializeField] private float dimensionMoveSpeedScroll;
    public float dimensionRange;
    public float dimensionLocation;
    public float boundWidth;
    public float boundHeight;
    void Awake()
    {
        boundWidth = transform.localScale.x / 2f;
        boundHeight = transform.localScale.y / 2f;
    }
    void Update()
    {
        // moveUsingButtons();
        MoveUsingScroll();
        WrapOnAxisZ();
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            dimensionLocation
        );
    }

    void WrapOnAxisZ()
    {
        float newLocation = Mathf.Repeat(dimensionLocation + dimensionRange, dimensionRange + dimensionRange);
        newLocation -= dimensionRange;
        dimensionLocation = newLocation;
    }

    void MoveUsingScroll()
    {
        dimensionLocation -= Input.mouseScrollDelta.y * dimensionMoveSpeedScroll * Time.deltaTime;
    }

    void MoveUsingButtons()
    {
      bool isPressing = false;
        if (Input.GetKey(KeyCode.Q))
        {
            isPressing = true;
            dimensionLocation += dimensionMoveSpeedButtons * Time.deltaTime;
        } 
        if (Input.GetKey(KeyCode.E))
        {
            isPressing = true;
            dimensionLocation -= dimensionMoveSpeedButtons * Time.deltaTime;
        }
        if (isPressing == false)
        {
            dimensionLocation += Input.mouseScrollDelta.y * dimensionMoveSpeedButtons * Time.deltaTime;
        }
    }
        
}
