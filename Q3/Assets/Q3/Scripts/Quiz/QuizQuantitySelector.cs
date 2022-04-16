using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Dropdown))]
public class QuizQuantitySelector : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    private void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(QuizParam.QuantityChoices.ConvertAll(i => i.ToString()));
        dropdown.value = QuizParam.QuantityChoices.IndexOf(QuizParam.GetQuantity());
    }

    public void OnClicked(int index)
    {
        QuizParam.SetQuantity(int.Parse(dropdown.options[index].text));
    }
}
