using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RayCatcher : MonoBehaviour
{
    [System.Serializable]
    public class Callback : UnityEvent<Ray, RaycastHit> { };
    public VRRaycaster.Callback raycastHitCallback;

    public void CatchRay(Ray ray, RaycastHit hit)
    {
        raycastHitCallback.Invoke(ray, hit);
    }
}
