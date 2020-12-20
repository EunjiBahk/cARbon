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

    //Round
    public TMP_Text RoundTxt;
    public UnityEngine.UI.Image[] Round_Result;

    //Image target
    public TrackableBehaviour theTrackable;
    
    //Questions
    public List<QuestionAndAnswers> QnA;    
    public int currentQuestion; //question index
    public int totalQuestions;

    //Answer GUI
    public GameObject[] AnswerButtons;    
    public Sprite[] AnswerImages;
    public Sprite[] AnswerCorrectImages;
    public Sprite[] AnswerWrongImages;
    public Sprite[] AnswerDisabledImages;

    //Score
    public TMP_Text QO_Score;
    public int score;    
    public int currentround;

    //Hint1-50:50    
    public GameObject Hint1Buttons; 
    public TMP_Text Hint1;
    public int Hint1_cnt=0;

    //Hint2-sum name of molecule
    public GameObject Hint2Buttons; 
    public TMP_Text Hint2;
    public GameObject HintPanel;
    public TMP_Text HintPanel_text;
    public int Hint2_cnt=0; 

    // Start game
    private void Start()
    {
        // Count how many questions we have
        totalQuestions = QnA.Count;

        // Enable Round result
        for(int i=0; i<totalQuestions; i++){
            Round_Result[i].gameObject.SetActive(true);
        }

        // Disable Quiz over panel, Hint panel
        QOPanel.SetActive(false);
        HintPanel.SetActive(false);

        // Generate question
        generateQuestion();        
    }
    
    // Generate question
    void generateQuestion()
    {
        // If we have questions left 
        if(QnA.Count > 0)
        {            
            // Make a random question from the question list
            currentQuestion = Random.Range(0, QnA.Count);
            
            // Set round text
            currentround = totalQuestions - QnA.Count + 1;
            RoundTxt.text = "ROUND: " + currentround + "/" + totalQuestions;

            //Answer button
            // Enable all answer buttons
            for (int i=0; i< AnswerButtons.Length; i++){
                AnswerButtons[i].GetComponent<Button>().interactable = true;
            }

            //Hint
            // If user doesn't use hint so far, Enable hint button
            if(Hint1_cnt==0){
                Hint1Buttons.GetComponent<Button>().interactable = true;
            }
            if(Hint2_cnt==0){
                Hint2Buttons.GetComponent<Button>().interactable = true;
            }
            // Disable Hint panel 
            HintPanel.SetActive(false);

            //Image target
            // Disable all child object of image target
            for (int i = 0; i < theTrackable.gameObject.transform.childCount; i++){
                theTrackable.gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
            // Enable current 3d model
            QnA[currentQuestion].augmentationObject.SetActive(true);            
            
            // Set answers
            SetAnswers();            
        }
        else // if we don't have any question
        {
            // Game over and show result
            GameOver();
        }
    }      

    // Set Answer
    void SetAnswers()
    {

        for(int i=0; i< AnswerButtons.Length; i++)
        {
            // Set value of all answer as false first (initiate)
            AnswerButtons[i].GetComponent<AnswerScript>().isCorrect = false;
            
            // Disable Answer text 
            AnswerButtons[i].transform.GetChild(0).gameObject.SetActive(false);
            
            // Set Image, Index
            //0:Methan 1:Ethan 2:Methanol 3:Ethanol 4:CycloPropan
            if(QnA[currentQuestion].Answers[i]=="Methane"){                
                AnswerButtons[i].GetComponent<UnityEngine.UI.Image>().sprite = AnswerImages[0];
                AnswerButtons[i].GetComponent<AnswerScript>().moleculeIdx = 0;                
            } else if(QnA[currentQuestion].Answers[i]=="Ethane"){                
                AnswerButtons[i].GetComponent<UnityEngine.UI.Image>().sprite = AnswerImages[1];
                AnswerButtons[i].GetComponent<AnswerScript>().moleculeIdx = 1; 
            } else if(QnA[currentQuestion].Answers[i]=="Methanol"){                
                AnswerButtons[i].GetComponent<UnityEngine.UI.Image>().sprite = AnswerImages[2];
                AnswerButtons[i].GetComponent<AnswerScript>().moleculeIdx = 2; 
            } else if(QnA[currentQuestion].Answers[i]=="Ethanol"){                
                AnswerButtons[i].GetComponent<UnityEngine.UI.Image>().sprite = AnswerImages[3];
                AnswerButtons[i].GetComponent<AnswerScript>().moleculeIdx = 3; 
            } else if(QnA[currentQuestion].Answers[i]=="Cyclopropane"){                  
                AnswerButtons[i].GetComponent<UnityEngine.UI.Image>().sprite = AnswerImages[4];
                AnswerButtons[i].GetComponent<AnswerScript>().moleculeIdx = 4; 
            }
            AnswerButtons[i].GetComponent<UnityEngine.UI.Image>().SetNativeSize();
            
            // If specific answer is correct, set value as true 
            if(QnA[currentQuestion].CorrectAnswer == i+1){
                AnswerButtons[i].GetComponent<AnswerScript>().isCorrect = true;
            }
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
        // Enable Gameover panel 
        QOPanel.SetActive(true);
        // Disable Quiz panel, Image Target
        QuizPanel.SetActive(false);
        theTrackable.gameObject.SetActive(false);    
        //Set score
        QO_Score.text = score+"/"+totalQuestions;         
    }

    // If you select correct answer
    public void correct()
    {
        // You can get additional score
        score += 1;

        // Make Round result green 
        Round_Result[currentround-1].GetComponent<UnityEngine.UI.Image>().color = new Color32(36,136,116,200);
        
        // Set feedback
        for (int i=0; i< AnswerButtons.Length; i++)
        {
            AnswerButtons[i].GetComponent<Button>().interactable = false;
            // If specific answer is correct, set value as true 
            if(QnA[currentQuestion].CorrectAnswer != i+1){
                AnswerButtons[i].GetComponent<UnityEngine.UI.Image>().sprite = AnswerDisabledImages[AnswerButtons[i].GetComponent<AnswerScript>().moleculeIdx];
            } else{
                AnswerButtons[i].GetComponent<UnityEngine.UI.Image>().sprite = AnswerCorrectImages[AnswerButtons[i].GetComponent<AnswerScript>().moleculeIdx];
            }
        }
    
        // Set Hint buttons interactable as false
        Hint1Buttons.GetComponent<Button>().interactable = false;
        Hint2Buttons.GetComponent<Button>().interactable = false;
        
        // Remove current question from the question list
        QnA.RemoveAt(currentQuestion);

        // Genereate next question in 2sec.
        Invoke("generateQuestion",2f);
        
    }

    // If you select wrong answer
    public void wrong(int num)
    {
        // Make Round result red 
        Round_Result[currentround-1].GetComponent<UnityEngine.UI.Image>().color = new Color32(196,33,22,200);
                
        //Set feedback
        for (int i=0; i< AnswerButtons.Length; i++)
        {
            AnswerButtons[i].GetComponent<Button>().interactable = false;            
            // If specific answer is correct, set value as true 
            if(QnA[currentQuestion].CorrectAnswer != i+1){
                AnswerButtons[i].GetComponent<UnityEngine.UI.Image>().sprite = AnswerDisabledImages[AnswerButtons[i].GetComponent<AnswerScript>().moleculeIdx];
            } else{
                AnswerButtons[i].GetComponent<UnityEngine.UI.Image>().sprite = AnswerCorrectImages[AnswerButtons[i].GetComponent<AnswerScript>().moleculeIdx];
            }
        }
        AnswerButtons[num-1].GetComponent<UnityEngine.UI.Image>().sprite = AnswerWrongImages[AnswerButtons[num-1].GetComponent<AnswerScript>().moleculeIdx];
    
        // Set Hint buttons interactable as false
        Hint1Buttons.GetComponent<Button>().interactable = false;
        Hint2Buttons.GetComponent<Button>().interactable = false;
        
        // Remove current question from the question list
        QnA.RemoveAt(currentQuestion);
        
        // Genereate next question in 2sec.
        Invoke("generateQuestion",2f);
    }

    public void hint1()
    {    
        // Change Hint1 used
        Hint1_cnt=1;

        List<int> wrongAnswers = new List<int>();
        int buttonIdx = 0;
        int moleculeIdx =0;
        
        // Disable hint button
        Hint1.color = Color.gray;
        Hint1Buttons.GetComponent<Button>().interactable = false;
        
        // Add Wrong answers to list
        for (int i=0; i< AnswerButtons.Length; i++)
        {
            if(QnA[currentQuestion].CorrectAnswer != i+1){
                wrongAnswers.Add(i);
            }
        }

        // Pick 2 wrong answers randomly
        moleculeIdx = Random.Range(0, wrongAnswers.Count);
        buttonIdx= wrongAnswers[moleculeIdx];
        wrongAnswers.RemoveAt(moleculeIdx);
        AnswerButtons[buttonIdx].GetComponent<UnityEngine.UI.Image>().sprite = AnswerDisabledImages[AnswerButtons[buttonIdx].GetComponent<AnswerScript>().moleculeIdx];
        AnswerButtons[buttonIdx].GetComponent<Button>().interactable = false;
        
        moleculeIdx = Random.Range(0, wrongAnswers.Count);
        buttonIdx= wrongAnswers[moleculeIdx];
        wrongAnswers.RemoveAt(moleculeIdx);
        AnswerButtons[buttonIdx].GetComponent<UnityEngine.UI.Image>().sprite = AnswerDisabledImages[AnswerButtons[buttonIdx].GetComponent<AnswerScript>().moleculeIdx];
        AnswerButtons[buttonIdx].GetComponent<Button>().interactable = false;        
    }

    public void hint2()
    {
        // Change Hint2 used
        Hint2_cnt=1;

        // Disable hint button
        Hint2.color = Color.gray;        
        Hint2Buttons.GetComponent<Button>().interactable = false;

        // Enable HINT panel
        HintPanel.SetActive(true);
        
        // Set HINT text
        HintPanel_text.text = "HINT: "+QnA[currentQuestion].Question;
    }

}
