using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// During production most of this data should come from the CMS, but for now it is set as presets
/// in the inspector.
/// </summary>
public class sceneController : MonoBehaviour
{
    public XMLReader reader;

    public GameObject openCube;

    private RayCatcher openCubeRayCatcher;

    private OpenCube openCubeScript;

    public GameObject cubePrefab;

    public GameObject cubeHolder;

    private GameObject cube;

    public CubeController controller;

    public List<Hotspot> hotspots;

    private Hotspot newHotspot;

    private Hotspot previousHotspot;

    private void Start()
    {
        openCubeRayCatcher = openCube.GetComponent<RayCatcher>();        
        LoadScene("museumplein");
    }

    public void LoadScene(string name)
    {
        if(previousHotspot != null)
        {
            foreach (GameObject arrow in previousHotspot.movementArrows)
            {
                arrow.SetActive(false);
            }
        }

        if(cube != null)
        {
            cubeHolder.SetActive(false);
            Destroy(cube);
        }

        newHotspot = hotspots.Find(item => item.name == name); // Grab the new scene from the list
        
        if (newHotspot.number != -1)
        {
            cube = Instantiate(cubePrefab) as GameObject;
            cube.transform.parent = cubeHolder.transform;
            controller = cube.GetComponentInChildren<CubeController>();
            StartCoroutine(reader.GetText(newHotspot.number));
            openCube.SetActive(true);
        }
        else
            openCube.SetActive(false);

        RenderSettings.skybox = newHotspot.material;

        foreach (GameObject arrow in newHotspot.movementArrows)
        {
            arrow.SetActive(true);
        }

        previousHotspot = newHotspot;
    }
}

[Serializable]
public class Hotspot
{
    public string name;

    [Tooltip("The material of the hotspot")]
    public Material material;

    [Tooltip("The number of the hotspot and the corresponding .xml file")]
    public int number;

    public List<GameObject> movementArrows;

}
