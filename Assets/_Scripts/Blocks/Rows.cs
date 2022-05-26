using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rows : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public GameObject notePrefab; // What note to spawn
    public float Speed;

    List<Note> notes = new List<Note>(); // List with all notes in the row
    public List<double> timeStamps = new List<double>(); // Time stamps of notes in the row

    public int spawnIndex = 0; // What note to spawn next
    public int row;

    void Start()
    {

    }

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, MidiManager.midiFile.GetTempoMap());
                // Saving the time stamp of the note in seconds in the timestamps list
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }

    void Update()
    {
        // Spawning notes
        if (spawnIndex < timeStamps.Count)
        {
            if (MidiManager.GetAudioSourceTime() >= timeStamps[spawnIndex])
            {
                // Instantiating the note if the time stamp is reached
                print("Note: " + spawnIndex + " at " + timeStamps[spawnIndex] + " in the row" + row);
                GameObject newBlock = Instantiate(notePrefab);
                newBlock.GetComponent<BlockController>().Rail = row;
                newBlock.GetComponent<BlockController>().Speed = Speed;

                //var note = Instantiate(notePrefab, transform);
                spawnIndex++;
            }
        }
    }
}