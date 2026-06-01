using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Pantalla de victoria: se suscribe a GameManager.OnVictory (disparado al destruir el nucleo del ultimo
// nivel, ver CoreController), congela el juego y ofrece jugar de nuevo desde el primer nivel o volver al
// menu principal. Misma estructura que GameOverController: la UI vive en el prefab
// Assets/Objects/UI/VictoryMenu.prefab, este script solo referencia esos componentes.
public class VictoryController : MonoBehaviour
{
    [SerializeField] private GameObject overlayRoot;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button titleButton;
    [SerializeField] private AudioClip clickSound;

    void Awake()
    {
        playAgainButton.onClick.AddListener(PlayAgain);
        playAgainButton.onClick.AddListener(PlayClickSound);
        titleButton.onClick.AddListener(GoToTitleScreen);
        titleButton.onClick.AddListener(PlayClickSound);

        overlayRoot.SetActive(false);

        GameManager.OnVictory += Show;
    }

    void OnDestroy()
    {
        GameManager.OnVictory -= Show;
    }

    private void Show()
    {
        Time.timeScale = 0f;
        overlayRoot.SetActive(true);
    }

    // Empieza una partida nueva desde el primer nivel, sin arrastrar las stats de la partida ganada
    private void PlayAgain()
    {
        Time.timeScale = 1f;
        GameManager.ResetState();
        SceneManager.LoadScene("EntranceHall");
    }

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
