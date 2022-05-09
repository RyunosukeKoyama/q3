using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// クイズのスコア(不正解記録)
public class QuizScore
{
    private const string QUIZ_TYPE_KEY = "JSTQBFL:";
    private const string QUIZ_CORRECT_KEY = "CORRECT:";
    private const string QUIZ_INCORRECT_KEY = "INCORRECT:";

    public static void SaveScore(int chapter, List<int> correctIds, List<int> incorrectIds)
    {
        var currentCorrectIds = GetCorrectIds(chapter);
        var currentIncorrectIds = GetIncorrectIds(chapter);

        var updatedCorrectIds = currentCorrectIds.Union(correctIds).Except(incorrectIds);
        var updatedIncorrectIds = currentIncorrectIds.Union(incorrectIds).Except(correctIds);

        PlayerPrefs.SetString(CorrectChapterKey(chapter), string.Join(',', updatedCorrectIds));
        PlayerPrefs.SetString(IncorrectChapterKey(chapter), string.Join(',', updatedIncorrectIds));
        PlayerPrefs.Save();
    }

    public static List<int> GetCorrectIds(int chapter)
    {
        return StringToIntList(PlayerPrefs.GetString(CorrectChapterKey(chapter)));
    }

    public static List<int> GetIncorrectIds(int chapter)
    {
        return StringToIntList(PlayerPrefs.GetString(IncorrectChapterKey(chapter)));
    }

    private static string CorrectChapterKey(int chapter)
    {
        return QUIZ_TYPE_KEY + QUIZ_CORRECT_KEY + chapter.ToString();
    }

    private static string IncorrectChapterKey(int chapter)
    {
        return QUIZ_TYPE_KEY + QUIZ_INCORRECT_KEY + chapter.ToString();
    }

    private static List<int> StringToIntList(string str)
    {
        return str == "" ? new List<int>() : str.Split(',').ToList().ConvertAll(s => int.Parse(s));
    }
}