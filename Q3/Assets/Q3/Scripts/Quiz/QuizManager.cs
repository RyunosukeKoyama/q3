using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private Transform questionParent;
    private TextMeshProUGUI questionGUI;
    [SerializeField] private Transform choicesParent;
    [SerializeField] private GameObject choicePrefab;
    [SerializeField] private GameObject correctAnswer;
    [SerializeField] private GameObject incorrectAnswer;
    [SerializeField] private Transform explanationParent;
    private TextMeshProUGUI explanationGUI;
    [SerializeField] private Transform resultParent;

    private List<Quiz> remainingQuizzes;
    private Quiz currentQuiz;
    private List<Quiz> correctQuizzes = new List<Quiz>();
    private List<Quiz> loadedQuizzes;

    private IEnumerator Start()
    {
        explanationParent.GetComponent<Button>().onClick.AddListener(FinishQuiz);

        var ie = MasterLoader.I.LoadQuizzes();
        var coroutine = StartCoroutine(ie);

        yield return coroutine;

        loadedQuizzes = (List<Quiz>)ie.Current;
        remainingQuizzes = new List<Quiz>(loadedQuizzes);

        StartQuiz();
    }

    public void Restart()
    {
        HideResult();

        remainingQuizzes = new List<Quiz>(loadedQuizzes);
        correctQuizzes.Clear();

        StartQuiz();
    }

    private void StartQuiz()
    {
        currentQuiz = remainingQuizzes.First();

        SetQuestion(currentQuiz.Question);

        ClearChoices();
        GenerateChoices(currentQuiz.Choices);
        ShowChoices();

        HideAnswer();
    }

    private void FinishQuiz()
    {
        if (remainingQuizzes.Count > 0)
        {
            StartQuiz();
        }
        else
        {
            ShowResult();
        }
    }

    private void ShowChoices()
    {
        choicesParent.gameObject.SetActive(true);
        explanationParent.gameObject.SetActive(false);
    }

    private void ShowExplanation()
    {
        choicesParent.gameObject.SetActive(false);
        explanationParent.gameObject.SetActive(true);
    }

    private void ShowResult()
    {
        questionParent.gameObject.SetActive(false);
        explanationParent.gameObject.SetActive(false);
        resultParent.gameObject.SetActive(true);

        var gui = resultParent.Find("CorrectCount").GetComponentInChildren<TextMeshProUGUI>();
        gui.text = $"正解数\n<mark><size=150>{correctQuizzes.Count}/{loadedQuizzes.Count} </size></mark>";
    }

    private void SetQuestion(string question)
    {
        questionGUI ??= questionParent.GetComponentInChildren<TextMeshProUGUI>();
        questionGUI.text = question;
    }

    private void SetExplanation()
    {
        explanationGUI ??= explanationParent.GetComponentInChildren<TextMeshProUGUI>();
        explanationGUI.text = currentQuiz.Explanation;
    }

    private void ClearChoices()
    {
        foreach (Transform choice in choicesParent)
        {
            Destroy(choice.gameObject);
        }
    }

    private void HideAnswer()
    {
        correctAnswer.SetActive(false);
        incorrectAnswer.SetActive(false);
    }

    private void HideResult()
    {
        questionParent.gameObject.SetActive(true);
        explanationParent.gameObject.SetActive(true);
        resultParent.gameObject.SetActive(false);
    }

    private void GenerateChoices(string[] choices)
    {
        for (var i = 0; i < choices.Length; i++)
        {
            var choiceGameObject = Instantiate(choicePrefab, choicesParent);
            choiceGameObject.name = i.ToString();
            var button = choiceGameObject.GetComponent<Button>();
            var tmpI = i;
            button.onClick.AddListener(() => ChooceChoice(tmpI));
            var gui = choiceGameObject.GetComponentInChildren<TextMeshProUGUI>();
            gui.text = choices[i];
        }
    }

    private void ChooceChoice(int choiceNum)
    {
        SetExplanation();
        ShowExplanation();

        if (currentQuiz.CorrectChoiceIndex == choiceNum)
        {
            correctAnswer.SetActive(true);
            correctQuizzes.Add(currentQuiz);
        }
        else
        {
            incorrectAnswer.SetActive(true);
        }

        remainingQuizzes.Remove(currentQuiz);
    }
}
