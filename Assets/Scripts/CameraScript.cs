using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraScript : MonoBehaviour
{
    private GameObject player;
    private Vector3 playerPos;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private float zOffSet;
    
    void Start()
    {
        player = FindAnyObjectByType<PlayerData>().gameObject;
        playerPos = player.transform.position;
        EdgeWrapping.OnPlayerTeleport += playerTeleported;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;
        lerpToPlayer();
        setCameraPos(transform.position.x, transform.position.y, zOffSet);
    }

    void playerTeleported()
    {
        Vector3 beforePos = player.GetComponent<EdgeWrapping>().playerPosBeforeTeleport;
        Vector3 difference = player.transform.position - beforePos;
        transform.position += difference;
    }

    void lerpToPlayer()
    {
        Vector3 currentPos = transform.position;
        float distance = Vector3.Distance(currentPos, playerPos);
        transform.position = Vector3.Lerp(currentPos, playerPos, distance * lerpSpeed * Time.deltaTime);
    }
    void setCameraPos(float xPos, float yPos, float zPos)
    {
        Vector3 newPos = transform.position;
        newPos.x = xPos;
        newPos.y = yPos;
        newPos.z = zPos;
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
