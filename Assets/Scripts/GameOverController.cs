using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Pantalla de derrota: se suscribe a GameManager.OnPlayerDeath (disparado cuando la vida llega a 0), congela
// el juego y ofrece reintentar (nueva partida desde el primer nivel) o volver al menu principal. Misma
// estructura que PauseMenuController: la UI vive en el prefab Assets/Objects/UI/GameOverMenu.prefab, este
// script solo referencia esos componentes.
public class GameOverController : MonoBehaviour
{
    [SerializeField] private GameObject overlayRoot;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button titleButton;
    [SerializeField] private AudioClip clickSound;

    void Awake()
    {
        retryButton.onClick.AddListener(Retry);
        retryButton.onClick.AddListener(PlayClickSound);
        titleButton.onClick.AddListener(GoToTitleScreen);
        titleButton.onClick.AddListener(PlayClickSound);

        overlayRoot.SetActive(false);

        GameManager.OnPlayerDeath += Show;
    }

    void OnDestroy()
    {
        GameManager.OnPlayerDeath -= Show;
    }

    private void Show()
    {
        Time.timeScale = 0f;
        overlayRoot.SetActive(true);
    }

    // Empieza una partida nueva desde el primer nivel, sin arrastrar las stats de la partida perdida
    private void Retry()
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
