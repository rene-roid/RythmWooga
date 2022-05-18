using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelCreatorController : MonoBehaviour
{
    [Header("Block Properties")]
    public TMP_Text BlockName;
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
    public List<BlockInfo> saveProperties = new List<BlockInfo>();

    [Header("Load level")]
    [SerializeField] private float _lastMusicTime;
    public List<GameObject> currentBlocks = new List<GameObject>();
    public List<BlockInfo> currentBlockInfo = new List<BlockInfo>();

    [Header("UI")]
    public int CurrentID = -1;

    void Start()
    {
        OnMusicStart();
    }

    void Update()
    {
        OnMusicUpdate();
        OnUpdateLoadBlocks();
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
        int row = (Row.text == "") ? 3 : int.Parse(Row.text);


        _row = Mathf.Clamp(row - 1, 0, 4);
        Row.text = (_row + 1).ToString();

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

        //Row.text = "";
        //StartTimeInputField.text = "";
        //EndTimeInputField.text = "";
        //SpeedInputField.text = "";
        
    }


    // When click now button set start time to current time
    public void UpdateStartTime()
    {
        if (MusicSource.isPlaying)
        {
            MusicSource.Pause();
            _startTime = MusicSource.time;
            StartTimeInputField.text = _startTime.ToString();
            MusicSource.Play();   
        } else
        {
            _startTime = MusicSource.time;
            StartTimeInputField.text = _startTime.ToString();
        }
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
    private void OnStartSave()
    {
        
    }
    
    public void AddBlockToSave()
    {
        BlockInfo blockInfo = new BlockInfo();
        blockInfo.ID = saveProperties.Count;

        blockInfo.StartTime = _startTime;
        blockInfo.EndTime = _endTime;
        blockInfo.Speed = _speed;
        blockInfo.Row = _row;
        blockInfo.Returnable = ReturnableToggle.isOn;
        blockInfo.BlockPrefab = TypeDropdown.value;

        CurrentID = blockInfo.ID;
        BlockName.text = "Block ID: " + CurrentID;

        saveProperties.Add(blockInfo);
        SortBlockList();
    }
    
    private void SortBlockList()
    {
        // saveProperties by start time
        saveProperties.Sort((x, y) => x.StartTime.CompareTo(y.StartTime));
    }

    #endregion

    #region Load Blocks
    private void OnUpdateLoadBlocks()
    {
        CreateBlocks();
        UpdateBlockPosition();
        DeleteBlocksController();
    }

    private void CreateBlocks()
    {
        // Load blocks at the position
        for (int i = 0; i < saveProperties.Count; i++)
        {
            if (currentBlocks.Count == 0)
            {
                if (saveProperties[i].StartTime <= MusicTime && saveProperties[i].EndTime > MusicTime)
                {
                    GameObject newBlock = Instantiate(BlocksPrefab[saveProperties[i].BlockPrefab]);
                    newBlock.GetComponent<BlockController>().ID = saveProperties[i].ID;
                    newBlock.GetComponent<BlockController>().CanMove = false;
                    // Tiempo final, posicion final, velocidad de la nota y tiempo actual
                    Vector3 endPos = newBlock.GetComponent<BlockController>().EndRailPositions[saveProperties[i].Row];

                    // Caluclate direction with start rail pos and end rail pos
                    Vector3 direction = endPos - newBlock.GetComponent<BlockController>().StartRailPositions[saveProperties[i].Row];

                    endPos += saveProperties[i].Speed * direction.normalized * (MusicTime - saveProperties[i].EndTime);

                    // Set block position
                    //StartCoroutine(SetBlockPositionOnStart(endPos, newBlock));

                    BlockInfo blockInfo = new BlockInfo();
                    blockInfo.ID = saveProperties[i].ID;
                    blockInfo.StartTime = saveProperties[i].StartTime;
                    blockInfo.EndTime = saveProperties[i].EndTime;
                    blockInfo.Speed = saveProperties[i].Speed;
                    blockInfo.Row = saveProperties[i].Row;
                    blockInfo.Returnable = saveProperties[i].Returnable;
                    blockInfo.BlockPrefab = saveProperties[i].BlockPrefab;

                    currentBlockInfo.Add(blockInfo);
                    currentBlocks.Add(newBlock);
                }
            } else
            {
                if (saveProperties[i].StartTime <= MusicTime && saveProperties[i].EndTime > MusicTime)
                {
                    bool doesExist = false;
                    for (int j = 0; j < currentBlocks.Count; j++)
                    {
                        if (saveProperties[i].ID == currentBlockInfo[j].ID) doesExist = true;
                    }

                    if (!doesExist)
                    {
                        GameObject newBlock = Instantiate(BlocksPrefab[saveProperties[i].BlockPrefab]);
                        newBlock.GetComponent<BlockController>().ID = saveProperties[i].ID;
                        newBlock.GetComponent<BlockController>().CanMove = false;
                        // Tiempo final, posicion final, velocidad de la nota y tiempo actual
                        Vector3 endPos = newBlock.GetComponent<BlockController>().EndRailPositions[saveProperties[i].Row];

                        // Caluclate direction with start rail pos and end rail pos
                        Vector3 direction = endPos - newBlock.GetComponent<BlockController>().StartRailPositions[saveProperties[i].Row];

                        endPos += saveProperties[i].Speed * direction.normalized * (MusicTime - saveProperties[i].EndTime);

                        // Set block position
                        //StartCoroutine(SetBlockPositionOnStart(endPos, newBlock));

                        BlockInfo blockInfo = new BlockInfo();
                        blockInfo.ID = saveProperties[i].ID;
                        blockInfo.StartTime = saveProperties[i].StartTime;
                        blockInfo.EndTime = saveProperties[i].EndTime;
                        blockInfo.Speed = saveProperties[i].Speed;
                        blockInfo.Row = saveProperties[i].Row;
                        blockInfo.Returnable = saveProperties[i].Returnable;
                        blockInfo.BlockPrefab = saveProperties[i].BlockPrefab;

                        currentBlockInfo.Add(blockInfo);
                        currentBlocks.Add(newBlock);
                    }
                }

            }

        }
    }

    private void UpdateBlockPosition()
    {
        for (int i = 0; i < currentBlocks.Count; i++)
        {
            if (currentBlocks[i].GetComponent<BlockController>().IsActive)
            {
                StartCoroutine(SetBlockPositionOnStart(currentBlocks[i].GetComponent<BlockController>().EndRailPositions[saveProperties[i].Row], currentBlocks[i].gameObject));
                currentBlocks[i].GetComponent<BlockController>().IsActive = false;
            }
            // Tiempo final, posicion final, velocidad de la nota y tiempo actual
            Vector3 endPos = currentBlocks[i].GetComponent<BlockController>().EndRailPositions[saveProperties[i].Row];

            // Caluclate direction with start rail pos and end rail pos
            Vector3 direction = endPos - currentBlocks[i].GetComponent<BlockController>().StartRailPositions[saveProperties[i].Row];
            endPos += saveProperties[i].Speed * direction.normalized * (MusicTime - saveProperties[i].EndTime);


            currentBlocks[i].transform.position = endPos;
        }
    }

    private void DeleteBlocksController()
    {
        for (int i = 0; i < currentBlocks.Count; i++)
        {
            if (currentBlockInfo[i].StartTime > MusicTime || currentBlockInfo[i].EndTime < MusicTime)
            {
                Destroy(currentBlocks[i]);
                currentBlocks.Remove(currentBlocks[i]);
                currentBlockInfo.Remove(currentBlockInfo[i]);
            }
        }
    }

    private IEnumerator SetBlockPositionOnStart(Vector3 position, GameObject gameObject)
    {
        yield return new WaitForEndOfFrame();
        gameObject.transform.position = position;
    }

    #endregion

    #region Blocks UI
    public void DeleteBlockButton()
    {
        if (CurrentID != -1)
        {
            for (int i = 0; i < currentBlockInfo.Count; i++)
            {
                if (currentBlockInfo[i].ID == CurrentID)
                {
                    Destroy(currentBlocks[CurrentID]);
                    currentBlocks.RemoveAt(CurrentID);
                    currentBlockInfo.RemoveAt(CurrentID);
                    
                    break;
                }
            }
            
            for (int i = 0; i < saveProperties.Count; i++)
            {
                if (saveProperties[i].ID == CurrentID)
                {
                    saveProperties.Remove(saveProperties[CurrentID]);
                    break;
                }
            }
            BlockName.text = "Block ID: " + CurrentID;
            CurrentID = -1;
        }
    }

    public void EditSpecificBlock(int id)
    {
        // Open Edit UI

        // Set current ID
        CurrentID = id;

        // Set UI values
        BlockName.text = "Block ID: " + saveProperties[id].ID;
        StartTimeInputField.text = "Start Time: " + saveProperties[CurrentID].StartTime;
        EndTimeInputField.text = "End Time: " + saveProperties[CurrentID].EndTime;
        SpeedInputField.text = "Speed: " + saveProperties[CurrentID].Speed;
        Row.text = "Row: " + saveProperties[CurrentID].Row;
        ReturnableToggle.isOn = saveProperties[CurrentID].Returnable;
        TypeDropdown.value = saveProperties[CurrentID].BlockPrefab;
    }

    public void UpdateSpecificBlock(int id)
    {
        
    }

    #endregion
}

[System.Serializable]
public class BlockInfo
{
    public int ID;
    public int BlockPrefab;

    public int Row;
    public float StartTime;
    public float EndTime;
    public float Speed;
    public bool Returnable;

    public BlockInfo()
    {
        
    }

    public BlockInfo(int _ID, int _row, float _startTime, float _endTime, float _speed, bool _returnable, int _blockPrefab)
    {
        ID = _ID;
        Row = _row;
        StartTime = _startTime;
        EndTime = _endTime;
        Speed = _speed;
        Returnable = _returnable;
        BlockPrefab = _blockPrefab;
    }
}