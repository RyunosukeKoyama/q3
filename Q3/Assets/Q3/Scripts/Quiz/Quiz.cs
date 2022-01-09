using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Quiz
{
    public int Id;
    public string Question = "NO DATA";
    public string[] Choices;
    public int CorrectChoiceIndex;
    public string Explanation = "NO DATA";
}

[Serializable]
public class QuizArray
{
    public Quiz[] quizzes;
}

