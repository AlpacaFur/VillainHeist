using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using movement_and_Camera_Scripts;

public class EscapeTimer : MonoBehaviour
{
    public static EscapeTimer Instance;
    private PlayerController player;
    public float totalSecondsRemaining;
    [SerializeField] private float totalTime = 60;
    private bool useTimer = false;

    private GameUI _gameUI;
    
    /*private void Awake()
    {
        if (exists)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            exists = true;
        }
    }*/

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Timer Start Method");
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            _gameUI = FindObjectOfType<GameUI>();
            Instance = this;
        }
        totalSecondsRemaining = totalTime;
        _gameUI.HideTimer();
        //StartCoroutine(researchForGameUI());
    }

    /*private IEnumerator researchForGameUI()
    {
        while (true)
        {
            yield return new WaitUntil(() => _gameUI == null);
            _gameUI = FindObjectOfType<GameUI>();
        }
    }*/

    /*private IEnumerator waitUntilEscape()
    {
        this.player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerController>();
        Debug.Log("begin start timer");
        yield return new WaitUntil(() => !player.isFirstPov);
        Debug.Log("begin start timer 2");
        yield return new WaitUntil(() => player.isFirstPov);
        Debug.Log("start timer");
        useTimer = true;
        showTimer(true);
        StartCoroutine(timerUI());
    }*/

    public void startTimer()
    {
        useTimer = true;
        _gameUI.SetTimer((int)totalSecondsRemaining);
    }

    private void reset()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerController>();

        //player.ToPov(false);
        GameState.Instance.resetToTopDown();

        SceneManager.LoadScene("FinalRoom");
        useTimer = false;
        Debug.Log("reset timer");
        _gameUI.HideTimer();
        StopAllCoroutines();
        totalSecondsRemaining = totalTime;
        //StartCoroutine(waitUntilEscape());
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameUI == null)
        {
            _gameUI = FindObjectOfType<GameUI>();
        }
        if (useTimer)
        {
            updateTimer();
            _gameUI.SetTimer((int)totalSecondsRemaining);
            checkEnd();
        }
    }

    private void updateTimer()
    {
        totalSecondsRemaining -= Time.deltaTime;
    }

    private void checkEnd()
    {
        if (totalSecondsRemaining <= 0)
        {
            // Failure Script
            //Time.timeScale = 0;
            // To be implemented
            reset();
        }
    }
}
