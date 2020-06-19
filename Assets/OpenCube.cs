using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Open / Close the question cube
/// </summary>
public class OpenCube : MonoBehaviour
{
    public sceneController sceneController;
    public void OpenCloseCube(GameObject cube)
    {
        cube.SetActive(!cube.activeSelf);
    }
}
