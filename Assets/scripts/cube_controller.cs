using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class cube_controller : MonoBehaviour
{
    //private bool rotating = false;
    public float speedModifier;

    // vars for determining the rotation
    Quaternion startRotation;
    Quaternion endRotation;
    public float rotationProgress = -1;
    float extraRotation = 0; // Make 90 degree turns

    public List<textCollection> textCollections;

    public Transform textPivot;

    xml_reader reader;
     int _currentTextObj;
     int CurrentTextObj
    {
        get { return _currentTextObj; }
        set {
            Debug.Log(value);
                if (value > 3)
                {
                    
                    _currentTextObj = 0;
                }
                else if(value < 0)
                {
                Debug.Log("test");
                _currentTextObj = 3;
                }
                else
                {
                _currentTextObj = value;
                }
            }
    } // The current textobj the game is showing
     int currentQuestion = 0;

    bool textLoaded = false;
    



    // Start is called before the first frame update
    void Start()
    {
        reader = gameObject.GetComponent<xml_reader>();        
    }

    // Update is called once per frame
    void Update()
    {
        // Wait to show text untill it is loaded.
        if(textLoaded == false && reader.vragen.Count > 0)
        {
            ShowText(0);
            textLoaded = true;
        }

        // Check for LMB
        if (Input.GetButtonDown("Fire1") && rotationProgress < 0)
        {
            // Right side
            if (Input.mousePosition.x > Screen.width * 0.5f)
            {
                if (currentQuestion < reader.vragen.Count - 1)
                {
                    Debug.Log("rotate right");
                    extraRotation += 90;
                    CurrentTextObj += 1;
                    currentQuestion += 1;
                    RotateCube(extraRotation);
                }
            }
            else // Check for left side and if there are still questions to go back to;
            {
                if (currentQuestion >= 1)
                { 
                    Debug.Log("rotate left");
                    extraRotation -= 90;
                    CurrentTextObj -= 1;
                    currentQuestion -= 1;
                    RotateCube(extraRotation);
                }
            }            
        }

        // Rotation logic
        if (rotationProgress < 1 && rotationProgress >= 0)
        {
            rotationProgress += Time.deltaTime * speedModifier;

            transform.rotation = Quaternion.Lerp(startRotation, endRotation, rotationProgress);
            textPivot.rotation = Quaternion.Lerp(startRotation, endRotation, rotationProgress);
        }
        else if(rotationProgress > 1)
        {
            rotationProgress = -1;
        }
    }

    void RotateCube(float rot)
    {
        startRotation = gameObject.transform.rotation;
        endRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, rot, transform.rotation.eulerAngles.z);
        rotationProgress = 0;
        ShowText(CurrentTextObj);
    }

    void ShowText(int textNr)
    {
        textCollection textCollection = textCollections[textNr]; // The current Text-question-Object
        Vraag vraag = reader.vragen[currentQuestion];

        string answerText = "";

        for (int i=0; i < vraag.antwoorden.Count; i++)
        {
            answerText += string.Concat("\n", vraag.antwoorden[i].text);
        }

        Debug.Log(textCollections[0].vraag);
        //textAnswerObject.SetText(answerText);
    }
}
