using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpectrum : MonoBehaviour
{
    [Header("Spectrum")]
    [SerializeField] private float[] _audioSpectrum;
    [SerializeField] private int _spectrumSize = 128;

    public AudioSource audioSource;

    public static float SpectrumValue { get; private set; }

    void Start()
    {
        // Initializing the array
        _audioSpectrum = new float[_spectrumSize];

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (audioSource == null) return;
        // Getting the spectrum data
        audioSource.GetSpectrumData(_audioSpectrum, 0, FFTWindow.Hamming);

        // Getting the average value
        if (_audioSpectrum != null && _audioSpectrum.Length > 0)
        {
            SpectrumValue = _audioSpectrum[0] * 100;
        }
    }
}
