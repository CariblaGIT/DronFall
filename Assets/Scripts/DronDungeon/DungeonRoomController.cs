using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

// Datos generales de una sala: coordenadas X e Y y su nombre (tipo)
public class DungeonRoomInfo
{
    public string dungeonRoomName;
    public int x;
    public int y;
}

// Singleton que gestiona la cola de carga de salas, registra las salas activas y coordina puertas y enemigos
public class DungeonRoomController : MonoBehaviour
{

    public static DungeonRoomController controllerInstance;
    public string currentDungeonLevel = "EntranceHall";
    // Nombre de la escena raiz del siguiente nivel; lo lee DownstairsController al cruzar las escaleras de la sala End
    public string nextDungeonLevel = "";
    DungeonRoomInfo actualDungeonRoomData;
    DungeonRoom actualDungeonRoom;
    Queue<DungeonRoomInfo> loadDungeonRoomQueue = new Queue<DungeonRoomInfo>();
    public List<DungeonRoom> loadedDungeonRooms = new List<DungeonRoom>();
    bool isLoadingDungeonRoom = false;
    bool spawnBoss = false;
    bool updatedDungeonRooms = false;
    readonly HashSet<DungeonRoom> rewardedDungeonRooms = new HashSet<DungeonRoom>();
    // Todas las posiciones que van a tener sala, encoladas de golpe antes de cargar ninguna (ver LoadDungeonRoom).
    // Permite a cada sala saber si un vecino va a existir sin esperar a que ya este cargado, y asi ocultar sus
    // puertas sobrantes nada mas aparecer en vez de esperar a que termine de generarse toda la mazmorra
    readonly HashSet<Vector2Int> plannedRoomPositions = new HashSet<Vector2Int>();

    [SerializeField] private GameObject pauseMenuPrefab;
    [SerializeField] private GameObject gameOverPrefab;
    [SerializeField] private GameObject victoryPrefab;

    void Awake()
    {
        controllerInstance = this;
        new GameObject("Minimap").AddComponent<MinimapController>();
        Instantiate(pauseMenuPrefab);
        Instantiate(gameOverPrefab);
        Instantiate(victoryPrefab);
    }

    void Update(){
        UpdateDungeonRoomQueue();
    }

    // Procesa la cola de salas pendientes de cargar y, cuando ya no quedan, dispara la generación de la boss room
    void UpdateDungeonRoomQueue(){
        if(isLoadingDungeonRoom){
            return;
        }

        if(loadDungeonRoomQueue.Count == 0){
            if(!spawnBoss){
                StartCoroutine(DungeonSpawnBossRoom());
            } else if (spawnBoss && !updatedDungeonRooms){
                UpdatedDungeonRooms();
                updatedDungeonRooms = true;
            }
            return;
        }

        actualDungeonRoomData = loadDungeonRoomQueue.Dequeue();
        isLoadingDungeonRoom = true;

        StartCoroutine(LoadDungeonRoomRoutine(actualDungeonRoomData));
    }

    // Sustituye la última sala cargada por la sala final (boss room) una vez no quedan más salas por generar
    IEnumerator DungeonSpawnBossRoom(){
        spawnBoss = true;
        yield return new WaitForSeconds(0.5f);
        if(loadDungeonRoomQueue.Count == 0){
            DungeonRoom bossRoom = loadedDungeonRooms[loadedDungeonRooms.Count - 1];
            int bossRoomX = bossRoom.DungeonRoomX;
            int bossRoomY = bossRoom.DungeonRoomY;
            Destroy(bossRoom.gameObject);

            var roomRemove = loadedDungeonRooms.SingleOrDefault(roomToRemove => roomToRemove.DungeonRoomX == bossRoomX && roomToRemove.DungeonRoomY == bossRoomY);
            if(roomRemove != null){
                loadedDungeonRooms.Remove(roomRemove);
            }
            LoadDungeonRoom("End", bossRoomX, bossRoomY);
        }
    }

    // Encola la carga de una sala en unas coordenadas concretas, si todavía no existe una sala ahí
    public void LoadDungeonRoom(string roomName, int x, int y){
        plannedRoomPositions.Add(new Vector2Int(x, y));

        if(DungeonRoomExistence(x,y)){
            return;
        }

        DungeonRoomInfo newDungeonRoomData = new DungeonRoomInfo
        {
            dungeonRoomName = roomName,
            x = x,
            y = y
        };

        loadDungeonRoomQueue.Enqueue(newDungeonRoomData);
    }

    // Carga de forma aditiva la escena de la sala y espera a que termine antes de continuar con la cola
    IEnumerator LoadDungeonRoomRoutine(DungeonRoomInfo roomInfo){
        string roomName = currentDungeonLevel + roomInfo.dungeonRoomName;

        AsyncOperation loadDungeonRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while(loadDungeonRoom.isDone == false){
            yield return null;
        }
    }

    // Registra una sala recién cargada: la posiciona en el mundo según sus coordenadas y la añade a la lista de salas activas
    public void RegisterDungeonRoom(DungeonRoom room){
        if(!DungeonRoomExistence(actualDungeonRoomData.x, actualDungeonRoomData.y)){
            room.transform.position = new Vector3(
                actualDungeonRoomData.x * room.DungeonRoomWidth,
                actualDungeonRoomData.y * room.DungeonRoomHeight,
                0
            );

            room.DungeonRoomX = actualDungeonRoomData.x;
            room.DungeonRoomY = actualDungeonRoomData.y;
            room.DungeonRoomName = currentDungeonLevel + " / " + actualDungeonRoomData.dungeonRoomName + " " + room.DungeonRoomX + "-" + room.DungeonRoomY;
            room.transform.parent = transform;

            isLoadingDungeonRoom = false;

            if(loadedDungeonRooms.Count == 0){
                CameraController.cameraControllerInstance.currentDungeonRoom = room;
            }

            loadedDungeonRooms.Add(room);
        } else {
            Destroy(room.gameObject);
            isLoadingDungeonRoom = false;
        }
    }

    // Comprueba si ya existe una sala cargada en esas coordenadas
    public bool DungeonRoomExistence(int x, int y)
    {
        return loadedDungeonRooms.Find(item => item.DungeonRoomX == x && item.DungeonRoomY == y) != null;
    }

    // Comprueba si esas coordenadas van a tener sala (aunque todavia no se haya cargado), para que una sala
    // ya cargada pueda decidir si ocultar sus puertas sin esperar a que el vecino termine de generarse
    public bool DungeonRoomPlanned(int x, int y)
    {
        return plannedRoomPositions.Contains(new Vector2Int(x, y));
    }

    // Busca la sala cargada en unas coordenadas concretas
    public DungeonRoom FindDungeonRoom(int x, int y)
    {
        return loadedDungeonRooms.Find(item => item.DungeonRoomX == x && item.DungeonRoomY == y);
    }

    // Se llama cuando el jugador entra en una sala: actualiza la cámara y reevalúa el estado de todas las salas
    public void PlayerEntringRoom(DungeonRoom room){
        CameraController.cameraControllerInstance.currentDungeonRoom = room;
        actualDungeonRoom = room;

        StartCoroutine(DungeonRoomCoroutine());
    }

    public IEnumerator DungeonRoomCoroutine(){
        yield return new WaitForSeconds(0.2f);
        UpdatedDungeonRooms();
    }

    // Recorre todas las salas cargadas para activar/desactivar sus enemigos y bloquear o desbloquear sus puertas
    // según si el jugador está dentro y si quedan enemigos vivos
    public void UpdatedDungeonRooms(){
        foreach (DungeonRoom room in loadedDungeonRooms)
        {
            EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
            if(actualDungeonRoom != room){
                foreach (EnemyController enemy in enemies)
                {
                    enemy.notInRoom = true;
                }
                HideDoorsColl(room, false);
            } else if (room.DungeonRoomName.Contains("Start") || room.DungeonRoomName.Contains("End")){
                HideDoorsColl(room, false);
            } else {
                if(enemies.Length > 0){
                    foreach (EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = false;
                    }
                    HideDoorsColl(room, true);
                } else {
                    HideDoorsColl(room, false);
                    TryGiveRoomReward(room);
                }
            }
        }
    }

    // La primera vez que una sala se detecta sin enemigos, intenta dar su recompensa (si tiene un DungeonRoomRewardSpawner configurado)
    private void TryGiveRoomReward(DungeonRoom room){
        if(rewardedDungeonRooms.Contains(room)){
            return;
        }
        rewardedDungeonRooms.Add(room);

        DungeonRoomRewardSpawner rewardSpawner = room.GetComponentInChildren<DungeonRoomRewardSpawner>();
        if(rewardSpawner != null){
            rewardSpawner.TrySpawnReward();
        }
    }

    // Activa o desactiva el collider que bloquea físicamente una puerta (la "cierra" o la "abre")
    private void HideDoorsColl(DungeonRoom room, bool state){
        foreach (DungeonDoor door in room.GetComponentsInChildren<DungeonDoor>())
        {
            door.doorColl.SetActive(state);
        }
    }

}
