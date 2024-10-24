using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ItemData itemToDrop;
    public int quantityPerHit;
    public int capacity;

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        for (int i = 0; i < quantityPerHit; i++)
        {
            if (capacity <= 0)
            {
                break;
            }

            capacity -= 1;

            Instantiate(itemToDrop.dropPrefab, hitPoint + Vector3.up * 0.2f, Quaternion.LookRotation(hitNormal, Vector3.up));
        }
    }
}
