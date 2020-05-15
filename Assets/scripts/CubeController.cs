using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// This script handles both rotation logic and showing text
public class CubeController : MonoBehaviour
{
    //private bool rotating = false;
    public float speedModifier;

    // vars for determining the rotation
    Quaternion startRotation;
    Quaternion endRotation;
    float rotationProgress = -1;
    float extraRotation = 0; // Make 90 degree turns

    public Transform textPivot;

    XMLReader reader;
    int _currentTextObj;
    int CurrentTextObj
    {
        get { return _currentTextObj; }
        set {
                if (value > 3)
                {
                    
                    _currentTextObj = 0;
                }
                else if(value < 0)
                {
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
    List<TextObject> textObjects;
    public List<UserAnswers> correct = new List<UserAnswers>();

    public Color goodColor;
    public Color wrongColor;
    public Color normalColor;

    Material material;

    // Start is called before the first frame update
    void Start()
    {
        reader = gameObject.GetComponent<XMLReader>();

        material = gameObject.GetComponent<Renderer>().material;
        material.color = normalColor;
        
        textObjects = GetTextObjects(textPivot);
    }

    // Update is called once per frame
    void Update()
    {
        // Wait to show text untill it is loaded.
        if(textLoaded == false && reader.vragen.Count > 0)
        {
            UpdateText(0);
            textLoaded = true;
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        Transform hitObj = hit.transform;

        if (Input.GetButtonDown("Fire1") && hitObj.transform.CompareTag("checkbox")) // First check to hit checkbox
        {
            GameObject checkmark = hitObj.GetChild(0).gameObject;
            checkmark.SetActive(!checkmark.activeSelf);
            Debug.Log(hitObj.name);
            correct[currentQuestion].correct[int.Parse(hitObj.name)] = checkmark.activeSelf;
        }        
        else if (Input.GetButtonDown("Fire1") && rotationProgress < 0) // Check for LMB
        {
            // Right side
            if (Input.mousePosition.x > Screen.width * 0.5f)
            {
                if (correctAnswer())
                {
                    material.color = goodColor;

                    if (currentQuestion < reader.vragen.Count - 1)
                    {
                        extraRotation += 90;
                        CurrentTextObj += 1;
                        currentQuestion += 1;
                        RotateCube(extraRotation);
                    }
                }
                else
                {
                    material.color = wrongColor;
                }
            }
            else // Check for left side and if there are still questions to go back to;
            {
                if (currentQuestion >= 1)
                { 
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
            material.color = normalColor;
        }
    }

    void RotateCube(float rot)
    {
        startRotation = gameObject.transform.rotation;
        endRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, rot, transform.rotation.eulerAngles.z);
        rotationProgress = 0;
        UpdateText(CurrentTextObj);
    }

    // Get current textObject, set the question and all the answers
    void UpdateText(int textNr)
    {
        TextObject textObject = textObjects[textNr]; // The current Text-question-Object
        Vraag vraag = reader.vragen[currentQuestion];
        textObject.vraag.SetText(vraag.vraag);
        textObject.teller.SetText((currentQuestion + 1) + " van de " + reader.vragen.Count);

        for (int i=0; i < vraag.antwoorden.Count; i++)
        {
            textObject.antwoorden[i].SetText(vraag.antwoorden[i].text);
        }     
        
        // All the logic for checking if the correctList is setup
        if(currentQuestion >= correct.Count) // make new list of bools
        {
            correct.Add(new UserAnswers(4));
            for (int i = 0; i < 4; i++)
            {
                textObject.checkMarks[i].SetActive(false);
            }
        }
        else // load in list of bools
        {
            for (int i = 0; i < 4; i++)
            {
                Debug.Log(textNr + " " + correct[currentQuestion].correct[i]);
                textObject.checkMarks[i].SetActive(correct[currentQuestion].correct[i]);
            }
        }
    }

    bool correctAnswer()
    {
        bool _correct = true;
        for (int i = 0; i < 4; i++)
        {
            if(reader.vragen[currentQuestion].antwoorden[i].correct != correct[currentQuestion].correct[i])
            {
                _correct = false;
            }
        }
        return _correct;
    }

    List<TextObject> GetTextObjects(Transform cubeEmpty) // Populate all the textObjects
    {
        List<TextObject> textObjects = new List<TextObject>();
        foreach (Transform textObject in cubeEmpty.transform) // Loop through all childeren of the pivot
        {
            TextMeshPro vraag            = textObject.GetChild(0).GetComponent<TextMeshPro>();
            List<TextMeshPro> antwoorden = new List<TextMeshPro>();
            List<GameObject> checkBoxes  = new List<GameObject>();
            List<GameObject> checkMarks  = new List<GameObject>();
            TextMeshPro teller             = textObject.GetChild(3).GetComponent<TextMeshPro>();

            Transform antw = textObject.GetChild(1);
            for (int i = 0; i < antw.childCount; ++i)
            {
                antwoorden.Add(antw.GetChild(i).GetComponent<TextMeshPro>());
            }

            Transform _checkboxes = textObject.GetChild(2);
            for (int i = 0; i < _checkboxes.childCount; ++i)
            {
                GameObject checkbox  = _checkboxes.GetChild(i).gameObject;
                GameObject checkmark = _checkboxes.GetChild(i).GetChild(0).gameObject;
                checkMarks.Add(checkmark);
                checkBoxes.Add(checkbox);
                checkmark.SetActive(false);
            }
            textObjects.Add(new TextObject(vraag, antwoorden, checkBoxes, checkMarks, teller));
        }
        return textObjects;
    }
    [Serializable]
    public class UserAnswers
    {
        public List<bool> correct;
        public UserAnswers(int amount)
        {
            correct = new List<bool>(amount);
            for (int i = 0; i < amount; i++)
            {
                correct.Add(false);
            }
        }
    }

    class TextObject
    {
        public TextMeshPro vraag;
        public List<TextMeshPro> antwoorden;
        public List<GameObject> checkboxes;
        public List<GameObject> checkMarks;
        public TextMeshPro teller;

        public TextObject(TextMeshPro _vraag, List<TextMeshPro> _antwoorden, List<GameObject> _checkboxes, List<GameObject> _checkMarks, TextMeshPro _teller)
        {
            vraag      = _vraag;
            antwoorden = _antwoorden;
            checkboxes = _checkboxes;
            checkMarks = _checkMarks;
            teller     = _teller;
        }
    }
}
