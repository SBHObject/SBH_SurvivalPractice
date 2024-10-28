using Palmmedia.ReportGenerator.Core.CodeAnalysis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public Camera cam;
    public LayerMask groundLayerMask;

    private float minCamZ;
    private float maxCamZ;

    private void Start()
    {
        cam = Camera.main;
        minCamZ = transform.localPosition.z;
        maxCamZ = cam.transform.localPosition.z;
    }

    private void FixedUpdate()
    {
        float camZ;

        Ray ray = new Ray(transform.position, -transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5f, groundLayerMask))
        {
            camZ = Vector3.Distance(hit.point, transform.position) * -1;
        }
        else
        {
            camZ = minCamZ;
        }

        camZ = Mathf.Clamp(camZ, minCamZ, maxCamZ);

        cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, camZ);
    }
}
