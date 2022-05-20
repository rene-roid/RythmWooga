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
    public GameObject Block2;
    public KeyCode spawnkey = KeyCode.E;
    public KeyCode spawnkey2 = KeyCode.Q;

    public bool Spawn = true;
    public float timer = 0;
    
    void Start()
    {
        
    }

    private void Update()
    {
        SpawnBlock();

        // Instantiate wall every 0.5 seconds
        if (timer < 0.15f)
        {
            timer += Time.deltaTime;
        }
        else
        {
            GameObject block = Instantiate(Block2);
            block.GetComponent<BlockController>().Rail = Random.Range(0, 5);
            timer = 0;
        }
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

        if (Input.GetKeyDown(spawnkey2))
        {
            GameObject newBlock = Instantiate(Block2);
            BlockController blockController = newBlock.GetComponent<BlockController>();
            blockController.Rail = Random.Range(0, 4);
        }
    }
}
