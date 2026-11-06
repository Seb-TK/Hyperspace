using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyOffScreenArrow : MonoBehaviour
{
    [SerializeField] private float xClampMultiplier;
    [SerializeField] private float yClampMultiplier;
    public GameObject Enemy;
    public WorldPlane worldPlane;

    void Start()
    {
        worldPlane = FindAnyObjectByType<WorldPlane>();
    }

    void Update()
    {
        // Vector3 newPos = Camera.main.transform.position;
        // newPos.z += 5f;
        // transform.position = newPos;
        // Vector3 newDir = Enemy.transform.position - transform.position;
        // newDir.z = 0f;
        // transform.up = newDir;
        // newPos = transform.up * 5;
        // transform.position = newPos;
        // newPos.x = Mathf.Clamp(newPos.x,-worldPlane.boundWidth * xClampMultiplier,worldPlane.boundWidth * xClampMultiplier);
        // newPos.y = Mathf.Clamp(newPos.y,-worldPlane.boundHeight * yClampMultiplier,worldPlane.boundHeight * yClampMultiplier);
        // transform.position = newPos;
        // Debug.Log(newPos);
    }
}
