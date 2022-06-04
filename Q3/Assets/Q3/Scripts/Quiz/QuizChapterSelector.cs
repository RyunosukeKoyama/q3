using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Dropdown))]
public class QuizChapterSelector : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    private void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(QuizParam.ChapterChoices.ConvertAll(i =>
        {
            return i == 0 ? "全章" : i.ToString();
        }));
        dropdown.value = QuizParam.ChapterChoices.IndexOf(QuizParam.GetChapter());
    }

    public void OnClicked(int index)
    {
        QuizParam.SetChapter(QuizParam.ChapterChoices[index]);
    }
}
