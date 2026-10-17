using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private GameObject player;
    private Vector3 playerPos;
    
    void Start()
    {
        player = FindAnyObjectByType<PlayerData>().gameObject;
        playerPos = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;
        setCameraPos(playerPos.x, playerPos.y);
    }

    void setCameraPos(float xPos, float yPos)
    {
        Vector3 newPos = transform.position;
        newPos.x = xPos;
        newPos.y = yPos;
        transform.position = newPos;
    }

    void changeCameraPos(float xPosDelta, float yPosDelta)
    {
        Vector3 newPos = transform.position;
        newPos.x += xPosDelta;
        newPos.y =+ yPosDelta;
        transform.position = newPos;
    }
}
