using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncMaterial : AudioSyncer
{
    [Header("Color values")]
    public Color[] BeatColors;
    public Color RestColor = Color.white;

    public Material ChanginMaterial;

    // Private variables
    private int index = 0;


    private void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        ChanginMaterial = meshRenderer.material;

        // Get BeatColors array and shuffle it
        ShuffleArray(BeatColors);

    }

    // ShuffleArray
    public void ShuffleArray(Color[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            Color tmp = array[i];
            int r = Random.Range(i, array.Length);
            array[i] = array[r];
            array[r] = tmp;
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (IsBeat) return;

        // Change color to rest color
        ChanginMaterial.color = Color.Lerp(ChanginMaterial.color, RestColor, RestTime * Time.deltaTime);

    }

    public override void OnBeat()
    {
        base.OnBeat();

        StopCoroutine("ChangeColor");
        StartCoroutine("ChangeColor", BeatColors);
    }

    private IEnumerator ChangeColor(Color[] colors)
    {
        Color curr = ChanginMaterial.color;
        Color initColor = curr;

        float timer = 0;

        // Change color to beat color while the sprite isnt at the beat color
        while (curr != colors[index])
        {
            // Calculate color lerp
            curr = Color.Lerp(initColor, colors[index], timer / TimeToBeat);

            // Timer
            timer += Time.deltaTime;

            // Set color
            ChanginMaterial.color = curr;

            yield return null;
        }

        // On end of beat add index
        index++;

        if (index >= colors.Length) index = 0;

        IsBeat = false;
    }
}
