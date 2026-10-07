using UnityEngine;

public class WorldPlane : MonoBehaviour
{
    [SerializeField] private float dimensionMoveSpeed;
    public float dimensionRange;
    public float dimensionLocation;

    void Update()
    {
        bool isPressing = false;
        if (Input.GetKey(KeyCode.Q))
        {
            isPressing = true;
            dimensionLocation += dimensionMoveSpeed * Time.deltaTime;
        } 
        if (Input.GetKey(KeyCode.E))
        {
            isPressing = true;
            dimensionLocation -= dimensionMoveSpeed * Time.deltaTime;
        }
        if (isPressing == false)
        {
            dimensionLocation += Input.mouseScrollDelta.y * dimensionMoveSpeed * Time.deltaTime;
        }

        if(dimensionLocation > dimensionRange)
        {
            dimensionLocation = -dimensionRange;
        }
        if(dimensionLocation < -dimensionRange)
        {
            dimensionLocation = dimensionRange;
        }

        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            dimensionLocation
        );
    }
        
}
