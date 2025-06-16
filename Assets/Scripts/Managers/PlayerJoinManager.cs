using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinManager : MonoBehaviour
{
    public static PlayerJoinManager Instance;
    
    [Tooltip("The colors to use for the players")] [SerializeField]
    private Color[] PlayerColors;

    [Tooltip("The names to use for the players")] [SerializeField]
    private string[] PlayerNames;

    [Tooltip("The player prefab to instantiate")] [SerializeField]
    private GameObject PlayerPrefab;
    
    public int NumPlayers { get; private set; }

    private List<GameObject> _players;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _players = new List<GameObject>();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Remove(GameObject player)
    {
        if (_players.Contains(player))
            _players.Remove(player);
    }

    public GameObject GetPlayer(int index)
    {
        return _players[index];
    }

    public Color GetColor(GameObject player)
    {
        return PlayerColors[Array.IndexOf(PlayerNames, player.name)];
    }

    public int ActivePlayers()
    {
        return _players.Count;
    }

    public void RegisterPlayer(GameObject player)
    {
        if (_players.Contains(player))
            return;
        
        NumPlayers++;
        
        // Set its color
        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        sr.color = PlayerColors[NumPlayers-1];
        
        // Set its name in the editor
        player.name = PlayerNames[NumPlayers-1];
        
        // Add it to the list of players
        _players.Add(player);
    }
}
