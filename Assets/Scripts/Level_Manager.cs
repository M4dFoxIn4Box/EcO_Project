using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level_Manager : MonoBehaviour
{

    public static Level_Manager instance = null;

    public List<GameObject> civilList;
    private GameObject target;
    private int index;
    public List<Civil_Scriptable> civilType;
    private int randomIdx;
    public GameObject playerGO;
    private bool canPause = false;

    [Header("Timer Settings")]
    public float seconds;
    public float minutes;
    public Text timer;
    private bool civilKilled = false;

    [Header("Challenge")]
    public GameObject challengesUI;
    public Toggle timerToggle;
    public Toggle shotsToggle;
    public Toggle targetsToggle;    
    public Text[] challengeList;
    public string[] challengeCondition;
    public enum challengeType { Timer, Targets, Shots}
    public challengeType[] type;

    private int shotNumber = 0;

    [Header("Challenges Conditions")]
    public float targetSeconds;
    public float targetMinutes;
    public int shotLimit;
    private bool targetKilled = false;
    public int secretTargetTotal;

    [Header("Pause")]

    public GameObject pauseMenu;
    private bool isPaused = false;

    [Header("Mission")]

    public Text missionTxt;
    public string[] missionMessage;
    public Text missionLvlTxt;
    public int missionLvlNumber;

    [Header("EndMenu")]
    public GameObject endMenu;
    public GameObject nextMission;


    void Awake()
    {
        for (int a = 0; a < challengeList.Length; a++)
        {
            challengeList[a].text = challengeCondition[a];
        }

       
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        index = Random.Range(0, civilList.Count);
        target = civilList[index];
        target.gameObject.tag = "Target";

        for (int i = 0; i < civilList.Count;)
        {
            randomIdx = Random.Range(0, civilList.Count);
            civilList[i].gameObject.GetComponent<Civil_New>().civilComponents = civilType[randomIdx];
            civilType.Remove(civilType[randomIdx]);
            civilList.Remove(civilList[i]);
        }

    }

    public void Start()
    {
        missionLvlTxt.text = "Target" + " " + missionLvlNumber;
        playerGO.GetComponent<Scope>().enabled = false;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = (true);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause") && !isPaused && canPause)
        {
            Paused();
        }
        else if (Input.GetButtonDown("Pause") && isPaused && canPause)
        {
            UnPaused();
        }

        if (!civilKilled)
        {
            CountDown();
        }

        if (seconds <= 9.5)
        {
            timer.text = minutes.ToString("F0") + " : " + "0" + seconds.ToString("F0");
        }
        else
        {
            timer.text = minutes.ToString("F0") + " : " + seconds.ToString("F0");
        }
    }

    public void CountDown()
    {
        if (seconds > 0)
        {
            seconds -= Time.deltaTime;
        }

        else if (seconds <= 0 && minutes > 0)
        {
            seconds = 59;
            minutes--;
        }

        if (seconds <= 0 && minutes <= 0)
        {
            playerGO.GetComponent<Scope>().enabled = false;
            StartCoroutine("YouLose");
        }
    }

    public void KillSomeone(GameObject victim)
    {
        if(victim.CompareTag("Target"))
        {
            targetKilled = !targetKilled;
            for (int d = 0; d < type.Length; d++)
            {
                if (type[d] == challengeType.Timer)
                {
                    TimerChallenge();
                }
            }

            for (int d = 0; d < type.Length; d++)
            {
                if (type[d] == challengeType.Targets)
                {
                    TargetsChallenge(+0);
                }
            }

            for (int d = 0; d < type.Length; d++)
            {
                if (type[d] == challengeType.Shots)
                {
                    ShotsChallenge(+0);
                }
            }
            StartCoroutine("YouWin");
        }
        else
        {
            civilKilled = !civilKilled;
            StartCoroutine("YouLose");
        }
    }

    public void MissionStart()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = (false);
        playerGO.GetComponent<Scope>().enabled = true;
        canPause = true;
    }

    IEnumerator YouWin()
    {
        canPause = !canPause;
        yield return new WaitForSeconds(4f);
        challengesUI.SetActive(true);
        missionTxt.text = missionMessage[0]; // Display the message "mission completed".
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = (true);
        endMenu.SetActive(true);
        nextMission.SetActive(true);
    }

    IEnumerator YouLose()
    {
        canPause = !canPause;
        yield return new WaitForSeconds(4f);
        challengesUI.SetActive(true);
        missionTxt.text = missionMessage[1]; // Display the message "mission failed".
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = (true);
        endMenu.SetActive(true);
    }

    public void Death ()
    {
        canPause = !canPause;
        missionTxt.text = missionMessage[1]; // Display the message "mission failed".
        challengesUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = (true);
        endMenu.SetActive(true);
    }

    public void Paused()
    {
        pauseMenu.SetActive(true);
        challengesUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = !isPaused;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = (true);
        playerGO.GetComponent<Scope>().enabled = false;
        Debug.Log(isPaused);
    }

    public void UnPaused()
    {
        pauseMenu.SetActive(false);
        challengesUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = (false);
        Time.timeScale = 1f;
        isPaused = !isPaused;
        playerGO.GetComponent<Scope>().enabled = true;
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void TimerChallenge()
    {
        civilKilled = !civilKilled;
        if (targetMinutes > minutes)
        {
                        
        }
        else if (targetMinutes >= minutes && targetSeconds > seconds)
        {

        }
        else
        {
            timerToggle.isOn = true;
        }
    }

    public void TargetsChallenge(int secretTargetDestroyed)
    {
        secretTargetTotal -= secretTargetDestroyed;
        if(secretTargetTotal == 0 && targetKilled)
        {
            targetsToggle.isOn = true;
            Debug.Log(secretTargetTotal);
        }
    }

    public void ShotsChallenge(int shots)
    {
        shotNumber = shotNumber + shots;
        if(shotNumber < shotLimit && targetKilled)
        {
            shotsToggle.isOn = true;
            Debug.Log(shotNumber);
        }
    }
}


