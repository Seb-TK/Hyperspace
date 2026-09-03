using System.Drawing;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float MovementSpeed;
    private Vector3 InputVector;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashDistance;
    [SerializeField] private float screenWidth;
    [SerializeField] private float screenHeight;
    private float dashTimer;

    void Update()
    {
        Movement();
        DashInput();
    }

    void Movement()
    {
        InputVector = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"),
            0
        ).normalized;
        transform.position += InputVector * MovementSpeed * Time.deltaTime;
        
        Camera camera = Camera.main;
        float camOffset = -camera.transform.position.z;
        Vector3 rightEdge = camera.ViewportToWorldPoint(new Vector3(screenWidth,0,camOffset));
        Vector3 topEdge = camera.ViewportToWorldPoint(new Vector3(0,screenHeight,camOffset));
        float width = rightEdge.x;
        float height = topEdge.y;
        float newX = Mathf.Clamp(transform.position.x, -width, width);
        float newY = Mathf.Clamp(transform.position.y, -height, height);
        transform.position = new Vector3(newX, newY, transform.position.z);
    }
    void DashInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time > dashTimer)
            {
                Dash();
                dashTimer = Time.time + dashCooldown;
            }
        }
    }

    void Dash()
    {
        transform.position += InputVector * dashDistance;
    }

}
