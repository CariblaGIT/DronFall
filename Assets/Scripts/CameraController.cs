using UnityEngine;

// Sigue a la sala activa de la mazmorra, moviéndose hacia su centro a velocidad constante
public class CameraController : MonoBehaviour
{

    public static CameraController cameraControllerInstance;
    public DungeonRoom currentDungeonRoom;
    public float cameraSpeed;

    void Awake(){
        cameraControllerInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraPosition();
    }

    // Desplaza la cámara hacia el centro de la sala actual (asignada por el DungeonRoomController)
    void UpdateCameraPosition(){
        if(currentDungeonRoom == null){
            return;
        }

        Vector3 targetCameraPos = GetCameraTargetPos();

        transform.position = Vector3.MoveTowards(transform.position, targetCameraPos, Time.deltaTime * cameraSpeed);
    }

    // Calcula la posición objetivo de la cámara (centro de la sala actual, manteniendo la profundidad z de la cámara)
    Vector3 GetCameraTargetPos() {
        if(currentDungeonRoom == null){
            return Vector3.zero;
        }

        Vector3 targetCameraPos = currentDungeonRoom.GetRoomCentre();
        targetCameraPos.z = transform.position.z;

        return targetCameraPos;
    }

    // Indica si la cámara todavía está desplazándose hacia la sala actual (útil para bloquear acciones durante la transición)
    public bool IsMovingBetweenScenes(){
        return transform.position.Equals(GetCameraTargetPos()) == false;
    }
}
