using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuizParam
{
    private const string QUANTITY_KEY = "QuizQuantity";
    public static readonly List<int> QuantityChoices = new List<int> { 10, 20, 30, 40 };

    private const string CHAPTER_KEY = "QuizChapter";
    public static readonly List<int> ChapterChoices = new List<int> { 0, 1, 2, 3, 4, 5, 6 };

    private const string TREND_KEY = "QuizTrend";

    public static void SetQuantity(int quantity)
    {
        PlayerPrefs.SetInt(QUANTITY_KEY, quantity);
        PlayerPrefs.Save();
    }

    public static int GetQuantity()
    {
        var quantity = PlayerPrefs.GetInt(QUANTITY_KEY);
        return quantity == default ? QuantityChoices.First() : quantity;
    }

    public static void SetChapter(int chapter)
    {
        PlayerPrefs.SetInt(CHAPTER_KEY, chapter);
        PlayerPrefs.Save();
    }

    public static int GetChapter()
    {
        var chapter = PlayerPrefs.GetInt(CHAPTER_KEY);
        return chapter == default ? ChapterChoices.First() : chapter;
    }

    public static void SetTrend(int trend)
    {
        PlayerPrefs.SetInt(TREND_KEY, trend);
        PlayerPrefs.Save();
    }

    // 実質 isRandom?
    public static bool GetTrend()
    {
        return PlayerPrefs.GetInt(TREND_KEY) == 0; //0=random, 1=sort
    }
}
