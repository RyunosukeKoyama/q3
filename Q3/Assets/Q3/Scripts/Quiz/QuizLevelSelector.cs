using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Dropdown))]
public class QuizLevelSelector : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    private void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(QuizParam.LevelChoices);
        dropdown.value = QuizParam.LevelChoices.IndexOf(QuizParam.GetLevel());
    }

    public void OnClicked(int index)
    {
        QuizParam.SetLevel(QuizParam.LevelChoices[index]);
    }
}
