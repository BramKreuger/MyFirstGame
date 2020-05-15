﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;
using UnityEngine.Networking;

public class xml_reader : MonoBehaviour
{
    public List<Vraag> vragen;
    void Start()
    {        
        StartCoroutine(GetText());
    }



    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://www.bramkreuger.com/game/vragen.xml");
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            vragen = VragenContainer.LoadFromText (www.downloadHandler.text).vragen;
        }
    }
}

[System.Serializable]
public class Vraag
{
    [XmlAttribute("vraag")]
    public string vraag;
    [XmlArray("antwoorden")][XmlArrayItem("antwoord")]
    public List<Antwoord> antwoorden;
}

[System.Serializable]
public class Antwoord
{
    [XmlElement("text")]
    public string text;

    [XmlElement("correct")]
    public bool correct;
}

[XmlRoot("vraagCollectie")]
public class VragenContainer
{
    [XmlArray("vragen")]
    [XmlArrayItem("vraag_en_antwoord")]
    public List<Vraag> vragen = new List<Vraag>();

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(VragenContainer));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static VragenContainer Load(string path)
    {
        var serializer = new XmlSerializer(typeof(VragenContainer));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as VragenContainer;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static VragenContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(VragenContainer));
        return serializer.Deserialize(new StringReader(text)) as VragenContainer;
    }
}
