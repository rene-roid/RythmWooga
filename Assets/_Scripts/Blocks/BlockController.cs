using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    [Header("Block Settings")]
    
    
    [Header("Positions")]
    [Range(0, 4)] public int Rail;
    public Vector3[] StartRailPositions;
    public Vector3[] EndRailPositions;

    [Header("Stats")]
    public float Speed;
    public enum BlockType
    {
        Wall,
        Block,
        BreakableWall,
        BreakableBlock,
    };
    public BlockType ItemType;

    [Header("Animations")]
    public bool hasParticles = true;


    void Start()
    {
        // Set starting point
        transform.position = StartRailPositions[Rail];
    }

    void Update()
    {
        if (transform.position == EndRailPositions[Rail]) return;
        Movement();
    }

    private void Movement()
    {
        // Move block
        transform.position = Vector3.MoveTowards(transform.position, EndRailPositions[Rail], Speed * Time.deltaTime);
    }
}
