using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Userdata
{
    public string name;
    public int attempt;
    public int score;
    
    public Userdata (User user)
    {
        name = user.name;
        attempt = user.attempt;
        score = user.score;
    }
}
