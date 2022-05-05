using System;

[Serializable]
public class Quiz
{
    public int Id;
    public int Chapter;
    public string Section = "NO DATA";
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

