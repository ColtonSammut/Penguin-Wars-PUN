using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Pun.UtilityScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PenguinManager : MonoBehaviour
{
    PhotonView photonView;
    public PenguinData penguinData;
    public string owner;
    public List<GameObject> AvailableTiles { get; set; }


    private void Awake()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        photonView = GetComponent<PhotonView>();
        if(PhotonNetwork.IsMasterClient && owner == "Player1")
        {
            photonView.TransferOwnership(PhotonNetwork.MasterClient);
        }       
        if (!PhotonNetwork.IsMasterClient && owner == "Player2")
        {
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }

        penguinData = new PenguinData(PenguinData.PenguinState.None, true, owner, false);
    }
    
    public void OnClick()
    {
        if (GameData.Instance.playerTurn.ToString() == "Start" && GameData.Instance.State.ToString() == owner && photonView.IsMine)
            photonView.RPC("OnClickRPC", RpcTarget.All);
    }

    [PunRPC]
    public void OnClickRPC()
    {
            if (penguinData.Tiled && GameData.Instance.gamestartcount >= 8)
            {

                GameData.Instance.SelectedPenguin = gameObject;
                GameData.Instance.playerTurn = GameData.PlayerTurn.SelectedPenguin;
                AvailableTiles = Getavailabletiles();
            }
            else if (!penguinData.Tiled && GameData.Instance.gamestartcount < 8)
            {
                GameData.Instance.SelectedPenguin = gameObject;
                GameData.Instance.playerTurn = GameData.PlayerTurn.SelectedPenguin;
            }
    }

    public List<GameObject> Getavailabletiles()
    {
        List<GameObject> availableTiles = castRays(Vector2.left);
        availableTiles.AddRange(castRays(Vector2.right));
        availableTiles.AddRange(castRays(new Vector2(1, 0.75f)));
        availableTiles.AddRange(castRays(new Vector2(-1, 0.75f)));
        availableTiles.AddRange(castRays(new Vector2(1, -0.75f)));
        availableTiles.AddRange(castRays(new Vector2(-1, -0.75f)));
        return availableTiles;
    }

    private List<GameObject> castRays(Vector2 direction)
    {
        Vector3 startpos = penguinData.CurrentTile.transform.position;
        List<GameObject> tiles = new List<GameObject>();
        bool stop = false;
        GameObject hittile;
        Vector3 currentpos = startpos;

        penguinData.CurrentTile.SetActive(false);
        do
        {
            RaycastHit2D hit = Physics2D.Raycast(currentpos, direction);

            if (hit.collider != null)
            {
                hittile = hit.collider.gameObject;
                if (hittile.GetComponent<TileManager>().tileData.Usable)
                {
                    hittile.SetActive(false);
                    currentpos = hit.collider.transform.position;
                    tiles.Add(hittile);
                }
                else
                {
                    stop = true;
                }
            }
            else
            {
                stop = true;
            }   

        } while(!stop);

        foreach(GameObject tile in tiles)
        {
            tile.SetActive(true);
            GameData.Instance.availablespaces.Add(tile);
            if(GameData.Instance.playerTurn == GameData.PlayerTurn.SelectedPenguin)
            {
                Color color = tile.GetComponent<RawImage>().color;
                color = new Color(color.r, color.g, color.b, 0.5f);
                tile.GetComponent<RawImage>().color = color;
            }

        }
          penguinData.CurrentTile.SetActive(true);


        return tiles;
    }
}

