using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{

    // General specifications of the Room
    public int DungeonRoomWidth;
    public int DungeonRoomHeight;
    public int DungeonRoomX;
    public int DungeonRoomY;
    public string DungeonRoomName;
    private bool dungeonRoomDoorsUpdated = false;

    // Room doors specifications
    public DungeonDoor topDungeonDoor;
    public DungeonDoor leftDungeonDoor;
    public DungeonDoor rightDungeonDoor;
    public DungeonDoor bottomDungeonDoor;
    public List<DungeonDoor> listDungeonDoors = new List<DungeonDoor>();

    /* On start, we add all doors into the listDungeonDoors and assign each one to his respective variable,
       and after that we register the room on the RoomController*/
    void Start()
    {
        if(DungeonRoomController.controllerInstance == null)
        {
            return;
        }

        DungeonDoor[] doors = GetComponentsInChildren<DungeonDoor>();

        foreach (DungeonDoor door in doors)
        {
            listDungeonDoors.Add(door);
            switch (door.doorDirection)
            {
                case DungeonDoor.DoorDirection.top:
                    topDungeonDoor = door;
                    break;
                case DungeonDoor.DoorDirection.left:
                    leftDungeonDoor = door;
                    break;
                case DungeonDoor.DoorDirection.right:
                    rightDungeonDoor = door;
                    break;
                case DungeonDoor.DoorDirection.bottom:
                    bottomDungeonDoor = door;
                    break;
            }
        }

        DungeonRoomController.controllerInstance.RegisterDungeonRoom(this);
    }

    // Update method that checks if the doors have been removed using the bool variable and for the end room generation
    void Update(){
        if(DungeonRoomName.Contains("End") && !dungeonRoomDoorsUpdated){
            RemoveExtraDoors();
            dungeonRoomDoorsUpdated = true;
        }
    }

    // Method to remove the doors from each direction if there is not room at that point
    public void RemoveExtraDoors(){
        foreach (DungeonDoor door in listDungeonDoors)
        {
            switch (door.doorDirection)
            {
                case DungeonDoor.DoorDirection.top:
                    if(GetTopRoom() == null){
                        door.gameObject.SetActive(false);
                    }
                    break;
                case DungeonDoor.DoorDirection.left:
                    if(GetLeftRoom() == null){
                        door.gameObject.SetActive(false);
                    }
                    break;
                case DungeonDoor.DoorDirection.right:
                    if(GetRightRoom() == null){
                        door.gameObject.SetActive(false);
                    }
                    break;
                case DungeonDoor.DoorDirection.bottom:
                    if(GetBottomRoom() == null){
                        door.gameObject.SetActive(false);
                    }
                    break;
            }
        }
    }

    // Method to check if exists a room on top of this room
    public DungeonRoom GetTopRoom(){
        if(DungeonRoomController.controllerInstance.DungeonRoomExistence(DungeonRoomX, DungeonRoomY + 1)){
            return DungeonRoomController.controllerInstance.FindDungeonRoom(DungeonRoomX, DungeonRoomY + 1);
        }
        return null;
    }

    // Method to check if exists a room on the left of this room
    public DungeonRoom GetLeftRoom(){
        if(DungeonRoomController.controllerInstance.DungeonRoomExistence(DungeonRoomX - 1, DungeonRoomY)){
            return DungeonRoomController.controllerInstance.FindDungeonRoom(DungeonRoomX - 1, DungeonRoomY);
        }
        return null;
    }

    // Method to check if exists a room on the right of this room
    public DungeonRoom GetRightRoom(){
        if(DungeonRoomController.controllerInstance.DungeonRoomExistence(DungeonRoomX + 1, DungeonRoomY)){
            return DungeonRoomController.controllerInstance.FindDungeonRoom(DungeonRoomX + 1, DungeonRoomY);
        }
        return null;
    }

    // Method to check if exists a room on bottom of this room
    public DungeonRoom GetBottomRoom(){
        if(DungeonRoomController.controllerInstance.DungeonRoomExistence(DungeonRoomX, DungeonRoomY - 1)){
            return DungeonRoomController.controllerInstance.FindDungeonRoom(DungeonRoomX, DungeonRoomY - 1);
        }
        return null;
    }

    // Method to create a DungeonRoom using only the coords of it, is for creating a support DungeonRoom variable
    public DungeonRoom (int x, int y){
        DungeonRoomX = x;
        DungeonRoomY = y;
    }

    // Method to draw the border of a room to check the position on the field
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(DungeonRoomWidth, DungeonRoomHeight, 0));
    }

    // Method to retrieve the center of a room
    public Vector3 GetRoomCentre()
    {
        return new Vector3(DungeonRoomX * DungeonRoomWidth, DungeonRoomY * DungeonRoomHeight);
    }

    // Trigger to set on the DungeonRoomController the room in which the player is playing
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            DungeonRoomController.controllerInstance.PlayerEntringRoom(this);
        }
    }

}
