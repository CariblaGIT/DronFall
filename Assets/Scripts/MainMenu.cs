using UnityEngine;
using UnityEngine.SceneManagement;

// Controla los botones del menú principal
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenuPrefab;
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip clickSound;

    private OptionsMenuController optionsMenu;

    void Awake()
    {
        optionsMenu = Instantiate(optionsMenuPrefab, transform).GetComponent<OptionsMenuController>();
        AudioManager.PlayMusic(menuMusic);
    }

    // Resetea las stats del jugador (por si venimos de una partida anterior) y carga el primer nivel de la mazmorra
    public void PlayGameButton()
    {
        AudioManager.PlaySfx(clickSound);
        GameManager.ResetState();
        SceneManager.LoadSceneAsync("EntranceHall");
    }

    // Abre el panel de opciones (volumen de música/efectos y ver controles)
    public void OptionsButton()
    {
        AudioManager.PlaySfx(clickSound);
        optionsMenu.Open();
    }

    // Cierra la aplicación
    public void QuitGameButton()
    {
        AudioManager.PlaySfx(clickSound);
        Application.Quit();
    }
}
