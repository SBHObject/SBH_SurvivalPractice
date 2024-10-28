using Palmmedia.ReportGenerator.Core.CodeAnalysis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public Camera cam;
    public LayerMask groundLayerMask;

    private float minCamZ = -5f;
    private float maxCamZ = -2.5f;
    private float camDistance;

    private void Start()
    {
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        float camZ;

        Ray ray = new Ray(transform.position, -transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5f, groundLayerMask))
        {
            Debug.Log("Hit");
            camDistance = Vector3.Distance(hit.point, transform.position);
        }
        else
        {
            camDistance = minCamZ;
        }

        camZ = Mathf.Clamp(camDistance, minCamZ, maxCamZ);

        cam.transform.localPosition = new Vector3(0, 0, camZ);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, cam.transform.position);
    }
}
