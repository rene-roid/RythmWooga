using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Header("Timer")]
    public Text TextTimer;

    [Header("Random Block Spawn")]
    public GameObject Block;
    public KeyCode spawnkey = KeyCode.E;

    void Start()
    {
        
    }

    private void Update()
    {
        SpawnBlock();
    }

    void FixedUpdate()
    {
        TextTimer.text = "Time: " + Mathf.Round(Time.time * 100.0f) * 0.01f;
    }

    // Spawn a random block
    private void SpawnBlock()
    {
        if (Input.GetKeyDown(spawnkey))
        {
            GameObject newBlock = Instantiate(Block);
            BlockController blockController = newBlock.GetComponent<BlockController>();
            blockController.Rail = Random.Range(0, 4);
        }
        
        
    }
}
