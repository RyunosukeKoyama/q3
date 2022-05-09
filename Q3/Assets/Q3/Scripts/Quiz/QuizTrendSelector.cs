using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Dropdown))]
public class QuizTrendSelector : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    private void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(new List<string>{"ランダム","不正解を優先"});
        dropdown.value = QuizParam.GetTrend() == true ? 0 : 1;
    }

    public void OnClicked(int index)
    {
        QuizParam.SetTrend(index);
    }
}
