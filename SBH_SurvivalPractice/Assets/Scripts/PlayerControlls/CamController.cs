using Palmmedia.ReportGenerator.Core.CodeAnalysis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public Camera cam;
    public LayerMask groundLayerMask;

    private float minCamZ = -2;
    private float maxCamZ = 0;

    private void Start()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        float camZ;

        Ray ray = new Ray(transform.position, -transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2f, groundLayerMask))
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
