using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public static void LoadBiezierCurve()
    {
        SceneManager.LoadScene(0); //Change after merge
    }
    public static void LoadLatticeDeform()
    {
        SceneManager.LoadScene("LatticeDeform");        
    }

    public static void LoadRGBMixer()
    {
        SceneManager.LoadScene(0); //Change after merge
    }
}
