using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum WorldType
{
    Medieval,
    Futuristic
}

enum MoodType
{
    Horror,
    Joy
}

public class DungeonGen : MonoSingleton<DungeonGen>
{

    public List<GameObject> SpawnPoints = new List<GameObject>();
    public List<GameObject> DoorPoints = new List<GameObject>();
    public List<GameObject> AllPoints = new List<GameObject>();

    //public  List<GameObject> SpawnPointsUsed = new List<GameObject>();
    public List<GameObject> RoomPool = new List<GameObject>();

    [SerializeField] WorldType worldType;
    [SerializeField] MoodType moodType;

    [SerializeField] List<Material> worldMaterials = new List<Material>();

    [SerializeField] float spawnDelay = 0.1f;
    [SerializeField] int maxRoom = 50, randomSpawn = 1;

    [SerializeField] bool isClosingDoor;
    [SerializeField] bool isMixGen;
    [SerializeField] bool isGenOnlyFromLastSpawn;

    [SerializeField] GameObject doorFace;
    [SerializeField] GameObject startingRoom;

    public List<GameObject> Rooms;
    public List<GameObject> Corridors;

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

        while (true)
        {
            while (SpawnPoints.Count > 0 && RoomPool.Count < maxRoom)
            {

                Transform spawnObj = SpawnPoints[0].transform;

                //if (isGenOnlyFromLastSpawn)
                //    SpawnPoints.Clear();

                List<GameObject> roomExits = new List<GameObject>();
                tempRoom = GetRandomRoom(spawnObj);
                spawnObj.parent.gameObject.layer = 8;

                RoomPool.Add(Instantiate(tempRoom));
                tempRoom.layer = 9;
                tempRoom = RoomPool[RoomPool.Count - 1];

                if (worldType == WorldType.Medieval)
                    tempRoom.GetComponent<MeshRenderer>().material = worldMaterials[0];
                else
                    tempRoom.GetComponent<MeshRenderer>().material = worldMaterials[1];


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

                yield return new WaitForSeconds(.022f); //Wait time until the collisions update
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
                    var tempoDoor = Instantiate(doorFace, doorPoint.transform.position, doorPoint.transform.rotation);

                    if (worldType == WorldType.Medieval)
                        tempoDoor.GetComponent<MeshRenderer>().material = worldMaterials[0];
                    else
                        tempoDoor.GetComponent<MeshRenderer>().material = worldMaterials[1];
                }
                AllPoints.AddRange(DoorPoints);
                DoorPoints.Clear();
                foreach (GameObject door in AllPoints)
                {
                    Destroy(door);
                }
                AllPoints.Clear();
            }

            yield return new WaitForSeconds(1);
        }
    }

    GameObject GetRandomRoom(Transform exitDoor)
    {
        if (isMixGen)
        {
            if (Random.value >= 0.5)
            {
                return Rooms[Random.Range(0, Rooms.Count)];
            }
            else
            {
                return Corridors[Random.Range(0, Corridors.Count)];
            }
        }
        else
        {
            if (exitDoor.parent.GetComponent<Room>().roomType == RoomType.Room)
            {
                return Corridors[Random.Range(0, Corridors.Count)];
            }
            else
            {
                return Rooms[Random.Range(0, Rooms.Count)];
            }

        }

    }
}
