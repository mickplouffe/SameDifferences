using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundsScript : MonoBehaviour
{
    public AudioSource Feet;
    public AudioSource SoundPlayer;
    public CharacterController Character;
    public PlayerController Controller;

    public AudioSource[] Talking;
    public AudioSource[] Pickup;
    public AudioSource[] Footsteps;

    private float WalkingStepTime;
    
    public IEnumerator FootStepsPlayer() {
        float WalkingSpeed = Controller.walkingSpeed;

        while (true) {
            float CurrentSpeed = Controller.curSpeedX + Controller.curSpeedY;

            if (Character.isGrounded) {
                if (CurrentSpeed != 0) {
                    if (CurrentSpeed <= WalkingSpeed) {
                        WalkingStepTime = 0.7f;
                    } else {
                        WalkingStepTime = 0.5f;
                    }

                    Debug.Log("Insert Footstep Sound");
                    yield return new WaitForSeconds(WalkingStepTime);
                } else {
                    yield return new WaitForSeconds(0.1f);
                }
            } else {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    void Start() {
        StartCoroutine("FootStepsPlayer");
    }
}