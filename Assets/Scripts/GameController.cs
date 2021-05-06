using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameController : MonoBehaviour {

    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public Text scoreText;
    public Text restartText;
    public Text gameOverText;
    public GameObject optionsPanel;
    public AudioMixerSnapshot pausedSnapshot;
    public AudioMixerSnapshot unpausedSnapshot;

    private bool gameOver;
    //private bool restart;
    private int score;
    private bool paused;

	// Use this for initialization
	void Start () {
        gameOver = false;
        paused = false;
        //restart = false;
        restartText.text = "";
        gameOverText.text = "";
        score = 0;
        UpdateScore();
        StartCoroutine (SpawnWaves());
	}

    private void Update()
    {
        /*
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Main");
                //Application.LoadLevel(Application.loadedLevel);
            }
        }
        */
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Pause"))
        {
            GamePaused();
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (!gameOver)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];

                /*
                 * bool flag = (Random.value > 0.5);
                 * if(flag){
                 *  spawnValues.z (16 or -16)
                 * }
                */

                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                // GameObject clone = Instantiate(hazard, spawnPosition, spawnRotation);
                // ReverseDirection(clone);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);

            //if (gameOver)
            //{
            //    restartText.text = "Press 'R' for Restart";
            //    restart = true;
            //    break;
            //}
        }
        //restartText.text = "Press 'R' for Restart";
        //restart = true;
    }

    /*
     * void ReverseDirection(GameObject clone){
     *  clone.transform.rotation.y = 0;
     *  clone.GetComponent<Mover>().speed = 5;
     * }
    */

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void GamePaused()
    {
        paused = !paused;
        if (paused)
        {
            Time.timeScale = 0;
            pausedSnapshot.TransitionTo(.01f);
            optionsPanel.SetActive(true);
            foreach (Transform child in optionsPanel.transform)
            {
                if (child.name == "Resume") child.gameObject.GetComponent<Button>().Select();
            }
        }
        else
        {
            Time.timeScale = 1;
            unpausedSnapshot.TransitionTo(.01f);
            optionsPanel.SetActive(false);
        }
    }

    public void Restart()
    {
        if (paused) GamePaused();
        SceneManager.LoadScene("Main");
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over";
        gameOver = true;
        optionsPanel.SetActive(true);
        foreach (Transform child in optionsPanel.transform)
        {
            if (child.name == "Resume") child.gameObject.GetComponent<Button>().enabled = false;
            if (child.name == "Restart") child.gameObject.GetComponent<Button>().Select();
        }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
