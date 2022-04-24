using System;

[Serializable]
public class Quiz
{
    public int Id;
    public string Question = "NO DATA";
    public string[] Choices;
    public int CorrectChoiceIndex;
    public string[] Explanations;
}

[Serializable]
public class QuizArray
{
    public Quiz[] quizzes;
}

