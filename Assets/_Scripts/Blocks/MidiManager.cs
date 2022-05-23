using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class MidiManager : MonoBehaviour
{
    [Header("Midi")]
    public static MidiManager Instance;
    public AudioSource audioSource;
    public Rows[] lanes;
    public float songDelayInSeconds;
    public PlayableDirector director;

    public string fileLocation;
    public static MidiFile midiFile;

    void Start()
    {
        Instance = this;
        ReadFromFile();
    }

    private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
        GetDataFromMidi();
    }

    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count]; // Get all the notes to the array
        notes.CopyTo(array, 0);

        // Fill row array
        foreach (var lane in lanes) lane.SetTimeStamps(array);

        // After songDelayInSeconds seconds, start playing the song
        Invoke(nameof(StartSong), songDelayInSeconds);
    }


    // Start the song
    public void StartSong()
    {
        audioSource.Play();
        director.Play();
    }

    // Get the audiosource time in double
    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }

    void Update()
    {

    }
}
