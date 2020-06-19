using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Control the camera if not in VR-mode
/// </summary>
public class editorController : MonoBehaviour
{
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        if (Application.isEditor)
        {
            LineRenderer lr = gameObject.GetComponent<LineRenderer>();
            if (lr != null)
            {
                lr.enabled = true;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor)
        {
            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    RayCatcher rayCatcher = hit.transform.GetComponent<RayCatcher>();
                    if (rayCatcher != null)
                    {
                        rayCatcher.CatchRay(ray, hit);
                    }
                }
            }
        }
    }

    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width / 2, Screen.height / 2, 10, 10), "");
    }
}
