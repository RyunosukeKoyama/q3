using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class MasterLoader : MonoSingleton<MasterLoader>
{
    private const string URL = "https://script.google.com/macros/s/AKfycbzCXckJ26w-8FmboLsoMs0idYYgLdsRHJttvp8Scua5cdV9jdP1Mya0ZEp-K6U4H2Ve/exec";

    public IEnumerator GetMaster(string level)
    {
        Debug.Log("Request Start");
        UnityWebRequest webRequest = UnityWebRequest.Get(URL + "?level=" + level);
        yield return webRequest.SendWebRequest();

        Debug.Log(webRequest.result);

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

    public IEnumerator LoadQuizzes(string level)
    {
        var ie = GetMaster(level);
        var coroutine = StartCoroutine(ie);
        yield return coroutine;

        var json = (string) ie.Current;
        Debug.Log(json);
        var quizArray = JsonUtility.FromJson<QuizArray>("{\"quizzes\":" + json + "}");
        yield return quizArray.quizzes.ToList();
    }
}
