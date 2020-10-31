using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{

    //This MUST be done differently!
    [SerializeField] Animator UIAnimator;
    [SerializeField] Animator CameraAnimator;
    bool isCredit;


    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowCredits()
    {
        isCredit = !isCredit;
        UIAnimator.SetBool("ToCredit", isCredit);
        CameraAnimator.SetBool("ToCredit", isCredit);

    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
