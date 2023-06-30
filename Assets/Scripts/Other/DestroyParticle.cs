using System.Collections;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    [SerializeField] PoolType poolType;
    [SerializeField] float destroyTime = 3;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(destroyTime);

        PoolingManager.Manager.AddIntoPool(gameObject, poolType);
    }
}