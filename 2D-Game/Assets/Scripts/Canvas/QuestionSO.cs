using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quiz Question", fileName = "New Question")]
public class QuestionSO : ScriptableObject
{
    [TextArea(2, 6)]
    [SerializeField] string question = "Enter new question text here";
    [SerializeField] bool questionType;
    [SerializeField] string[] answers = new string[3];
    [SerializeField] int correctAnswerIndex;

    public string GetQuestion()
    {
        return question;
    }

    public string GetAnswer(int index)
    {
        return answers[index];
    }
    public bool GetQuestionType()
    {
        return questionType;
    }

    public int GetCorrectAnswerIndex()
    {
        return correctAnswerIndex;
    }
}
