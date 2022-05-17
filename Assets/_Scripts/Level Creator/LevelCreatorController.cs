using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelCreatorController : MonoBehaviour
{
    [Header("Block Properties")]
    public TMP_InputField Row;
    public TMP_InputField StartTimeInputField;
    public Button NowButton;
    public TMP_InputField EndTimeInputField;
    public TMP_InputField SpeedInputField;
    public TMP_Dropdown TypeDropdown;
    public Toggle ReturnableToggle;
    public Button CreateButton;
    public Button UpdateButton;

    public GameObject[] BlocksPrefab;
    
    [SerializeField] private int _row;
    [SerializeField] private float _startTime;
    [SerializeField] private float _endTime;
    [SerializeField] private float _speed;
    [SerializeField] private bool _returnable;

    [Header("Music")]
    public TMP_InputField MusicTimeInput;
    public Slider MusicSlider;
    public AudioSource MusicSource;
    public float MusicTime;
    public bool MusicIsPlaying = true;

    [Header("Save level")]
    public List<BlockInfo> blockInfoList = new List<BlockInfo>();

    void Start()
    {
        OnMusicStart();
    }

    void Update()
    {
        OnMusicUpdate();
    }

    public void OnValueChange()
    {
        CalculateBlockData();
    }
    
    #region Calculate Block Properties
    // Calculate data on value change
    public void CalculateBlockData()
    {
        float startTime = (StartTimeInputField.text == "") ? -1 : float.Parse(StartTimeInputField.text);
        float endTime = (EndTimeInputField.text == "") ? -1 : float.Parse(EndTimeInputField.text);
        float speed = (SpeedInputField.text == "") ? -1 : float.Parse(SpeedInputField.text);
        bool returnable = ReturnableToggle.isOn;
        BlockController blockControllerScript = BlocksPrefab[0].GetComponent<BlockController>();
        
        if (startTime >= 0 && endTime > 0 && endTime > startTime)
        {
            print("calculate speed");
            // Calculate speed
            _startTime = startTime;
            _endTime = endTime;

            float time = endTime - startTime;

            // Calculate distance from start to end
            float distance = Vector3.Distance(blockControllerScript.StartRailPositions[_row], blockControllerScript.EndRailPositions[_row]);

            // Calculate Speed
            _speed = distance / time;
            SpeedInputField.text = _speed.ToString();


        } else
        {
            if (startTime >= 0 && speed > 0 && (endTime < startTime || endTime == -1))
            {
                print("calculate end");
                // Calculate distance from start to end
                float distance = Vector3.Distance(blockControllerScript.StartRailPositions[_row], blockControllerScript.EndRailPositions[_row]);
                
                // Calculate end time
                _startTime = startTime;
                _speed = speed;
                _endTime = startTime + (distance / speed);
                EndTimeInputField.text = _endTime.ToString();

            }
            else
            {
                if (endTime >= 0 && speed > 0 && (startTime < endTime || startTime == -1))
                {
                    print("calculate start");
                    // Calculate start time
                    _endTime = endTime;
                    _speed = speed;
                    
                    // Calculate distance from start to end
                    float distance = Vector3.Distance(blockControllerScript.StartRailPositions[_row], blockControllerScript.EndRailPositions[_row]);
                    _startTime = endTime - (distance / speed);
                    StartTimeInputField.text = _startTime.ToString();
                } else
                {
                    if (startTime >= 0)
                    {
                        _speed = 3;
                        speed = _speed;
                        print("calculate end, give speed");
                        // Calculate distance from start to end
                        float distance = Vector3.Distance(blockControllerScript.StartRailPositions[_row], blockControllerScript.EndRailPositions[_row]);

                        // Calculate end time
                        _startTime = startTime;
                        _endTime = _startTime + Mathf.Abs((distance / speed));
                        
                        EndTimeInputField.text = _endTime.ToString();
                        SpeedInputField.text = _speed.ToString();
                    } else
                    {
                        if (endTime >= 0)
                        {
                            _speed = 3;
                            print("calculate start, give speed");
                            // Calculate start time
                            _endTime = endTime;

                            // Calculate distance from start to end
                            float distance = Vector3.Distance(blockControllerScript.StartRailPositions[_row], blockControllerScript.EndRailPositions[_row]);
                            _startTime = endTime - Mathf.Abs((distance / speed));
                            StartTimeInputField.text = _startTime.ToString();
                            SpeedInputField.text = _speed.ToString();
                        } else
                        {
                            if (speed > 0)
                            {
                                print("give start, calculate end");
                                _startTime = MusicSource.time;
                                startTime = _startTime;
                                _speed = speed;
                                
                                // Calculate distance from start to end
                                float distance = Vector3.Distance(blockControllerScript.StartRailPositions[_row], blockControllerScript.EndRailPositions[_row]);
                                _endTime = startTime + Mathf.Abs((distance / speed));

                                EndTimeInputField.text = _endTime.ToString();
                                StartTimeInputField.text = startTime.ToString();
                                SpeedInputField.text = _speed.ToString();
                            } else
                            {
                                print("Give everything");
                                _startTime = MusicSource.time;
                                startTime = _startTime;
                                _speed = 3;

                                // Calculate distance from start to end
                                float distance = Vector3.Distance(blockControllerScript.StartRailPositions[_row], blockControllerScript.EndRailPositions[_row]);
                                _endTime = startTime + Mathf.Abs((distance / speed));

                                EndTimeInputField.text = _endTime.ToString();
                                StartTimeInputField.text = startTime.ToString();
                                SpeedInputField.text = _speed.ToString();
                            }
                        }
                    }
                }
            }
        }
    }


    // When click now button set start time to current time
    public void UpdateStartTime()
    {
        _startTime = MusicSource.time;
        StartTimeInputField.text = _startTime.ToString();
    }
    #endregion

    #region Top Music Slider

    private void OnMusicStart()
    {
        // Set slider values depending on the music length
        MusicSlider.minValue = 0;
        MusicSlider.maxValue = MusicSource.clip.length - 0.01f;
    }
    
    // Function in the update (Only music slider stuff)
    private void OnMusicUpdate()
    {
        UpdateMusic();
    }
    
    public void UpdateMusic()
    {
        // Update slider value and text
        if (MusicIsPlaying)
        {
            MusicTime = MusicSource.time;
            MusicSlider.value = MusicTime;
            MusicTimeInput.text = MusicTime.ToString();       
        }
    }

    // When the music time value is changed
    public void OnMusicValChange()
    {
        // Update slider place and time in music
        CultureInfo culture = CultureInfo.GetCultureInfo("en-US");
        float time;
        if (float.Parse(MusicTimeInput.text) > MusicSource.clip.length)
        {
            time = MusicSlider.maxValue;
            MusicTimeInput.text = time.ToString();
        }
        else
        {
            time = float.Parse(MusicTimeInput.text, culture);
        }
        MusicSource.time = time;
        MusicTime = MusicSource.time;
    }

    // When the slider value is changed
    public void OnMusicSliderChange()
    {
        // Update the val and music time
        MusicSource.time = MusicSlider.value;
        MusicTime = MusicSource.time;
    }

    public void PauseMusic()
    {
        MusicSource.Pause();
    }

    public void PlayMusic()
    {
        MusicSource.Play();
    }

    #endregion

    #region Save Blocks
    public void SortBlockList()
    {
        // Sort blockInfoList by start time
        blockInfoList.Sort((x, y) => x.StartTime.CompareTo(y.StartTime));

        // Bubble sort blockInfoList by start time
        for (int i = 0; i < blockInfoList.Count; i++)
        {
            for (int j = 0; j < blockInfoList.Count - 1; j++)
            {
                if (blockInfoList[j].StartTime > blockInfoList[j + 1].StartTime)
                {
                    BlockInfo temp = blockInfoList[j];
                    blockInfoList[j] = blockInfoList[j + 1];
                    blockInfoList[j + 1] = temp;
                }
            }
        }
    }

    #endregion
}

[System.Serializable]
public class BlockInfo
{
    public BlockInfo(int _row, float _startTime, float _endTime, float _speed)
    {
        Row = _row;
        StartTime = _startTime;
        EndTime = _endTime;
        Speed = _speed;
    }

    public GameObject BlockPrefab;

    public int Row;
    public float StartTime;
    public float EndTime;
    public float Speed;
    public bool Returnable;
}