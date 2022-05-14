using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Header("Timer")]
    public Text TextTimer;
    
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        TextTimer.text = "Time: " + Mathf.Round(Time.time * 100.0f) * 0.01f;
    }
}
