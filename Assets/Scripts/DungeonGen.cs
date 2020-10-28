using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGen : MonoSingleton<DungeonGen>
{

    public List<GameObject> SpawnPoints = new List<GameObject>();
    public List<GameObject> DoorPoints = new List<GameObject>();
    public List<GameObject> AllPoints = new List<GameObject>();


    //public  List<GameObject> SpawnPointsUsed = new List<GameObject>();
    public List<GameObject> RoomPool = new List<GameObject>();

    [SerializeField] LayerMask detectionLayerMask;

    [SerializeField] float spawnDelay = 0.1f;
    [SerializeField] int maxRoom = 50, randomSpawn = 1;

    [SerializeField] bool isClosingDoor;
    [SerializeField] GameObject doorFace;
    [SerializeField] GameObject startingRoom;

    public List<GameObject> Rooms;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in startingRoom.transform)
        {
            if (child.tag == "Door")
            {
                SpawnPoints.Add(child.gameObject);
            }
        }

            StartCoroutine(SpawningRooms());
        
    }

    public void AddSpawnPoint(GameObject Obj)
    {
        SpawnPoints.Add(Obj);
    }

    IEnumerator SpawningRooms()
    {
        GameObject tempRoom;
        float roomLength;

        while (true)
        {
            while (SpawnPoints.Count > 0 && RoomPool.Count < maxRoom)
            {
                Transform spawnObj = SpawnPoints[0].transform;
                List<GameObject> roomExits = new List<GameObject>();
                tempRoom = Rooms[Random.Range(0, randomSpawn)];
                roomLength = tempRoom.GetComponent<Room>().GetRoomLenght();
                spawnObj.parent.gameObject.layer = 8;

                RoomPool.Add(Instantiate(tempRoom));
                tempRoom.layer = 9;
                tempRoom = RoomPool[RoomPool.Count - 1];

                foreach (Transform child in tempRoom.transform)
                {
                    if (child.tag == "Door")
                    {
                        roomExits.Add(child.gameObject);
                    }
                }
                Transform tempDoor = roomExits[Random.Range(0, roomExits.Count)].transform;
                tempDoor.name = "Entrance";
                AllPoints.Add(tempDoor.gameObject);
                roomExits.Remove(tempDoor.gameObject);

                foreach (GameObject exit in roomExits)
                {
                    AddSpawnPoint(exit);
                }

                //Check for the room rotation
                float angleDiff = Mathf.Round(tempDoor.rotation.eulerAngles.y - spawnObj.rotation.eulerAngles.y);                
                tempRoom.transform.rotation *= Quaternion.Euler(0, 180 - angleDiff, 0);

                //Check for the room position
                Vector3 doorOffset = tempDoor.position - tempRoom.transform.position;
                tempRoom.transform.position = spawnObj.position - doorOffset;
                tempRoom.transform.position = new Vector3(tempRoom.transform.position.x, spawnObj.position.y + (tempDoor.localPosition.y * -1), tempRoom.transform.position.z);

                yield return new WaitForSeconds(.030f); //Wait time until the collisions update
                if (tempRoom.GetComponent<Room>().isCollidingOtherRoom)
                {
                    DoorPoints.Add(spawnObj.gameObject);
                    RoomPool.Remove(tempRoom);
                    foreach (GameObject exit in roomExits)
                    {
                        SpawnPoints.Remove(exit);
                    }                    
                    Destroy(tempRoom);
                }
                else
                {
                    tempRoom.layer = 7;
                    tempRoom.GetComponent<Room>().RemoveRb();
                }

                AllPoints.Add(spawnObj.gameObject);
                SpawnPoints.RemoveAt(0);
                spawnObj.parent.gameObject.layer = 7;

                yield return new WaitForSeconds(spawnDelay);
            }

            if (RoomPool.Count == maxRoom && isClosingDoor)
            {
                DoorPoints.AddRange(SpawnPoints);
                SpawnPoints.Clear();
                foreach (GameObject doorPoint in DoorPoints)
                {
                    Instantiate(doorFace, doorPoint.transform.position, doorPoint.transform.rotation);                    
                }
                AllPoints.AddRange(DoorPoints);
                DoorPoints.Clear();
                foreach (GameObject door in AllPoints)
                {
                    Destroy(door);
                }
                AllPoints.Clear();
            }

            yield return new WaitForSeconds(2);
        }
    }
}
