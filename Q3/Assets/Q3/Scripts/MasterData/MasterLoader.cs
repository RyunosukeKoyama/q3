using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class MasterLoader : MonoSingleton<MasterLoader>
{
    private const string URL = "https://script.google.com/macros/s/AKfycbxyMJtn9rQjNfp3Am4NEL2lS5GdXoUPfkUJRHAdUEOdvCp25bweKo2Zj5R3h9IKmbYI/exec";

    public IEnumerator GetMaster()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(URL);
        yield return webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.Success:
                yield return webRequest.downloadHandler.text;
                break;
            default:
                Debug.LogError(webRequest.error);
                break;

        }
    }

    public IEnumerator LoadQuizzes()
    {
        var ie = GetMaster();
        var coroutine = StartCoroutine(ie);
        yield return coroutine;

        var json = (string)ie.Current;
        var quizArray = JsonUtility.FromJson<QuizArray>("{\"quizzes\":" + json + "}");
        yield return quizArray.quizzes.ToList();
    }
}
