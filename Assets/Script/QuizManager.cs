using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Vuforia;

public class QuizManager : MonoBehaviour
{   
    public GameObject QOPanel;
    public GameObject QuizPanel;

    public UnityEngine.UI.Image[] Round_Result;
    public TMP_Text RoundTxt;
    
    public TMP_Text HintTxt;

    //Image target
    public TrackableBehaviour theTrackable;
    
    public List<QuestionAndAnswers> QnA;
    public GameObject[] AnswerButtons;    
    public Sprite[] AnswerImages;

    public GameObject[] QO_Result;  
    public GameObject[] QO_Score;

    public int currentQuestion;
    public int score;    
    int totalQuestions = 0;
    int currentround = 0;

    // Start game
    private void Start()
    {
        // Count how many questions we have
        totalQuestions = QnA.Count;

        //Disable Quiz over panel
        QOPanel.SetActive(false);

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
            HintTxt.text = "HINT: "+QnA[currentQuestion].Question;
            // Set round
            currentround = totalQuestions - QnA.Count + 1;
            RoundTxt.text = "ROUND: " + currentround + "/" + totalQuestions;
            
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
    // !!!! We have to make a new logic for game over display
    // 21.11.2020, It is for Demo scenario
    public void GameOver()
    {
        // Enable Gameover panel 
        QOPanel.SetActive(true);
        // Disable Quiz panel 
        QuizPanel.SetActive(false); 
        // Disable Image Target
        theTrackable.gameObject.SetActive(false);      
        for (int i=0; i< QO_Result.Length; i++)
        {
            QO_Result[i].SetActive(false);
        }
        for (int i=0; i< QO_Score.Length; i++)
        {
            QO_Score[i].SetActive(false);
        }

        ///////////////////////////////// !!!!!!! FOR DEMO
        // Set result and score     
        // GO_Result-0: Bad, 1: Good, 2: Great
        // GO_Score-0: 0/3, 1: 1/3, 2: 2/3, 3: 3/3
        if (score==3)
        {
            // If you solve all questions, you get great messege
            QO_Result[2].SetActive(true);
            QO_Score[3].SetActive(true);
            QOPanel.GetComponent<UnityEngine.UI.Image>().color = new Color32(179,255,144,100);
        } else if (score==2)
        {
            // If you solve most questions, you get good messege
            QO_Result[1].SetActive(true);
            QO_Score[2].SetActive(true);
            QOPanel.GetComponent<UnityEngine.UI.Image>().color = new Color32(255,255,144,100);
        } else if (score==1)
        {
            // If you solve few questions, you get bad messege
            QO_Result[0].SetActive(true);
            QO_Score[1].SetActive(true);
            QOPanel.GetComponent<UnityEngine.UI.Image>().color = new Color32(255,157,144,100);
        } else if (score==0)
        {
            // If you solve few questions, you get bad messege
            QO_Result[0].SetActive(true);
            QO_Score[0].SetActive(true);
            QOPanel.GetComponent<UnityEngine.UI.Image>().color = new Color32(255,157,144,100);
        }     
        /////////////////////////////////
        
    }

    // If you select correct answer
    public void correct()
    {
        // You can get additional score
        score += 1;

        //Set color
        Round_Result[currentround-1].GetComponent<UnityEngine.UI.Image>().color = new Color32(0,215,0,200);

        // Remove current question from the question list
        QnA.RemoveAt(currentQuestion);

        // Genereate next question
        generateQuestion();
    }

    // If you select wrong answer
    public void wrong()
    {
        //Set color
        Round_Result[currentround-1].GetComponent<UnityEngine.UI.Image>().color = new Color32(236,0,0,200);

        // Remove current question from the question list
        QnA.RemoveAt(currentQuestion);
        // Genereate next question
        generateQuestion();
    }

    // Set Answer
    void SetAnswers()
    {

        for (int i=0; i< AnswerButtons.Length; i++)
        {
            // Set value of all answer as false first (initiate)
            AnswerButtons[i].GetComponent<AnswerScript>().isCorrect = false;
            // Set text from the answer option
            // options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = QnA[currentQuestion].Answers[i];

            ///////////////////////////////// !!!!!!! For Demo
            AnswerButtons[i].transform.GetChild(0).gameObject.SetActive(false);
            if(QnA[currentQuestion].Answers[i]=="Methan"){                
                AnswerButtons[i].GetComponent<UnityEngine.UI.Image>().sprite = AnswerImages[0];                
            } else if(QnA[currentQuestion].Answers[i]=="Ethan"){                
                AnswerButtons[i].GetComponent<UnityEngine.UI.Image>().sprite = AnswerImages[1];
            } else if(QnA[currentQuestion].Answers[i]=="Propan"){                
                AnswerButtons[i].GetComponent<UnityEngine.UI.Image>().sprite = AnswerImages[2];
            } else {                
                AnswerButtons[i].GetComponent<UnityEngine.UI.Image>().sprite = AnswerImages[3];
            }
            AnswerButtons[i].GetComponent<UnityEngine.UI.Image>().SetNativeSize();
            /////////////////////////////////

            // If specific answer is correct, set value as true 
            if(QnA[currentQuestion].CorrectAnswer == i+1)
            {
                AnswerButtons[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

}
