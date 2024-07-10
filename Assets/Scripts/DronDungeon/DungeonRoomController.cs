using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class DungeonRoomInfo
{
    public string dungeonRoomName;
    public int x;
    public int y;
}

public class DungeonRoomController : MonoBehaviour
{

    public static DungeonRoomController controllerInstance;
    string currentDungeonLevel = "EntranceHall";
    DungeonRoomInfo actualDungeonRoomData;
    DungeonRoom actualDungeonRoom;
    Queue<DungeonRoomInfo> loadDungeonRoomQueue = new Queue<DungeonRoomInfo>();
    public List<DungeonRoom> loadedDungeonRooms = new List<DungeonRoom>();
    bool isLoadingDungeonRoom = false;
    bool spawnBoss = false;
    bool updatedDungeonRooms = false;

    void Awake()
    {
        controllerInstance = this;
    }

    void Update(){
        UpdateDungeonRoomQueue();
    }

    void UpdateDungeonRoomQueue(){
        if(isLoadingDungeonRoom){
            return;
        }

        if(loadDungeonRoomQueue.Count == 0){
            if(!spawnBoss){
                StartCoroutine(DungeonSpawnBossRoom());
            } else if (spawnBoss && !updatedDungeonRooms){
                foreach (DungeonRoom room in loadedDungeonRooms)
                {
                    room.RemoveExtraDoors();
                }
                UpdatedDungeonRooms();
                updatedDungeonRooms = true;
            }
            return;
        }

        actualDungeonRoomData = loadDungeonRoomQueue.Dequeue();
        isLoadingDungeonRoom = true;

        StartCoroutine(LoadDungeonRoomRoutine(actualDungeonRoomData));
    }

    IEnumerator DungeonSpawnBossRoom(){
        spawnBoss = true;
        yield return new WaitForSeconds(0.5f);
        if(loadDungeonRoomQueue.Count == 0){
            DungeonRoom bossRoom = loadedDungeonRooms[loadedDungeonRooms.Count - 1];
            DungeonRoom suppRoom = new DungeonRoom(bossRoom.DungeonRoomX, bossRoom.DungeonRoomY);
            Destroy(bossRoom.gameObject);

            var roomRemove = loadedDungeonRooms.Single(roomToRemove => roomToRemove.DungeonRoomX == suppRoom.DungeonRoomX && roomToRemove.DungeonRoomY == suppRoom.DungeonRoomY);
            loadedDungeonRooms.Remove(roomRemove);
            LoadDungeonRoom("End", suppRoom.DungeonRoomX, suppRoom.DungeonRoomY);
        }
    }

    public void LoadDungeonRoom(string roomName, int x, int y){
        if(DungeonRoomExistence(x,y)){
            return;
        }

        DungeonRoomInfo newDungeonRoomData = new DungeonRoomInfo();
        newDungeonRoomData.dungeonRoomName = roomName;
        newDungeonRoomData.x = x;
        newDungeonRoomData.y = y;

        loadDungeonRoomQueue.Enqueue(newDungeonRoomData);
    }

    IEnumerator LoadDungeonRoomRoutine(DungeonRoomInfo roomInfo){
        string roomName = currentDungeonLevel + roomInfo.dungeonRoomName;

        AsyncOperation loadDungeonRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while(loadDungeonRoom.isDone == false){
            yield return null;
        }
    }

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
            // room.RemoveExtraDoors();
        } else {
            Destroy(room.gameObject);
            isLoadingDungeonRoom = false;
        }
    }

    public bool DungeonRoomExistence(int x, int y)
    {
        return loadedDungeonRooms.Find(item => item.DungeonRoomX == x && item.DungeonRoomY == y) != null;
    }

    public DungeonRoom FindDungeonRoom(int x, int y)
    {
        return loadedDungeonRooms.Find(item => item.DungeonRoomX == x && item.DungeonRoomY == y);
    }

    public void PlayerEntringRoom(DungeonRoom room){
        CameraController.cameraControllerInstance.currentDungeonRoom = room;
        actualDungeonRoom = room;

        StartCoroutine(DungeonRoomCoroutine());
    }

    public IEnumerator DungeonRoomCoroutine(){
        yield return new WaitForSeconds(0.2f);
        UpdatedDungeonRooms();
    }

    public void UpdatedDungeonRooms(){
        foreach (DungeonRoom room in loadedDungeonRooms)
        {
            EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
            if(actualDungeonRoom != room){
                if(enemies != null){
                    foreach (EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = true;
                    }
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
                }
            }
        }
    }

    private void HideDoorsColl(DungeonRoom room, bool state){
        foreach (DungeonDoor door in room.GetComponentsInChildren<DungeonDoor>())
        {
            door.doorColl.SetActive(state);
        }
    }
    
}
