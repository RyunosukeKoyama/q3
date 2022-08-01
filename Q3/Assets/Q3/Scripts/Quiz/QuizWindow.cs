using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizWindow : MonoBehaviour
{
    [SerializeField] private Transform headerParent;
    [SerializeField] private TextMeshProUGUI sectionGUI;
    [SerializeField] private TextMeshProUGUI countGUI;
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
    private TextMeshProUGUI correctCountGUI;

    public void ShowQuiz()
    {
        headerParent.gameObject.SetActive(true);
        questionParent.gameObject.SetActive(true);
        choicesParent.gameObject.SetActive(true);
        explanationParent.gameObject.SetActive(false);
    }

    public void HideQuiz()
    {
        headerParent.gameObject.SetActive(false);
        questionParent.gameObject.SetActive(false);
        choicesParent.gameObject.SetActive(false);
        explanationParent.gameObject.SetActive(false);
    }

    public void ShowCorrectAnswer()
    {
        correctAnswer.SetActive(true);
    }

    public void ShowIncorrectAnswer()
    {
        incorrectAnswer.SetActive(true);
    }

    public void HideAnswer()
    {
        correctAnswer.SetActive(false);
        incorrectAnswer.SetActive(false);
    }

    public void ShowExplanation(string text)
    {
        explanationParent.gameObject.SetActive(true);
        explanationGUI ??= explanationParent.GetComponentInChildren<TextMeshProUGUI>();
        explanationGUI.text = text;
    }

    public void ShowResult(int correctCount, int totalCount)
    {
        resultParent.gameObject.SetActive(true);
        var isIncorrect = correctCount != totalCount;
        restartOnlyIncorrect.SetActive(isIncorrect);

        correctCountGUI ??= resultParent.Find("CorrectCount").GetComponentInChildren<TextMeshProUGUI>();
        correctCountGUI.text = $"正解数\n<mark><size=150>{correctCount}/{totalCount} </size></mark>";
    }

    public void HideResult()
    {
        resultParent.gameObject.SetActive(false);
    }

    public void SetCount(int currentCount, int totalCount)
    {
        countGUI.text = $"{currentCount}/{totalCount}";
    }
    public void SetSection(string section)
    {
        sectionGUI ??= headerParent.GetComponentInChildren<TextMeshProUGUI>();
        sectionGUI.text = section;
    }

    public void SetQuestion(string question)
    {
        questionGUI ??= questionParent.GetComponentInChildren<TextMeshProUGUI>();
        questionGUI.text = question;
    }

    public Transform GetChoice(int choiceNum)
    {
        return choicesParent.Find(choiceNum.ToString());
    }

    public Button GenerateChoice(string name, string text)
    {
        var choiceGameObject = Instantiate(choicePrefab, choicesParent);
        choiceGameObject.name = name.ToString();
        var gui = choiceGameObject.GetComponentInChildren<TextMeshProUGUI>();
        gui.text = text;
        return choiceGameObject.GetComponent<Button>();
    }

    public void ClearChoices(Transform ignoreChoice = null)
    {
        foreach (Transform choice in choicesParent)
        {
            if (ignoreChoice == choice) continue;

            Destroy(choice.gameObject);
        }
    }
}
