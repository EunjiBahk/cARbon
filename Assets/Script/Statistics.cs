using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using System;

public class Statistics : MonoBehaviour
{

    public GameObject StatisticsPanel;
    public GameObject Background;
    public GameObject LearnButton;
    public GameObject QuizButton;
    public GameObject StatisticsButton;

    public TMP_Text quizAttempsText;
    public TMP_Text lastScoreText;

    // Name
    // public TMP_Text nameText;
    // public TMP_InputField inputName;

    public void OpenSetting()
    {
        string path = Application.persistentDataPath + "/player.carbon";
        BinaryFormatter formatter = new BinaryFormatter();        

        if(File.Exists(path)){
            FileStream stream = new FileStream(path, FileMode.Open);
            Userdata data = formatter.Deserialize(stream) as Userdata;
            stream.Close();
           
            quizAttempsText.text = data.attempt.ToString();
            lastScoreText.text = data.score.ToString()+"/5";
            //nameText.text = data.name; 
        } else{
            FileStream stream = new FileStream(path, FileMode.Create);
            Userdata data = new Userdata();

            data.name = "User";
            data.attempt = 0;
            data.score = 0;

            formatter.Serialize(stream, data);
            stream.Close();
            
            quizAttempsText.text = data.attempt.ToString();
            lastScoreText.text = data.score.ToString()+"/5";
            //nameText.text = data.name;
        }
        
        // Set placeholder
        //inputName.GetComponent<TMP_InputField>().placeholder.GetComponent<TMP_Text>().text = "Enter Your Name...";

        LearnButton.GetComponent<Button>().interactable = false;
        QuizButton.GetComponent<Button>().interactable = false;
        StatisticsButton.GetComponent<Button>().interactable = false;
        
        StatisticsPanel.SetActive(true);
    }

    // Save user name
    public void SaveName()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.carbon";
        FileStream stream = new FileStream(path, FileMode.Create);

        Userdata data = new Userdata();
        data.attempt = Int32.Parse(quizAttempsText.text);
        data.score = Int32.Parse(lastScoreText.text);

        //data.name = inputName.GetComponent<TMP_InputField>().text;        
        //nameText.text = user.name;

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public void Okay()
    {
        LearnButton.GetComponent<Button>().interactable = true;
        QuizButton.GetComponent<Button>().interactable = true;
        StatisticsButton.GetComponent<Button>().interactable = true;
        
        StatisticsPanel.SetActive(false);
    }
}
