using System.Collections.Generic;
using UnityEngine;

public class Modal : MonoBehaviour
{
    public void ReturnHome()
    {
        StartCoroutine(ModalManager.I.GenerateOkCancel("ホームに\n戻りますか？"));
    }

    public void ShowScore()
    {
        var scoreMsg = new List<string>();
        scoreMsg.Add($"<b>章モード\t総問題数\t正解数\t不正解数\t正答率</b>");
        scoreMsg.Add($"<b>------\t------\t------\t------\t------</b>");
        var chapters = QuizParam.ChapterChoices;
        foreach (var chapter in chapters)
        {
            var allNum = QuizScore.GetAllIds(chapter).Count;
            if(allNum == 0) 
            {
                scoreMsg.Add($"{chapter}\t未挑戦");
                continue;
            }
            var correctNum = QuizScore.GetCorrectIds(chapter).Count;
            var incorrectNum = QuizScore.GetIncorrectIds(chapter).Count;
            var percentage = ((float)correctNum/allNum).ToString("P1");
            scoreMsg.Add($"{(chapter == 0 ? "全章" : chapter)}\t{allNum}\t{correctNum}\t{incorrectNum}\t{percentage}");
        }
        StartCoroutine(ModalManager.I.GenerateOk(string.Join("\n", scoreMsg)));
    }

    public void CloseModal()
    {
        Destroy(gameObject);
    }
}
