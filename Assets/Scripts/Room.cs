using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    Room,
    Corridor,
    Special
}

public class Room : MonoBehaviour
{
    public RoomType roomType;
    public bool isCollidingOtherRoom;
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject[] exits;
    [SerializeField] BoxCollider[] colliders;


    public void RemoveRb()
    {
        Destroy(rb);        
    }

    public void RemoveColliders()
    {
        foreach (var collider in colliders)
        {
            Destroy(collider);
        }
    }
    public void EnableExits()
    {
        foreach (var exit in exits)
        {
            exit.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        isCollidingOtherRoom = true;
    }

    private void OnTriggerExit(Collider other)
    {

        isCollidingOtherRoom = false;
    }
}
