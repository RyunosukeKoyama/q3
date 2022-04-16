using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuizParam
{
    private const string QUANTITY_KEY = "QuizQuantity";
    public static readonly List<int> QuantityChoices = new List<int> { 10, 20, 30, 40 };

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
}
