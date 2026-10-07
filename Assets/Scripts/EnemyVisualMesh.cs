using UnityEngine;

public class EnemyVisualMesh : MonoBehaviour
{
    public float SizeMultiplier;
    [SerializeField] private float fixedSizeMultiplier;
    

    // Update is called once per frame
    void LateUpdate()
    {
        transform.localScale = transform.localScale * SizeMultiplier * fixedSizeMultiplier;
    }
}
