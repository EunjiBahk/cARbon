using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Vuforia;

public class QuizManager : MonoBehaviour
{
    public List<QuestionAndAnswers> QnA;
    public GameObject[] options;
    public int currentQuestion;
 
    public GameObject Quizpanel;
    public GameObject Gopanel;

    //public GameObject augmentationModel; image target
    public TrackableBehaviour theTrackable;

    public TMP_Text QuestionTxt;  
    public TMP_Text ResultTxt;  
    public TMP_Text ScoreTxt;    

    int totlaQuestions = 0;
    public int score;

    private void Start()
    {
        totlaQuestions = QnA.Count;
        Gopanel.SetActive(false);
        generateQuestion();
    }

    public void retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        Quizpanel.SetActive(false);
        Gopanel.SetActive(true);
        ScoreTxt.text = score + "/" + totlaQuestions;
        if (score>=3)
        {
            ResultTxt.text = "GREAT!!";
        } else if (score>=2 && score<3)
        {
            ResultTxt.text = "GOOD!";
        } else
        {
            ResultTxt.text = "BAD..";
        }       
        
    }

    public void correct()
    {
        score += 1;
        QnA.RemoveAt(currentQuestion);
        generateQuestion();
    }

    public void wrong()
    {
        QnA.RemoveAt(currentQuestion);
        generateQuestion();
    }

    void SetAnswers()
    {
        for (int i=0; i< options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = QnA[currentQuestion].Answers[i];

            if(QnA[currentQuestion].CorrectAnswer == i+1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
            //Debug.Log(options[i].GetComponent<AnswerScript>().isCorrect);
        }
    }

    void generateQuestion()
    {
        if(QnA.Count > 0)
        {
            //Debug.Log(QnA.Count); // 2
            currentQuestion = Random.Range(0, QnA.Count);
            //Debug.Log(currentQuestion); // 0 or 1
            QuestionTxt.text = QnA[currentQuestion].Question;
            
            //image target
            GameObject augmentationModel = theTrackable.gameObject;

            for (int i = 0; i < augmentationModel.transform.childCount; i++)
            {
                Transform child = augmentationModel.transform.GetChild(i);
                child.gameObject.SetActive(false);
            }

            //3d model
            QnA[currentQuestion].augmentationObject.transform.parent = theTrackable.transform;
            QnA[currentQuestion].augmentationObject.SetActive(true);
            
            if(QnA[currentQuestion].Question=="CH4")
            {
                Debug.Log("CH4"); 
                QnA[currentQuestion].augmentationObject.transform.localPosition = new Vector3(0.03f,0.05f,0.06f);
            } 
            else if(QnA[currentQuestion].Question=="C3H8")
            {
                Debug.Log("C3H8"); 
                QnA[currentQuestion].augmentationObject.transform.localPosition = new Vector3(-0.03f,0.11f,-0.06f);
            } 
            else if(QnA[currentQuestion].Question=="C3H6")
            {
                Debug.Log("C3H6");
                //QnA[currentQuestion].augmentationObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                QnA[currentQuestion].augmentationObject.transform.localPosition = new Vector3(-0.03f,0.08f,-0.04f);
            }
            
            QnA[currentQuestion].augmentationObject.transform.localRotation = Quaternion.identity;
            SetAnswers();            
        }
        else
        {
            Debug.Log("Out of Questions");
            GameOver();
        }
    }
}
