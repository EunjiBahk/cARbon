using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    public string name = "User";
    public int attempt = 0;
    public int score = 0;

    public void SaveUser()
    {
        Save.SaveUser(this);
    }

    public void LoadUser()
    {
        Userdata data = Save.LoadUser();

        name = data.name;
        attempt = data.attempt;
        score = data.score;
    }
}
