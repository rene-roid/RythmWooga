using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    [Header("Block Settings")]
    public SpriteRenderer Sprite;
    
    [Header("Positions")]
    [Range(0, 4)] public int Rail; // Set what rail the block is on
    public Vector3[] StartRailPositions; // Positions
    public Vector3[] EndRailPositions;

    [Header("Stats")]
    public bool IsReturnable = false; // Is the block returnable
    [SerializeField] private bool isReturning = false; // Is the block returning
    
    public float Speed; // Speed

    [Header("Animations")]
    public bool hasParticles = true;

    [Header("Math")]
    public float TotalDistance; // Total distance between start and end positions
    public float CurrentDistance; // Current distance between current position and end position
    public float TimeToFullTravel; // Time to travel the full distance
    public float TimeToEnd; // Time left to reach the end position

    #region Unity Functions
    void Start()
    {
        OnStart();
    }

    void Update()
    {
        OnUpdate();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnTrigger(other);
    }

    public void OnStart()
    {
        // Set starting point
        transform.position = StartRailPositions[Rail];
    }

    public void OnUpdate()
    {
        Movement();
        MathCalc();
    }

    public void OnTrigger(Collider2D other)
    {
        DetectHit(other);
    }

    #endregion

    #region Movement
    public void Movement()
    {
        if (IsReturnable)
        {
            if (isReturning)
            {
                ReturnMovement();
            } else
            {
                DefaultMovement();
            }
        } else
        {
            DefaultMovement();
        }

        if (transform.position == EndRailPositions[Rail])
        {
            // End Anim

        }
    }

    // Moving the block linearly towards the end position
    // Movement based on speed
    public void DefaultMovement()
    {
        // Move block
        transform.position = Vector3.MoveTowards(transform.position, EndRailPositions[Rail], Speed * Time.deltaTime);
    }

    private void DetectHit(Collider2D other)
    {
        print("VULBA");
        if (other.tag == "Player")
        {
            print("PENE");
            // Detect if the player is attacking
            if (other.GetComponent<AttackController>().isAttacking)
            {
                isReturning = true;
                print("PITO CACA CULO");
            }
        }
    }
    
    public void ReturnMovement()
    {
        // Move block
        transform.position = Vector3.MoveTowards(transform.position, StartRailPositions[Rail], Speed * 4 * Time.deltaTime);
    }

    #endregion
    
    public void MathCalc()
    {
        // Calculate distance from start to end
        TotalDistance = Vector3.Distance(StartRailPositions[Rail], EndRailPositions[Rail]);

        // Calculate CurrentDistance to end
        CurrentDistance = Vector3.Distance(transform.position, EndRailPositions[Rail]);

        // Calculate time to full travel
        TimeToFullTravel = TotalDistance / Speed;

        // Calculate time to end
        TimeToEnd = CurrentDistance / Speed;

    }
}
