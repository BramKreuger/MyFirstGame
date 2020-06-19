using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movePano : MonoBehaviour
{
    public string nextHotspot;
    private sceneController sc;

    private void Start()
    {
        sc = FindObjectOfType<sceneController>();
    }

    public void MoveToNextHotspot()
    {
        sc.LoadScene(nextHotspot);
    }
}

