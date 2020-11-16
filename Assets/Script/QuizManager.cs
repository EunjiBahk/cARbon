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

    // Start game
    private void Start()
    {
        // Count how many questions we have
        totlaQuestions = QnA.Count;
        // Disable Gameover panel 
        Gopanel.SetActive(false);
        // Generate question
        generateQuestion();
    }
    
    // Generate question
    void generateQuestion()
    {
        // If we have questions left go into here 
        if(QnA.Count > 0)
        {
            // Make a random question from the question list
            currentQuestion = Random.Range(0, QnA.Count);
            // Set question text
            QuestionTxt.text = QnA[currentQuestion].Question;
            
            // Disable all child object of image target
            for (int i = 0; i < theTrackable.gameObject.transform.childCount; i++)
            {
                theTrackable.gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
            
            // Make a parent to 3d model as image target
            QnA[currentQuestion].augmentationObject.transform.parent = theTrackable.transform;
            // Enable 3d model
            QnA[currentQuestion].augmentationObject.SetActive(true);
            
            // Change the location of 3d model a little bit
            if(QnA[currentQuestion].Question=="CH4")
            {
                QnA[currentQuestion].augmentationObject.transform.localPosition = new Vector3(0.03f,0.05f,0.06f);
            } 
            else if(QnA[currentQuestion].Question=="C3H8")
            {
                QnA[currentQuestion].augmentationObject.transform.localPosition = new Vector3(-0.03f,0.11f,-0.06f);
            } 
            else if(QnA[currentQuestion].Question=="C3H6")
            {
                QnA[currentQuestion].augmentationObject.transform.localPosition = new Vector3(-0.03f,0.08f,-0.04f);
            }
            
            QnA[currentQuestion].augmentationObject.transform.localRotation = Quaternion.identity;
            
            // Set answers
            SetAnswers();            
        }
        else // If we don't have any question
        {
            // Game over and show result
            GameOver();
        }
    }

    // Retry game
    public void retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Enable Gameover panel and Show result
    public void GameOver()
    {
        // Disable Quiz panel 
        Quizpanel.SetActive(false);
        // Enable Gameover panel 
        Gopanel.SetActive(true);
        // Set score text e.g. 1/3, 2/3, 3/3..
        ScoreTxt.text = score + "/" + totlaQuestions;
        if (score>=3)
        {
            // If you solve all questions, you get great messege
            ResultTxt.text = "GREAT!!";
        } else if (score>=2 && score<3)
        {
            // If you solve most questions, you get good messege
            ResultTxt.text = "GOOD!";
        } else
        {
            // If you solve few questions, you get bad messege
            ResultTxt.text = "BAD..";
        }       
        
    }

    // If you select correct answer
    public void correct()
    {
        // You can get additional score
        score += 1;
        // Remove current question from the question list
        QnA.RemoveAt(currentQuestion);
        // Genereate next question
        generateQuestion();
    }

    // If you select wrong answer
    public void wrong()
    {
        // Remove current question from the question list
        QnA.RemoveAt(currentQuestion);
        // Genereate next question
        generateQuestion();
    }

    // Set Answer
    void SetAnswers()
    {
        for (int i=0; i< options.Length; i++)
        {
            // Set value of all answer as false first (initiate)
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            // Set text from the answer option
            options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = QnA[currentQuestion].Answers[i];

            // If specific answer is correct, set value as true 
            if(QnA[currentQuestion].CorrectAnswer == i+1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

}
