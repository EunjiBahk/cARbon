﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using System;

public class Settings : MonoBehaviour
{
    public GameObject SettingPanel;
    public GameObject Background;
    public GameObject LearnButton;
    public GameObject QuizButton;
    public GameObject SettingsButton;

    public TMP_Text nameText;
    public TMP_Text quizAttempsText;
    public TMP_Text lastgradeText;

    public TMP_InputField inputName;

    public void OpenSetting()
    {
        string path = Application.persistentDataPath + "/player.carbon";
        BinaryFormatter formatter = new BinaryFormatter();        

        if(File.Exists(path)){
            FileStream stream = new FileStream(path, FileMode.Open);
            Userdata data = formatter.Deserialize(stream) as Userdata;
            stream.Close();

            nameText.text = data.name;            
            quizAttempsText.text = data.attempt.ToString();
            lastgradeText.text = data.score.ToString()+"/5";

        } else{
            FileStream stream = new FileStream(path, FileMode.Create);
            User user = new User();
            user.name = "User";
            user.attempt = 0;
            user.score = 0;

            Userdata data = new Userdata(user);
            formatter.Serialize(stream, data);
            stream.Close();

            nameText.text = user.name;
            quizAttempsText.text = user.attempt.ToString();
            lastgradeText.text = user.score.ToString()+"/5";
        }
        
        inputName.GetComponent<TMP_InputField>().placeholder.GetComponent<TMP_Text>().text = "Enter Your Name...";

        LearnButton.GetComponent<Button>().interactable = false;
        QuizButton.GetComponent<Button>().interactable = false;
        SettingsButton.GetComponent<Button>().interactable = false;
        
        SettingPanel.SetActive(true);
    }

    public void SaveName()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.carbon";
        FileStream stream = new FileStream(path, FileMode.Create);

        User user = new User();
        user.name = inputName.GetComponent<TMP_InputField>().text;
        user.attempt = Int32.Parse(quizAttempsText.text);
        user.score = Int32.Parse(lastgradeText.text);
        
        Userdata data = new Userdata(user);

        nameText.text = user.name;

        formatter.Serialize(stream, data);
        stream.Close();

    }

    public void Okay()
    {
        LearnButton.GetComponent<Button>().interactable = true;
        QuizButton.GetComponent<Button>().interactable = true;
        SettingsButton.GetComponent<Button>().interactable = true;
        
        SettingPanel.SetActive(false);
    }
}
