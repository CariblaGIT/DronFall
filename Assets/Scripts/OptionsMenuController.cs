using UnityEngine;
using UnityEngine.UI;

// Panel de opciones reutilizable (volumen de musica/efectos y ver controles), instanciado tanto desde el
// menu principal como desde el menu de pausa (ver PauseMenuController y MainMenu). La UI vive en el prefab
// Assets/Objects/UI/OptionsMenu.prefab; este script solo referencia esos componentes y contiene la logica
// de apertura/cierre y el enganche con SettingsManager.
public class OptionsMenuController : MonoBehaviour
{
    [SerializeField] private GameObject optionsOverlay;
    [SerializeField] private GameObject controlsOverlay;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button showControlsButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button closeControlsButton;
    [SerializeField] private AudioClip clickSound;

    void Awake()
    {
        musicSlider.value = SettingsManager.MusicVolume;
        sfxSlider.value = SettingsManager.SfxVolume;
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);

        showControlsButton.onClick.AddListener(ShowControls);
        showControlsButton.onClick.AddListener(PlayClickSound);
        backButton.onClick.AddListener(Close);
        backButton.onClick.AddListener(PlayClickSound);
        closeControlsButton.onClick.AddListener(HideControls);
        closeControlsButton.onClick.AddListener(PlayClickSound);

        optionsOverlay.SetActive(false);
        controlsOverlay.SetActive(false);
    }

    public void Open()
    {
        controlsOverlay.SetActive(false);
        optionsOverlay.SetActive(true);
    }

    public void Close()
    {
        optionsOverlay.SetActive(false);
    }

    public bool IsOpen()
    {
        return optionsOverlay.activeSelf;
    }

    private void SetMusicVolume(float value)
    {
        SettingsManager.MusicVolume = value;
    }

    private void SetSfxVolume(float value)
    {
        SettingsManager.SfxVolume = value;
    }

    private void ShowControls()
    {
        controlsOverlay.SetActive(true);
    }

    private void HideControls()
    {
        controlsOverlay.SetActive(false);
    }

    private void PlayClickSound()
    {
        AudioManager.PlaySfx(clickSound);
    }
}
