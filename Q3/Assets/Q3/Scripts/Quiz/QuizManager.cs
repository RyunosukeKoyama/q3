using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Transform sectionParent;
    private TextMeshProUGUI sectionGUI;
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
        var level = QuizParam.GetLevel();
        levelText.text = level;

        StartCoroutine(ModalManager.I.GenerateLoading());
        Debug.Log("start");

        var ie = MasterLoader.I.LoadQuizzes(level);
        var coroutine = StartCoroutine(ie);

        yield return coroutine;

        var allQuizzes = (List<Quiz>) ie.Current;
        var chapter = QuizParam.GetChapter();
        var selectedChapterQuizzes = allQuizzes.Where(q => chapter == 0 || q.Chapter == chapter);
        QuizScore.SaveAllIds(chapter, selectedChapterQuizzes.Select(q => q.Id).ToList());

        var isRandom = QuizParam.GetTrend();
        var correctIds = QuizScore.GetCorrectIds(chapter);
        var incorrectIds = QuizScore.GetIncorrectIds(chapter);

        selectedQuizzes = selectedChapterQuizzes
                            .OrderBy(_ => Guid.NewGuid())
                            .OrderBy(q => isRandom || incorrectIds.Contains(q.Id) ? -1 : correctIds.Contains(q.Id) ? 1 : 0)
                            .Take(QuizParam.GetQuantity())
                            .ToList();

        Debug.Log(string.Join(',', correctIds));
        Debug.Log(string.Join(',', incorrectIds));
        Debug.Log(string.Join(',', selectedQuizzes.Select(q => q.Id)));

        remainingQuizzes = new List<Quiz>(selectedQuizzes);

        StartQuiz();

        ModalManager.I.DeleteModal();
    }

    public void Restart(bool onlyIncorrect = false)
    {
        HideResult();

        remainingQuizzes = onlyIncorrect ? new List<Quiz>(incorrectQuizzes) : new List<Quiz>(selectedQuizzes);
        incorrectQuizzes.Clear();

        StartQuiz();
    }

    private void StartQuiz()
    {
        currentQuiz = remainingQuizzes.First();

        SetSection(currentQuiz.Section);
        ShowSection();

        SetQuestion(currentQuiz.Question);
        ShowQuestion();

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
            SaveScore();
        }
    }

    private void ShowSection()
    {
        sectionParent.gameObject.SetActive(true);
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
        explanationParent.gameObject.SetActive(true);
    }

    private void ShowResult()
    {
        sectionParent.gameObject.SetActive(false);
        questionParent.gameObject.SetActive(false);
        explanationParent.gameObject.SetActive(false);
        resultParent.gameObject.SetActive(true);
        var isIncorrect = incorrectQuizzes.Count > 0;
        restartOnlyIncorrect.SetActive(isIncorrect);

        var gui = resultParent.Find("CorrectCount").GetComponentInChildren<TextMeshProUGUI>();
        gui.text = $"正解数\n<mark><size=150>{selectedQuizzes.Count - incorrectQuizzes.Count}/{selectedQuizzes.Count} </size></mark>";
    }

    private void SaveScore()
    {
        var correctIds = selectedQuizzes.Except(incorrectQuizzes).Select(q => q.Id).ToList();
        var incorrectIds = incorrectQuizzes.Select(q => q.Id).ToList();
        QuizScore.SaveScore(QuizParam.GetChapter(), correctIds, incorrectIds);
        Debug.Log(string.Join(',', correctIds) + " : " + string.Join(',', incorrectIds));
    }

    private void SetSection(string section)
    {
        sectionGUI ??= sectionParent.GetComponentInChildren<TextMeshProUGUI>();
        sectionGUI.text = section;
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

    private void ClearChoices(Transform ignoreTransform = null)
    {
        foreach (Transform choice in choicesParent)
        {
            if (ignoreTransform == choice) continue;

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
            button.onClick.AddListener(() => ChooseChoice(tmpI));
            var gui = choiceGameObject.GetComponentInChildren<TextMeshProUGUI>();
            gui.text = choices[i];
        }
    }

    private void ChooseChoice(int choiceNum)
    {
        var chooseChoice = choicesParent.Find(choiceNum.ToString());
        chooseChoice.GetComponent<Button>().onClick.RemoveAllListeners();
        ClearChoices(chooseChoice);

        SetExplanation(choiceNum);
        ShowExplanation();

        if (currentQuiz.CorrectChoiceIndex == choiceNum)
        {
            chooseChoice.GetComponent<Image>().color = new Color32(180, 255, 180, 255); // Green
            correctAnswer.SetActive(true);
        }
        else
        {
            chooseChoice.GetComponent<Image>().color = new Color32(255, 180, 180, 255); // Red
            incorrectAnswer.SetActive(true);
            incorrectQuizzes.Add(currentQuiz);
        }

        remainingQuizzes.Remove(currentQuiz);
    }
}
