using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public GameObject LevelHolder;
    public GameObject LevelIcon;
    public GameObject ThisCanvas;
    public int NumberOfLevels;
    private Rect PanelDimensions;
    private Rect IconDimensions;
    private int AmmountPerPage;
    private int CurrentLevelCount;

    void Start()
    {
        Rect panelDimensions = LevelHolder.GetComponent<RectTransform>().rect;
        IconDimensions = LevelIcon.GetComponent<RectTransform>().rect;

        int maxInARow = Mathf.FloorToInt(panelDimensions.width / IconDimensions.width);
        int maxInAColumn = Mathf.FloorToInt(panelDimensions.height / IconDimensions.height);
        AmmountPerPage = maxInARow * maxInAColumn;
        int totalPages = Mathf.CeilToInt((float)NumberOfLevels / AmmountPerPage);
        LoadPanels(totalPages);

    }

    private void LoadPanels(int numberOfPanels)
    {
        GameObject panelClone = Instantiate(LevelHolder) as GameObject;

        for (int i = 0; i <= numberOfPanels; i++)
        {
            GameObject panel = Instantiate(panelClone) as GameObject;
            panel.transform.SetParent(ThisCanvas.transform, false);
            panel.transform.SetParent(LevelHolder.transform);
            panel.name = "Page-" + i;
            panel.GetComponent<RectTransform>().localPosition = new Vector2(PanelDimensions.width * (i - 1), 0);
            SetUpGrid(panel);
            int numberOfIcons = i== CurrentLevelCount ? NumberOfLevels - CurrentLevelCount : AmmountPerPage;
            LoadIcons(numberOfIcons, panel);
        }
        Destroy(panelClone);
    }
    
    private void SetUpGrid(GameObject panel)
    {
        GridLayoutGroup grid = panel.AddComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(IconDimensions.width, IconDimensions.height);
        grid.childAlignment = TextAnchor.MiddleCenter;
        
    }

    private void LoadIcons(int numberOfIcons, GameObject parentObject)
    {
        for (int i = 0; i <= numberOfIcons; i++)
        {
            CurrentLevelCount++;
            GameObject icon = Instantiate(LevelIcon) as GameObject;
            icon.transform.SetParent(ThisCanvas.transform, false);
            icon.transform.SetParent(parentObject.transform);
            icon.name = "Level-" + i;
        }
    }

    void Update()
    {
        
    }
}
