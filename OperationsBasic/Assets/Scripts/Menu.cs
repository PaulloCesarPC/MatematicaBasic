using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public void LoadScene(string name)
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene(name);
    }

    public void setOperation(int id)
    {
        PlayerPrefs.SetInt("op",id);
    }
}
