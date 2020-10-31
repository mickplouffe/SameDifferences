using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    public Transform Player;
    public float MoveSpeed;
    public float ReactionDistance;

    public AudioSource MonsterSounds;
    public AudioClip RelaxedSound;
    public AudioClip AngrySound;
    private bool Awake;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, Player.position) <= ReactionDistance) {
            if (Awake != true) {
                MonsterSounds.clip = AngrySound;
                MonsterSounds.Play();
                
                Awake = true;
            }
            
            transform.LookAt(Player);

            transform.position += transform.forward*MoveSpeed*Time.deltaTime;
        }
    }
}
