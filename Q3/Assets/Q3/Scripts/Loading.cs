using UnityEngine;
using TMPro;
using System.Collections;

public class Loading : MonoSingleton<Loading>
{
    public IEnumerator ShowLoading()
    {
        gameObject.SetActive(true);
        var gui = GetComponentInChildren<TextMeshProUGUI>();
        gui.text = "読み込み中";
        while (true)
        {
            yield return new WaitForSeconds(1);
            gui.text += "・";
            if (gui.text.Length > 9) gui.text = "読み込み中";
        }
    }
}
