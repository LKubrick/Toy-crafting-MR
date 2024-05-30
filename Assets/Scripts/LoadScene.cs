using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void RobotScene()
    {
        SceneManager.LoadScene("ToyController");
    }    
    public void AirplaneScene()
    {
        SceneManager.LoadScene("AirplaneController");
    }
}
