using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{

    //This MUST be done differently!
    [SerializeField] Animator UIAnimator;
    [SerializeField] Animator CameraAnimator;
    [SerializeField] AudioSource audioSource;

    bool isCredit;


    public void PlayGame()
    {
        PlayClick();
        SceneManager.LoadScene(1);
    }

    public void ShowCredits()
    {
        isCredit = !isCredit;
        UIAnimator.SetBool("ToCredit", isCredit);
        CameraAnimator.SetBool("ToCredit", isCredit);
        PlayClick();

    }

    public void Quit()
    {
        PlayClick();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    void PlayClick() 
    {
        audioSource.Play();
    }


}
