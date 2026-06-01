using UnityEngine;
using UnityEngine.SceneManagement;

// Objeto interactivo de la sala End: al tocarlo el jugador, carga la escena raiz del siguiente nivel de la
// mazmorra, sustituyendo la escena actual (mismo mecanismo que MainMenu.PlayGameButton). No resetea las
// stats del jugador, a diferencia de una partida nueva, para que las mejoras conseguidas persistan entre niveles.
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class DownstairsController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        SceneManager.LoadScene(DungeonRoomController.controllerInstance.nextDungeonLevel);
    }

    // Ajusta el collider de trigger al tamano del sprite al añadir el componente en el Editor
    private void Reset()
    {
        BoxCollider2D coll = GetComponent<BoxCollider2D>();
        coll.isTrigger = true;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite != null)
        {
            coll.size = spriteRenderer.sprite.bounds.size;
        }
    }
}
