using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameData : MonoBehaviour
{
    
    private static GameData instance;

    public int gamestartcount;
    public static GameData Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField]
    private GameObject[] Board = new GameObject[49];

    public GameObject[,] board
    {
        get 
        {
            GameObject[,] board = new GameObject[7, 7];
            for (int i = 0; i < 7; i++)
                for (int j = 0; j < 7; j++)
                {
                    board[i, j] = Board[i + j];
                    board[i, j].GetComponent<TileManager>().tileData.Tileposx = i;
                    board[i, j].GetComponent<TileManager>().tileData.Tileposy = j;
                }
            
            return board;
        }
    }

    public enum GameState
    {
        Player1,
        Player2,
        Win,
        Lose,
        Draw
    }

    public enum PlayerTurn
    {
        Start,
        SelectedPenguin,
        SelectedTile,
        Finished
    }

    public PlayerTurn playerTurn{ get; set; }

    public GameState State { get; set; }

    public GameObject SelectedPenguin { get; set;}

    public GameObject SelectedTile { get; set;}

    public List<GameObject> availablespaces;

    public int Score { get; set; }
    public int Score2 { get; set; }

}


