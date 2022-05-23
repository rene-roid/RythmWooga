using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RotatingTriangle : MonoBehaviour
{
    [Header("Rotating Triangle")]
    public float BaseSpeed = 20f;
    public float Angle = 0f;

    public static float LevelSelected = -1;
    public GameObject[] Levels;

    [Header("Objects")]
    public GameObject Triangle;
    public TMP_Text TextAngle;
    public TMP_Text TextLevel;

    [Header("Private Vars")]
    [SerializeField] private float _speed;
    [SerializeField] private float _direction;
    [SerializeField] private float _lastLevel;
    [SerializeField] private float _targetLevel = -1;

    void Start()
    {
        _speed = BaseSpeed;
    }

    void Update()
    {
        DetectLevelAngle();
        ChangeLevel();
        MoveTriangleToTarget();
        SelectLevel();
    }

    // Rotate triangle
    void FixedUpdate()
    {
        RotateTriangle();
    }

    private void RotateTriangle()
    {
        Angle += _speed * Time.deltaTime;
        Triangle.transform.rotation = Quaternion.Euler(0f, 0f, Angle);

        if (Angle >= 360f)
        {
            Angle = 0f;
        }

        if (Angle <= -0.1f)
        {
            Angle = 360f;
        }

        for (int i = 0; i < Levels.Length; i++)
        {
            Levels[i].transform.localRotation = Quaternion.Euler(0f, 0f, Angle * -1);
        }
    }

    // Detect user input to call the change level
    private void ChangeLevel()
    {
        // Go to left
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // 1
            GetNextLevel(1);
        }

        // Go to right
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            // -1
            GetNextLevel(-1);
        }
    }

    // Calculating the next level and changing triangle direction depending on the user input
    private void GetNextLevel(int direction)
    {
        if (direction == -1) _speed = -Mathf.Abs(_speed);
        else _speed = Mathf.Abs(_speed);

        _direction = direction;
        _targetLevel = _lastLevel + direction;

        // If the target level gets out of bounds, set to the other corner
        if (_targetLevel <= 0)
        {
            _targetLevel = 3;
        }
        else if (_targetLevel >= 4)
        {
            _targetLevel = 1;
        }
    }

    private void MoveTriangleToTarget()
    {
        if (_targetLevel == -1) return;
        
        if (LevelSelected != _targetLevel && Mathf.Abs(_speed) == BaseSpeed)
        {
            _speed *= 10;
        }
        else if (LevelSelected == _targetLevel && _speed != BaseSpeed)
        {
            _speed = BaseSpeed * _direction;
            _targetLevel = -1;
        }
    }

    // Detects what level is selected
    public void DetectLevelAngle()
    {
        // From Level 3 to 1 (Detecting whitch level is pointing to depending on the angle)
        if (330f <= Angle || Angle <= 30f)
        {
            LevelSelected = 1;
        } else if (Angle >= 90f && Angle <= 150f)
        {
            LevelSelected = 2;
        } else if (Angle >= 210f && Angle <= 270f)
        {
            LevelSelected = 3;
        } else
        {
            LevelSelected = -1;
        }

        // Get the last level selected or the actual one
        if (LevelSelected != -1) _lastLevel = LevelSelected;

        TextAngle.text = Angle.ToString("0.00");
        TextLevel.text = "Level " + LevelSelected;
    }

    // "Select" Level
    public void SelectLevel()
    {
        // If level selected  is the same as any of the levels, scale the gameobject to 1.5
        for (int i = 0; i < Levels.Length; i++)
        {
            if (LevelSelected == i + 1)
            {
                Levels[i].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }
            else
            {
                Levels[i].transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }
}
