using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorWaveColor : MonoBehaviour
{
    public MeshRenderer[] PlatformRows;
    void Start()
    {
        
    }

    void Update()
    {
        WhiteWaveRows();
    }

    private IEnumerator WhiteWaveRows()
    {
        // Lerp the material color of the rows to white in order with a .1 second delay between each
        for (int i = 0; i < PlatformRows.Length; i++)
        {
            PlatformRows[i].material.color = Color.Lerp(PlatformRows[i].material.color, Color.white, .1f);
            yield return new WaitForSeconds(.1f);
        }
    }

    private IEnumerator BlackWaveRows()
    {
        // Lerp the material color of the rows to white in order with a .1 second delay between each
        for (int i = 0; i < PlatformRows.Length; i++)
        {
            PlatformRows[i].material.color = Color.Lerp(PlatformRows[i].material.color, Color.black, .1f);
            yield return new WaitForSeconds(.1f);
        }
    }
}
