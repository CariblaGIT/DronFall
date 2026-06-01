using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{

    // Especificaciones generales de la sala
    public int DungeonRoomWidth;
    public int DungeonRoomHeight;
    public int DungeonRoomX;
    public int DungeonRoomY;
    public string DungeonRoomName;

    // Especificaciones de las puertas de la sala
    public DungeonDoor topDungeonDoor;
    public DungeonDoor leftDungeonDoor;
    public DungeonDoor rightDungeonDoor;
    public DungeonDoor bottomDungeonDoor;
    public List<DungeonDoor> listDungeonDoors = new List<DungeonDoor>();

    // Desplazamientos (x, y) de cada sala vecina respecto a esta, usados para buscarla en el DungeonRoomController
    private static readonly Dictionary<DungeonDoor.DoorDirection, Vector2Int> neighborOffsets = new Dictionary<DungeonDoor.DoorDirection, Vector2Int>{
        {DungeonDoor.DoorDirection.top, Vector2Int.up},
        {DungeonDoor.DoorDirection.left, Vector2Int.left},
        {DungeonDoor.DoorDirection.right, Vector2Int.right},
        {DungeonDoor.DoorDirection.bottom, Vector2Int.down}
    };

    /* Al arrancar, añadimos todas las puertas a listDungeonDoors y asignamos cada una a su variable
       correspondiente, y después registramos la sala en el RoomController */
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

        // Se hace aqui mismo, no esperando a que termine de generarse toda la mazmorra: como el generador ya
        // encola de golpe todas las posiciones antes de cargar ninguna sala, esta sala ya sabe si un vecino va
        // a existir o no desde el primer frame, así que la puerta nunca llega a mostrarse abierta para luego
        // desaparecer de golpe delante del jugador
        RemoveExtraDoors();
    }

    // Desactiva las puertas de cada dirección si no hay ninguna sala planificada en esa posición
    public void RemoveExtraDoors(){
        foreach (DungeonDoor door in listDungeonDoors)
        {
            Vector2Int offset = neighborOffsets[door.doorDirection];
            int neighborX = DungeonRoomX + offset.x;
            int neighborY = DungeonRoomY + offset.y;

            if(!DungeonRoomController.controllerInstance.DungeonRoomPlanned(neighborX, neighborY)){
                door.gameObject.SetActive(false);
            }
        }
    }

    // Dibuja el borde de la sala para comprobar su posición en el editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(DungeonRoomWidth, DungeonRoomHeight, 0));
    }

    // Devuelve el centro de la sala
    public Vector3 GetRoomCentre()
    {
        return new Vector3(DungeonRoomX * DungeonRoomWidth, DungeonRoomY * DungeonRoomHeight);
    }

    // Trigger que avisa al DungeonRoomController de en qué sala está jugando el jugador
    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            DungeonRoomController.controllerInstance.PlayerEntringRoom(this);
        }
    }

}
