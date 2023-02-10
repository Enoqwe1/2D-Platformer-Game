using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;

    private Sprite _buttonImage;
    private GameManager GM;
    private int wrongAns = 0;
    [HideInInspector]
    public bool colorblindIsActivated = false, canPlayerMove = true;

    public int totalChest = 0;
    public bool isComplete;
    public GameObject Panel, colorblindlessAsk, colorblindless;
    bool isActive = false;
    private void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void CheckWrongAns()
    {
        if (wrongAns == 2)
        {
            colorblindlessAsk.SetActive(true);
        }
        else if (wrongAns == 3)
        {
            colorblindless.SetActive(true);
        }
    }
    public void PanelOpener()
    {
        canPlayerMove = false;
        isActive = true;
        GetRandomQuestion();
        DisplayQuestion();
        SetDefaultButtonSprites();
        Panel.SetActive(isActive);
        isActive = false;
    }
    void GetRandomQuestion()
    {
        int index = Random.Range(0, questions.Count);
        currentQuestion = questions[index];

        if (questions.Contains(currentQuestion))
        {
            questions.Remove(currentQuestion);
        }
    }

    private void SetButtonState(bool state)
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    private void SetDefaultButtonSprites()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = Resources.Load<Sprite>("Images/" + currentQuestion.GetAnswer(i));
        }
    }

    private void DisplayQuestion()
    {
        questionText.text = currentQuestion.GetQuestion();
    }

    public void OnAnswerSelected(int index)
    {
        DisplayAnswer(index);
        SetButtonState(false);
    }

    private void DisplayAnswer(int index)
    {
        correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();
        if (index == correctAnswerIndex)
        {
            GM.CoinAddToScore(1000);
            questionText.text = "Correct!";
            Invoke("ClosePanel", 2);
        }

        else
        {
            if (currentQuestion.GetQuestionType())
            {
                questionText.text = "Sorry, the correct answer was the " + (correctAnswerIndex + 1) + ".";
            }
            else
            {
                string correctAnswer = currentQuestion.GetAnswer(correctAnswerIndex);
                questionText.text = "Sorry, the correct answer was;\n" + correctAnswer;
            }
            wrongAns += 1;
            Invoke("ClosePanel", 2);
            if (!colorblindIsActivated)
            {
                CheckWrongAns();
            }
        }
        totalChest += 1;
        canPlayerMove = true;
    }

    public void ClosePanel()
    {
        Panel.SetActive(isActive);
        GetRandomQuestion();
        DisplayQuestion();
        SetButtonState(true);
        SetDefaultButtonSprites();
    }


}
