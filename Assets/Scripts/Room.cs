using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] float roomLength = 10;
    public bool isCollidingOtherRoom;
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject[] exits;
    [SerializeField] BoxCollider[] colliders;

    public float GetRoomLenght()
    {
        return roomLength;
    }

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
