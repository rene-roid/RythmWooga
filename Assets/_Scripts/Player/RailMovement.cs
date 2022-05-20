using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailMovement : MonoBehaviour
{
    [Header("Rail Movement")]
    public Vector3[] Positions;
    
    public float TimeMove; // Minimum time between movement
    public float TimeToMove; // How long it takes the movement

    public float JumpTime;
    public bool IsGrounded = true;

    [Header("Animations")]
    public Animator PlayerAnimator;
    public SpriteRenderer PlayerSpriteRenderer;

    // Private variables
    [Header("Private variables")]
    [SerializeField] private int _indexPosition;
    [SerializeField] private int _lastIndex;

    [SerializeField] private float _clock;

    private AttackController _attackController;
    // Buffer input
    //[SerializeField] private int _bufferInput = 0;

    void Start()
    {
        _attackController = GetComponent<AttackController>();

        transform.position = Positions[_indexPosition];
        PlayerAnimator = GetComponent<Animator>();
        PlayerSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Input
        InputController();

        // Movement
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) && _clock > TimeMove)
        {
            Move();
            StopJump();
            this.gameObject.GetComponent<GhostSpriteSpawner>().SpawnGhost();
        }

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && IsGrounded && !_attackController.isAttacking)
        {
            // Jump
            Jump();
        }

        _clock += Time.deltaTime;
    }

    private void InputController()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            _lastIndex = _indexPosition;
            _indexPosition -= 1;
            PlayerSpriteRenderer.flipX = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            _lastIndex = _indexPosition;
            _indexPosition += 1;
            PlayerSpriteRenderer.flipX = false;
        }

        // Check if the index is out of range
        _indexPosition = Mathf.Clamp(_indexPosition, 0, Positions.Length - 1);
    }

    private void Move()
    {
        if (transform.position != Positions[_lastIndex]) transform.position = Positions[_lastIndex];
        
        StopCoroutine("Movement");
        StartCoroutine("Movement", Positions);

        _clock = 0;
    }

    private IEnumerator Movement(Vector3[] _target)
    {
        PlayerAnimator.SetTrigger("Dash");
        Vector3 curr = transform.position;
        Vector3 initPos = curr;

        float timer = 0;

        // While player is not at the target position
        while (curr != _target[_indexPosition])
        {
            // Lerp position
            curr = Vector3.Lerp(initPos, _target[_indexPosition], timer / TimeToMove);

            // Update timer
            timer += Time.deltaTime;

            // Update position
            transform.position = curr;

            yield return null;
        }
    }

    private void Jump()
    {
        StopCoroutine("PlayerJump");
        StartCoroutine("PlayerJump");
    }

    private IEnumerator PlayerJump()
    {
        IsGrounded = false;

        this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(JumpTime);
        StopJump();
    }

    private void StopJump()
    {
        IsGrounded = true;
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
