using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    [Header("Block Settings")]
    public SpriteRenderer Sprite;
    public int ID;
    
    [Header("Positions")]
    [Range(0, 4)] public int Rail; // Set what rail the block is on
    public Vector3[] StartRailPositions = new Vector3[5]; // Positions
    public Vector3[] EndRailPositions = new Vector3[5];

    [Header("Stats")]
    public bool IsReturnable = false; // Is the block returnable
    [SerializeField] private bool isReturning = false; // Is the block returning

    public bool IsJumpable = false;
    
    public float Speed; // Speed

    [Header("Animations")]
    public bool HasParticles = true;
    public bool AreParticlesOn = true;
    public GameObject particles;

    [Header("Math")]
    public float TotalDistance; // Total distance between start and end positions
    public float CurrentDistance; // Current distance between current position and end position
    public float TimeToFullTravel; // Time to travel the full distance
    public float TimeToEnd; // Time left to reach the end position

    [Header("Other")]
    public bool IsActive = true;
    public bool CanMove = true;

    #region Unity Functions
    void Start()
    {
        OnStart();
    }

    void Update()
    {
        if (!IsActive) return;
        OnUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTrigger(other);
    }

    public void OnStart()
    {
        // Set starting point
        transform.position = StartRailPositions[Rail];

        // Setting particle systems
        if (HasParticles) if (AreParticlesOn) particles.SetActive(true);

    }

    public void OnUpdate()
    {
        if (!IsActive) return;
        Movement();
        MathCalc();
    }

    public void OnTrigger(Collider other)
    {
        DetectHit(other);
    }

    #endregion

    #region Movement
    public void Movement()
    {
        if (!CanMove) return;
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
            Destroy(gameObject);
        }
    }

    // Moving the block linearly towards the end position
    // Movement based on speed
    public void DefaultMovement()
    {
        // Move block
        transform.position = Vector3.MoveTowards(transform.position, EndRailPositions[Rail], Speed * Time.deltaTime);
    }

    private void DetectHit(Collider other)
    {
        if (other.tag == "Player")
        {
            // Detect if the player is attacking
            if (other.GetComponent<AttackController>().isAttacking)
            {
                isReturning = true;
            }
        }
    }
    
    public void ReturnMovement()
    {
        // Move block
        transform.position = Vector3.MoveTowards(transform.position, StartRailPositions[Rail], Speed * 4 * Time.deltaTime);
        if (transform.position == StartRailPositions[Rail])
        {
            Destroy(gameObject);
            // Shake the camera during .1 seconds
            Camera.main.GetComponent<CameraController>().Shake(.1f);
        }
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
