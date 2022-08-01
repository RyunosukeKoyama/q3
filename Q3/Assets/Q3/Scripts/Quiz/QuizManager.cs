using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private QuizWindow window;
    private List<Quiz> remainingQuizzes;
    private int totalQuizzesCount;
    private Quiz currentQuiz;
    private List<Quiz> incorrectQuizzes = new List<Quiz>();
    private List<Quiz> selectedQuizzes;
    private string level;

    private IEnumerator Start()
    {
        StartCoroutine(ModalManager.I.GenerateLoading());
        Debug.Log("start");

        level = QuizParam.GetLevel();
        var ie = MasterLoader.I.LoadQuizzes(level);
        var coroutine = StartCoroutine(ie);
        yield return coroutine;

        ModalManager.I.DeleteModal();

        selectedQuizzes = SelectQuizzes((List<Quiz>) ie.Current);
        SetRemainingQuizzes(selectedQuizzes);
        StartQuiz();
    }

    public void Restart(bool onlyIncorrect = false)
    {
        window.HideResult();

        SetRemainingQuizzes(onlyIncorrect ? incorrectQuizzes : selectedQuizzes);
        selectedQuizzes = new List<Quiz>(remainingQuizzes); // 次のために分母を更新する
        StartQuiz();
    }

    private List<Quiz> SelectQuizzes(List<Quiz> allQuizzes)
    {
        var chapter = QuizParam.GetChapter();
        var selectedChapterQuizzes = allQuizzes.Where(q => chapter == 0 || q.Chapter == chapter);
        QuizScore.SaveAllIds(chapter, selectedChapterQuizzes.Select(q => q.Id).ToList());

        var isRandom = QuizParam.GetTrend();
        var correctIds = QuizScore.GetCorrectIds(chapter);
        var incorrectIds = QuizScore.GetIncorrectIds(chapter);

        return selectedChapterQuizzes
                            .OrderBy(_ => Guid.NewGuid())
                            .OrderBy(q => isRandom || incorrectIds.Contains(q.Id) ? -1 : correctIds.Contains(q.Id) ? 1 : 0)
                            .Take(QuizParam.GetQuantity())
                            .ToList();
    }

    private void SetRemainingQuizzes(List<Quiz> quizzes)
    {
        remainingQuizzes = new List<Quiz>(quizzes);
        totalQuizzesCount = remainingQuizzes.Count();
        incorrectQuizzes.Clear();
    }

    private void StartQuiz()
    {
        currentQuiz = remainingQuizzes.First();

        window.SetSection(level);
        window.SetQuestion(currentQuiz.Question);
        window.SetCount(totalQuizzesCount - remainingQuizzes.Count + 1, totalQuizzesCount);

        window.ClearChoices();
        GenerateChoices(currentQuiz.Choices);

        window.ShowQuiz();
    }

    public void FinishQuiz()
    {
        window.HideAnswer();

        if (remainingQuizzes.Count > 0)
        {
            StartQuiz();
        }
        else
        {
            window.HideQuiz();
            window.ShowResult(totalQuizzesCount - incorrectQuizzes.Count, totalQuizzesCount);
            SaveScore();
        }
    }

    private void SaveScore()
    {
        var correctIds = selectedQuizzes.Except(incorrectQuizzes).Select(q => q.Id).ToList();
        var incorrectIds = incorrectQuizzes.Select(q => q.Id).ToList();
        QuizScore.SaveScore(QuizParam.GetChapter(), correctIds, incorrectIds);
        Debug.Log(string.Join(',', correctIds) + " : " + string.Join(',', incorrectIds));
    }


    private void GenerateChoices(string[] choices)
    {
        for (var i = 0; i < choices.Length; i++)
        {
            var button = window.GenerateChoice(i.ToString(), choices[i]);
            var tmpI = i; // temporary index for function argument
            button.onClick.AddListener(() => ChooseChoice(tmpI));
        }
    }

    private void ChooseChoice(int choiceNum)
    {
        var chooseChoice = window.GetChoice(choiceNum);
        chooseChoice.GetComponent<Button>().onClick.RemoveAllListeners();
        window.ClearChoices(ignoreChoice: chooseChoice);

        window.SetSection(currentQuiz.Section);
        window.ShowExplanation(currentQuiz.Explanations[choiceNum]);

        if (currentQuiz.CorrectChoiceIndex == choiceNum)
        {
            chooseChoice.GetComponent<Image>().color = new Color32(180, 255, 180, 255); // Green
            window.ShowCorrectAnswer();
        }
        else
        {
            chooseChoice.GetComponent<Image>().color = new Color32(255, 180, 180, 255); // Red
            window.ShowIncorrectAnswer();
            incorrectQuizzes.Add(currentQuiz);
        }

        remainingQuizzes.Remove(currentQuiz);
    }
}
