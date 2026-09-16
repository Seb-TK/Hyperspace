using UnityEngine;

public class SpawnCircle : MonoBehaviour
{
    [SerializeField] private float maxSize;
    [SerializeField] private float sizeIncreaseSpeed;
    [SerializeField] private float opacity;
    public GameObject Enemy;
    private float size;

    void Awake()
    {
        size = 0.01f;
        transform.localScale = Vector3.one * size;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color color = renderer.material.color;
        color.a = opacity;
    }
    void Update()
    {
        if (size <= maxSize)
        {
            size += sizeIncreaseSpeed * Time.deltaTime;
            transform.localScale = Vector3.one * size;
        }
        else
        {
            Enemy.SetActive(true);
            Destroy(gameObject);
        }
    }
}
