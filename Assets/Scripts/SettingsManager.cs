using System;
using UnityEngine;

// Guarda y persiste las preferencias de audio del jugador (volumen de musica y de efectos) entre partidas.
// No usa un AudioMixer: cualquier AudioSource de musica/efectos debe leer estos valores (o suscribirse a los
// eventos) para aplicar su propio volumen, ya que todavia no hay audio en el proyecto
public static class SettingsManager
{
    private const string MusicVolumeKey = "MusicVolume";
    private const string SfxVolumeKey = "SfxVolume";

    public static event Action<float> OnMusicVolumeChanged;
    public static event Action<float> OnSfxVolumeChanged;

    public static float MusicVolume
    {
        get => PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        set
        {
            PlayerPrefs.SetFloat(MusicVolumeKey, value);
            OnMusicVolumeChanged?.Invoke(value);
        }
    }

    public static float SfxVolume
    {
        get => PlayerPrefs.GetFloat(SfxVolumeKey, 1f);
        set
        {
            PlayerPrefs.SetFloat(SfxVolumeKey, value);
            OnSfxVolumeChanged?.Invoke(value);
        }
    }
}
