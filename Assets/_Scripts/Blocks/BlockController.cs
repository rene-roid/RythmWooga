using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    [Header("Block Settings")]
    
    
    [Header("Positions")]
    public int Rail;
    private Vector3[] StartRailPositions;
    private Vector3[] EndRailPositions;

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
        
    }

    void Update()
    {
        
    }
}
