using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public class XMLWriter {

    private const string SAVE_FILE_NAME = "/Perceptron_Weights.xml";

    private Perceptron m_Perceptron;

    public XMLWriter(Perceptron perceptron)
    {
        m_Perceptron = perceptron;
        LoadData();
    }
    private void LoadData()
    {
        if (CheckForExistingFile())
        {
            ReadXML();
        }
    }

    private bool CheckForExistingFile()
    {
        if (File.Exists(Application.persistentDataPath + SAVE_FILE_NAME))
            return true;
        else
            return false;
    }

    private void ReadXML()
    {
        XmlReaderSettings xmlReaderSettings = new XmlReaderSettings
        {
            IgnoreWhitespace = true
        };
        StreamReader streamReader = new StreamReader(Application.persistentDataPath + SAVE_FILE_NAME);

        XmlTextReader xmlReader = new XmlTextReader(streamReader);

        while (xmlReader.Read())
        {
            DecideOnXml(xmlReader);
        }
    }

    private void DecideOnXml(XmlReader xmlReader)
    {
        if (xmlReader.IsStartElement())
            SwitchOnElement(xmlReader);
            
    }

    private void SwitchOnElement(XmlReader xmlReader)
    {
        switch (xmlReader.Name)
        {
            case "Weight0":
                m_Perceptron.SetWeight(0, GetValue(xmlReader));
                break;

            case "Weight1":
                m_Perceptron.SetWeight(1, GetValue(xmlReader));
                break;

            case "Weight2":
                m_Perceptron.SetWeight(2, GetValue(xmlReader));
                break;
        }
    }

    private float GetValue(XmlReader xmlReader)
    {
        xmlReader.Read();
        return float.Parse(xmlReader.Value);
    }




    public void StartSaving()
    {
        string path = Application.persistentDataPath + SAVE_FILE_NAME;

        StreamWriter streamWriter = new StreamWriter(path);
        XmlTextWriter xmlTextWtiter = new XmlTextWriter(streamWriter)
        {
            Formatting = Formatting.Indented
        };

        WriteToFile(xmlTextWtiter);
    }

    private void WriteToFile(XmlTextWriter xmlTextWriter)
    {
        xmlTextWriter.WriteStartDocument();

        xmlTextWriter.WriteStartElement("WeightsData");

        xmlTextWriter.WriteElementString("Weight0", m_Perceptron.Weights[0].ToString());
        xmlTextWriter.WriteElementString("Weight1", m_Perceptron.Weights[1].ToString());
        xmlTextWriter.WriteElementString("Weight2", m_Perceptron.Weights[2].ToString());

        xmlTextWriter.WriteEndElement();

        xmlTextWriter.WriteEndDocument();

        xmlTextWriter.Close();
    }
}
