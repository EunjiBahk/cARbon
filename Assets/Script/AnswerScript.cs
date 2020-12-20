using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public QuizManager quizManager;
    public int buttonnum = 0;
    public int moleculeIdx = 0;
    //0:Methan 1:Ethan 2:Methanol 3:Ethanol 4:CycloPropan

    public void Answer()
    {
        if(isCorrect)
        {
            quizManager.correct();
        }
        else
        {
            quizManager.wrong(buttonnum);
        }
    }
}
