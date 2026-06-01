using UnityEngine;

// Reproduce musica y efectos de sonido respetando el volumen configurado en SettingsManager. La primera vez
// que se llama a PlayMusic/PlaySfx crea su propio GameObject oculto con los AudioSource necesarios y lo marca
// DontDestroyOnLoad, asi que no hace falta colocarlo a mano en cada escena ni pasar por Resources.Load.
public static class AudioManager
{
    private static AudioSource musicSource;
    private static AudioSource sfxSource;

    private static void EnsureInitialized()
    {
        if (musicSource != null)
        {
            return;
        }

        GameObject holder = new GameObject("AudioManager");
        Object.DontDestroyOnLoad(holder);

        musicSource = holder.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = SettingsManager.MusicVolume;

        sfxSource = holder.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;

        SettingsManager.OnMusicVolumeChanged += volume => musicSource.volume = volume;
    }

    // Cambia la musica de fondo; no hace nada si ya es la que esta sonando, para no reiniciarla cada vez que
    // se llama (por ejemplo al recargar una sala del mismo nivel)
    public static void PlayMusic(AudioClip clip)
    {
        EnsureInitialized();
        if (clip == null || musicSource.clip == clip)
        {
            return;
        }

        musicSource.clip = clip;
        musicSource.Play();
    }

    public static void PlaySfx(AudioClip clip)
    {
        EnsureInitialized();
        if (clip == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip, SettingsManager.SfxVolume);
    }
}
