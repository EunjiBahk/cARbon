using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public QuizManager quizManager;

    public void Answer()
    {
        Debug.Log("isCorrect");
        Debug.Log(isCorrect);
        if(isCorrect)
        {
            quizManager.correct();
        }
        else
        {
            quizManager.wrong();
        }
    }
}
