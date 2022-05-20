using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncer : MonoBehaviour
{
    [Header("Spectrum variables")]
    public float Bias; // What spectrum value is going to trigger a beat
    public float TimeStep; // Minimum time between beats
    public float TimeToBeat; // How long it takes for the beat to happen
    public float RestTime; // How long does the object last to go to default position

    [Header("Audio variables (DONTU TOUCHU JOTO)")]
    [SerializeField] private float _previousAudioValue;
    [SerializeField] private float _currentAudioValue;
    [SerializeField] private float _timer;

    public bool IsBeat;
    public bool RandomVars = false;

    private void Awake()
    {
        if (RandomVars)
        {
            DoesHaveRandom();
        }
    }

    void Start()
    {
        
    }

    private void DoesHaveRandom()
    {
        Bias += Random.Range(-1, 1);
        TimeStep += Random.Range(-.1f, .1f);
        TimeToBeat += Random.Range(-.1f, .1f);
        RestTime += Random.Range(-1, 1);
    }

    void Update()
    {
        OnUpdate();
    }

    public virtual void OnUpdate()
    {
        _previousAudioValue = _currentAudioValue;
        _currentAudioValue = AudioSpectrum.SpectrumValue;

        if (_previousAudioValue > Bias && _currentAudioValue <= Bias)
        {
            if (_timer > TimeStep) OnBeat();
        }

        if (_previousAudioValue <= Bias && _currentAudioValue > Bias)
        {
            if (_timer > TimeStep) OnBeat();
        }

        _timer += Time.deltaTime;
    }

    public virtual void OnBeat()
    {
        // print("Beat");

        _timer = 0;
        IsBeat = true;
    }
}
