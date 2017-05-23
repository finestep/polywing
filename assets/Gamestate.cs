using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Gamestate : MonoBehaviour {
    

    GameObject[] ships;

    int shipCount;

    bool over;

    // Use this for initialization
    void Start () {

        ships = GameObject.FindGameObjectsWithTag("Ship");

        shipCount = ships.Length;

        over = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        if (over) return;

        ships = GameObject.FindGameObjectsWithTag("Ship");

        if (ships.Length == 1) {
            Invoke("Restart", 3f);
            over = true;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("devscene");
    }
}
