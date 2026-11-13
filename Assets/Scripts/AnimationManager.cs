using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] private GameObject explolion;
    [SerializeField] private float explosionWaitTime;

    public void SpawnExplolion(Vector3 position)
    {
        GameObject explosionObj = Instantiate(explolion, position, Quaternion.identity);
        StartCoroutine(WaitThenDestroy(explosionWaitTime,explosionObj));
    }

    IEnumerator WaitThenDestroy(float waitTime, GameObject targetGameObject)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(targetGameObject);
    }
}
