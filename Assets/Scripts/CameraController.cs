using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void UpdateCameraPosition(){
        if(currentDungeonRoom == null){
            return;
        }

        Vector3 targetCameraPos = GetCameraTargetPos();

        transform.position = Vector3.MoveTowards(transform.position, targetCameraPos, Time.deltaTime * cameraSpeed);
    }

    Vector3 GetCameraTargetPos() {
        if(currentDungeonRoom == null){
            return Vector3.zero;
        }

        Vector3 targetCameraPos = currentDungeonRoom.GetRoomCentre();
        targetCameraPos.z = transform.position.z;

        return targetCameraPos;
    }

    public bool IsMovingBetweenScenes(){
        return transform.position.Equals(GetCameraTargetPos()) == false;
    }
}
