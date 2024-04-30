using Newtonsoft.Json;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    PhotonView photonView;

    GameData gamedata;
    GameObject StatusText;
    public Text ScoreText { get; private set; }
    public int player1penguincount { get; private set; }
    public int player2penguincount { get; private set; }

    private void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene(0);
        }
        photonView = GetComponent<PhotonView>();
        player1penguincount = 0;
        gamedata = GameData.Instance;
        gamedata.gamestartcount = 0;
        gamedata.State = GameData.GameState.Player1;
        StatusText = GameObject.Find("StatusText");
        gamedata.playerTurn = GameData.PlayerTurn.Start;
        gamedata.Score = 0;
        gamedata.Score2 = 0;
        ScoreText = GameObject.Find("Score").GetComponent<Text>();
    }

    private void Update()
    {
        if (gamedata.playerTurn != GameData.PlayerTurn.Finished){
            StatusText.GetComponent<Text>().text = $"{gamedata.State.ToString()} Turn {gamedata.playerTurn.ToString()}";
        }
        else
        {
            if (gamedata.Score > gamedata.Score2)
            {
                StatusText.GetComponent<Text>().text = "Player 1 Wins";
            }
            else if (gamedata.Score2 > gamedata.Score)
            {
                StatusText.GetComponent<Text>().text = "Player 2 Wins";
            }
            else if (gamedata.Score2 == gamedata.Score)
            {
                StatusText.GetComponent<Text>().text = "Draw";

            }
            }
        if (gamedata.playerTurn == GameData.PlayerTurn.SelectedTile && gamedata.playerTurn != GameData.PlayerTurn.Finished)
        {
            photonView.RPC("SetPenguinRPC", RpcTarget.All);
           
            if (gamedata.gamestartcount > 8)
                photonView.RPC("CheckPenguinsRPC", RpcTarget.All);


            if (gamedata.State == GameData.GameState.Player1)
                gamedata.State = GameData.GameState.Player2;
            else
                gamedata.State = GameData.GameState.Player1;


            if (player1penguincount == 4 || player2penguincount == 4)
            {
                gamedata.playerTurn = GameData.PlayerTurn.Finished;

            }
            else
            {
                gamedata.playerTurn = GameData.PlayerTurn.Start;
                gamedata.gamestartcount++;
            }

        }

        if(gamedata.State == GameData.GameState.Player1)
        {
            ScoreText.text = $"{GameData.GameState.Player1.ToString()}: {gamedata.Score}";
        }
        if(gamedata.State == GameData.GameState.Player2)
        {
            ScoreText.text = $"{GameData.GameState.Player2.ToString()}: {gamedata.Score2}";
        }

    }

    [PunRPC]
    private void CheckPenguinsRPC()
    {
        GameObject[] penguins = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject penguin in penguins)
        {
            if(penguin.GetComponent<PenguinManager>().Getavailabletiles().Count == 0)
            {
                if (penguin.GetComponent<PenguinManager>().penguinData.ownerName == "Player1") 
                {
                    player1penguincount++;
                }
                if (penguin.GetComponent<PenguinManager>().penguinData.ownerName == "Player2")
                {
                    player2penguincount++;
                }

                penguin.SetActive(false);
            }
        }
    }

    [PunRPC]
    public void SetPenguinRPC()
    {
        gamedata.SelectedPenguin.transform.position = gamedata.SelectedTile.transform.position;
        gamedata.SelectedPenguin.GetComponent<PenguinManager>().penguinData.Tiled = true;
        gamedata.SelectedPenguin.GetComponent<PenguinManager>().penguinData.CurrentTile = gamedata.SelectedTile;
    }
}
