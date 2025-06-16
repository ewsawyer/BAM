using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public bool isGameRunning { get; private set; }

    [SerializeField] private PlayerInputManager InputManager;
    
    [Header("Start of game")] 
    [SerializeField] private GameObject StartGameUI;
    
    [Header("End of game")]
    [SerializeField] private TextMeshProUGUI WinnerNameDisplay;
    [SerializeField] private TextMeshProUGUI WinTextDisplay;
    [SerializeField] private TextMeshProUGUI RestartText;

    private bool _canRestart;

    private void Awake()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        _canRestart = false;
        StartGameUI.SetActive(true);
        WinnerNameDisplay.gameObject.SetActive(false);
        WinTextDisplay.gameObject.SetActive(false);
        RestartText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene("GAME");
        else if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void OnStart()
    {
        if (_canRestart)
            RestartGame();
        else if (!isGameRunning)
            StartGame();
    }

    public void StartGame()
    {
        InputManager.DisableJoining();
        foreach (WeaponRandomizer wr in FindObjectsOfType<WeaponRandomizer>())
            wr.enabled = true;
        foreach (WeaponsController wc in FindObjectsOfType<WeaponsController>())
            wc.enabled = true;
        
        StartGameUI.SetActive(false);
        isGameRunning = true;
    }

    public void RestartGame()
    {
        if (!_canRestart)
            return;

        SceneManager.LoadScene("GAME");
    }
    
    public void EndGame()
    {
        StartCoroutine(EndGameCoroutine());
    }

    private IEnumerator EndGameCoroutine()
    {
        isGameRunning = false;
        _canRestart = false;
        
        // Check which player is still alive
        GameObject winner = PlayerJoinManager.Instance.GetPlayer(0);
        
        // Make them invulnerable
        winner.GetComponent<PlayerHealth>().IsVulnerable = false;
        
        // Get their color
        Color color = PlayerJoinManager.Instance.GetColor(winner);
        
        // Display a banner for them
        WinnerNameDisplay.text = winner.name;
        WinnerNameDisplay.color = color;
        WinTextDisplay.gameObject.SetActive(true);
        WinnerNameDisplay.gameObject.SetActive(true);

        // Wait a bit and then let them restart
        yield return new WaitForSeconds(1.5f);
        _canRestart = true;
        RestartText.gameObject.SetActive(true);
    }
}
