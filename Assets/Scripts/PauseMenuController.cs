using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Gestiona la pausa de la partida: escucha "Cancel" (Esc) para abrir/cerrar el menu de pausa, congela el
// juego con Time.timeScale y ofrece reanudar, abrir opciones o volver a la pantalla de titulo.
// La UI vive en el prefab Assets/Objects/UI/PauseMenu.prefab (Canvas + Panel + Botones montados en el
// Editor); este script solo referencia esos componentes y contiene la logica de pausa.
// Se instancia automaticamente al cargar cualquier nivel, ver DungeonRoomController.Awake()
public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject overlayRoot;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button titleButton;
    [SerializeField] private GameObject optionsMenuPrefab;
    [SerializeField] private AudioClip clickSound;

    private OptionsMenuController optionsMenu;
    private bool isPaused = false;

    void Awake()
    {
        resumeButton.onClick.AddListener(Resume);
        resumeButton.onClick.AddListener(PlayClickSound);
        optionsButton.onClick.AddListener(() => optionsMenu.Open());
        optionsButton.onClick.AddListener(PlayClickSound);
        titleButton.onClick.AddListener(GoToTitleScreen);
        titleButton.onClick.AddListener(PlayClickSound);

        optionsMenu = Instantiate(optionsMenuPrefab, transform).GetComponent<OptionsMenuController>();

        overlayRoot.SetActive(false);
    }

    void Update()
    {
        if(!Input.GetButtonDown("Cancel")){
            return;
        }

        if(optionsMenu.IsOpen()){
            optionsMenu.Close();
            return;
        }

        if(isPaused){
            Resume();
        } else {
            Pause();
        }
    }

    private void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        overlayRoot.SetActive(true);
    }

    private void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        overlayRoot.SetActive(false);
    }

    // Restaura el tiempo, resetea las stats del jugador y vuelve a la pantalla de titulo
    private void GoToTitleScreen()
    {
        Time.timeScale = 1f;
        GameManager.ResetState();
        SceneManager.LoadScene("MainMenu");
    }

    private void PlayClickSound()
    {
        AudioManager.PlaySfx(clickSound);
    }
}
