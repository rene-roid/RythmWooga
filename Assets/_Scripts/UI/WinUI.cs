using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinUI : MonoBehaviour
{
    public GameObject winUI;

    private void Awake()
    {
        winUI.SetActive(false);
    }

    public void OnWin()
    {
        winUI.SetActive(true);
    }
}
