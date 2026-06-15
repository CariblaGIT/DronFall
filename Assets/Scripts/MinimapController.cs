using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Dibuja en pantalla el layout completo de la mazmorra generada (todas las salas, visitadas o no),
// coloreando cada sala segun su tipo y resaltando en la que esta el jugador ahora mismo.
// Se construye enteramente por codigo (Canvas + iconos) para no depender de ninguna escena o prefab.
public class MinimapController : MonoBehaviour
{
    private const float cellSize = 14f;
    private const float cellSpacing = 2f;
    private const float screenMargin = 16f;

    private static readonly Color colorPending = new Color(1f, 1f, 1f, 0.15f);
    private static readonly Color colorRoom = new Color(0.6f, 0.6f, 0.6f, 0.9f);
    private static readonly Color colorStart = new Color(0.3f, 0.8f, 0.3f, 0.9f);
    private static readonly Color colorEnd = new Color(0.85f, 0.25f, 0.25f, 0.9f);
    private static readonly Color colorCurrent = new Color(1f, 0.85f, 0.2f, 1f);

    private RectTransform panelRect;
    private readonly Dictionary<Vector2Int, Image> roomIcons = new Dictionary<Vector2Int, Image>();
    private bool gridBuilt = false;

    void Start()
    {
        BuildCanvas();
    }

    void Update()
    {
        if(!gridBuilt)
        {
            TryBuildGrid();
            return;
        }

        RefreshRoomStates();
    }

    // Crea el Canvas y el panel contenedor del minimapa, anclados a la esquina superior derecha de la pantalla
    private void BuildCanvas()
    {
        GameObject canvasGO = new GameObject("MinimapCanvas");
        canvasGO.transform.SetParent(transform, false);
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        GameObject panelGO = new GameObject("MinimapPanel", typeof(RectTransform));
        panelGO.transform.SetParent(canvasGO.transform, false);
        panelRect = panelGO.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(1, 1);
        panelRect.anchorMax = new Vector2(1, 1);
        panelRect.pivot = new Vector2(1, 1);
        panelRect.anchoredPosition = new Vector2(-screenMargin, -screenMargin);

        Image panelBackground = panelGO.AddComponent<Image>();
        panelBackground.color = new Color(0f, 0f, 0f, 0.35f);
    }

    // En cuanto el generador ha calculado las posiciones de las salas, crea un icono por cada una (mapa completo desde el inicio)
    private void TryBuildGrid()
    {
        if(DungeonCrawlerController.visitedPositions.Count == 0)
        {
            return;
        }

        List<Vector2Int> roomPositions = new List<Vector2Int>(DungeonCrawlerController.visitedPositions);
        roomPositions.Add(Vector2Int.zero);

        int minX = 0, maxX = 0, minY = 0, maxY = 0;
        foreach (Vector2Int pos in roomPositions)
        {
            minX = Mathf.Min(minX, pos.x);
            maxX = Mathf.Max(maxX, pos.x);
            minY = Mathf.Min(minY, pos.y);
            maxY = Mathf.Max(maxY, pos.y);
        }

        int gridWidth = maxX - minX + 1;
        int gridHeight = maxY - minY + 1;
        panelRect.sizeDelta = new Vector2(gridWidth * cellSize + cellSpacing, gridHeight * cellSize + cellSpacing);

        foreach (Vector2Int pos in roomPositions)
        {
            if(roomIcons.ContainsKey(pos))
            {
                continue;
            }
            roomIcons[pos] = CreateRoomIcon(pos, minX, minY);
        }

        gridBuilt = true;
    }

    // Instancia el icono (un simple cuadrado de UI) que representa una sala en su posicion dentro de la grid del minimapa
    private Image CreateRoomIcon(Vector2Int gridPos, int minX, int minY)
    {
        GameObject iconGO = new GameObject($"Room {gridPos.x}-{gridPos.y}", typeof(RectTransform));
        iconGO.transform.SetParent(panelRect, false);

        RectTransform rect = iconGO.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(cellSize - cellSpacing, cellSize - cellSpacing);
        rect.anchoredPosition = new Vector2(
            (gridPos.x - minX) * cellSize + cellSize / 2,
            (gridPos.y - minY) * cellSize + cellSize / 2
        );

        Image image = iconGO.AddComponent<Image>();
        image.color = colorPending;
        return image;
    }

    // Actualiza el color de cada sala segun si ya esta cargada, su tipo (inicial/normal/boss) y si es la sala actual del jugador
    private void RefreshRoomStates()
    {
        DungeonRoomController controller = DungeonRoomController.controllerInstance;
        if(controller == null)
        {
            return;
        }

        DungeonRoom currentRoom = CameraController.cameraControllerInstance != null
            ? CameraController.cameraControllerInstance.currentDungeonRoom
            : null;

        foreach (KeyValuePair<Vector2Int, Image> entry in roomIcons)
        {
            Vector2Int pos = entry.Key;
            Image icon = entry.Value;
            DungeonRoom room = controller.FindDungeonRoom(pos.x, pos.y);

            if(room == null)
            {
                icon.color = colorPending;
                continue;
            }

            if(currentRoom == room)
            {
                icon.color = colorCurrent;
            }
            else if(room.DungeonRoomName.Contains("Start"))
            {
                icon.color = colorStart;
            }
            else if(room.DungeonRoomName.Contains("End"))
            {
                icon.color = colorEnd;
            }
            else
            {
                icon.color = colorRoom;
            }
        }
    }
}
