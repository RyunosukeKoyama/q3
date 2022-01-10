using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modal : MonoBehaviour
{
    public void OpenOkCancelModal()
    {
        StartCoroutine(ModalManager.I.GenerateOkCancel("ホームに\n戻りますか？"));
    }

    public void CloseModal()
    {
        Destroy(gameObject);
    }
}
