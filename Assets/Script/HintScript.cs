using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintScript : MonoBehaviour
{

    public QuizManager quizManager;

    public void Hint1()
    {
        quizManager.hint1();
    }
    public void Hint2()
    {
        quizManager.hint2();
    }
}
