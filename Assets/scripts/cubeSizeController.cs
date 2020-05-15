using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSizeController : MonoBehaviour
{

    float start_height = 0;
    float start_width = 0;

    float height;
    float width;

    public List<GameObject> textObjects; //Front, Right, Back, Left
    public List<Vector3> facePositions = new List<Vector3>(4);

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        height = Camera.main.orthographicSize * 2;
        width = height * Screen.width / Screen.height; // basically height * screen aspect ratio        

        if (start_height != height || start_width != width)
        {
            start_height = height;
            start_width = width;
            gameObject.transform.localScale = new Vector3(width, height, width);

            facePositions[0] = transform.position + Vector3.back * width/2;
            facePositions[1] = transform.position + Vector3.right * width / 2;
            facePositions[2] = transform.position + Vector3.forward * width / 2;
            facePositions[3] = transform.position + Vector3.left * width / 2; ;


            for (int i = 0; i < 4; i++)
            {
                textObjects[i].transform.position = facePositions[i];
            }
        }
    }
}
