using UnityEngine;
using TMPro;
using System.Collections;

public class ModalManager : MonoSingleton<ModalManager>
{
    [SerializeField] private GameObject onlyMessagePrefab;
    [SerializeField] private GameObject withOkCancelPrefab;
    [SerializeField] private GameObject withOkPrefab;

    public void DeleteModal()
    {
        StopAllCoroutines();

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public IEnumerator GenerateLoading()
    {
        var go = Instantiate(onlyMessagePrefab, transform);
        var gui = go.GetComponentInChildren<TextMeshProUGUI>();

        gui.text = "読み込み中";
        while (true)
        {
            yield return new WaitForSeconds(1);
            gui.text += "・";
            if (gui.text.Length > 9) gui.text = "読み込み中";
        }
    }

    public IEnumerator GenerateOkCancel(string message)
    {
        //意図的なバグ: 開くまで時間がかかるモーダルで連打すると複数開いてしまう
        yield return new WaitForSeconds(0.5f);

        var go = Instantiate(withOkCancelPrefab, transform);
        var gui = go.GetComponentInChildren<TextMeshProUGUI>();

        gui.text = message;
    }

    public IEnumerator GenerateOk(string message)
    {
        var go = Instantiate(withOkPrefab, transform);
        var gui = go.GetComponentInChildren<TextMeshProUGUI>();

        gui.text = message;
        yield return null;
    }
}
