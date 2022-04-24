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
    [SerializeField] private GameObject restartOnlyIncorrect;


    private List<Quiz> remainingQuizzes;
    private Quiz currentQuiz;
    private List<Quiz> incorrectQuizzes = new List<Quiz>();
    private List<Quiz> selectedQuizzes;

    private IEnumerator Start()
    {
        StartCoroutine(ModalManager.I.GenerateLoading());
        Debug.Log("start");

        var ie = MasterLoader.I.LoadQuizzes();
        var coroutine = StartCoroutine(ie);

        yield return coroutine;

        var allQuizzes = (List<Quiz>)ie.Current;
        selectedQuizzes = allQuizzes.Take(QuizParam.GetQuantity()).ToList();
        remainingQuizzes = new List<Quiz>(selectedQuizzes);

        ShowQuestion();
        StartQuiz();

        ModalManager.I.DeleteModal();
    }

    public void Restart(bool onlyIncorrect = false)
    {
        HideResult();

        remainingQuizzes = onlyIncorrect ? new List<Quiz>(incorrectQuizzes) : new List<Quiz>(selectedQuizzes);
        incorrectQuizzes.Clear();

        ShowQuestion();
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

    public void FinishQuiz()
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

    private void ShowQuestion()
    {
        questionParent.gameObject.SetActive(true);
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
        var isIncorrect = incorrectQuizzes.Count > 0;
        restartOnlyIncorrect.SetActive(isIncorrect);

        var gui = resultParent.Find("CorrectCount").GetComponentInChildren<TextMeshProUGUI>();
        gui.text = $"正解数\n<mark><size=150>{selectedQuizzes.Count - incorrectQuizzes.Count}/{selectedQuizzes.Count} </size></mark>";
    }

    private void SetQuestion(string question)
    {
        questionGUI ??= questionParent.GetComponentInChildren<TextMeshProUGUI>();
        questionGUI.text = question;
    }

    private void SetExplanation(int choiceNum)
    {
        explanationGUI ??= explanationParent.GetComponentInChildren<TextMeshProUGUI>();
        explanationGUI.text = currentQuiz.Explanations[choiceNum];
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
        SetExplanation(choiceNum);
        ShowExplanation();

        if (currentQuiz.CorrectChoiceIndex == choiceNum)
        {
            correctAnswer.SetActive(true);
        }
        else
        {
            incorrectAnswer.SetActive(true);
            incorrectQuizzes.Add(currentQuiz);
        }

        remainingQuizzes.Remove(currentQuiz);
    }
}
