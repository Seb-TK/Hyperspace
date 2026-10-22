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

        WorldPlane worldPlane = FindAnyObjectByType<WorldPlane>();
        Vector3 newPos = transform.position;
        newPos.z = worldPlane.transform.position.z;
        transform.position = newPos;
    }

    void Movement()
    {
        InputVector = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"),
            0
        ).normalized;
        transform.position += InputVector * MovementSpeed * Time.deltaTime;
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
