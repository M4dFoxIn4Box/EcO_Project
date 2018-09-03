using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Level_Select : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void LevelToLoad(int level)
    {
        SceneManager.LoadScene(level);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = (false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
