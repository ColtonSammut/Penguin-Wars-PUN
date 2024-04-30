using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR;
using Button = UnityEngine.UI.Button;

public class TileManager : MonoBehaviour
{

    public Texture onefish;
    public Texture twofish;
    public Texture threefish;
    public Texture nofish;
    private PhotonView photonView;
    public TileData tileData;
    public string debug;


    public GameObject PivotTile { get; private set; }
    public GameObject[] test { get; private set; }

    private void Awake()
    {

        photonView = GetComponent<PhotonView>();

        if (!PhotonNetwork.IsConnected)
        {
            return;
        }


        if(PhotonNetwork.IsMasterClient)
        GenerateTiles();
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void GenerateTiles()
    {
        int rand = Random.Range(0, 6);

        photonView.RPC("GenerateTilesRPC",RpcTarget.All,rand);
    }

    [PunRPC]
    private void GenerateTilesRPC(int rand)
    {
        if (rand > 3)
        {
            tileData = new TileData(TileData.Tiletype.OneFish, true);
            //gameObject.GetComponent<RawImage>().color = Color.white;
            gameObject.GetComponent<RawImage>().texture = onefish;
        }
        else if (rand == 3 || rand == 4)
        {
            tileData = new TileData(TileData.Tiletype.TwoFish, true);
            //gameObject.GetComponent<RawImage>().color = Color.green;
            gameObject.GetComponent<RawImage>().texture = twofish;


        }
        else
        {
            tileData = new TileData(TileData.Tiletype.ThreeFish, true);
            //gameObject.GetComponent<RawImage>().color = Color.yellow;
            gameObject.GetComponent<RawImage>().texture = threefish;


        }
    }

    public void OnClick()
    {
        print("Clicked");
        photonView.RPC("OnClickRPC", RpcTarget.All);
    }

    [PunRPC]
    public void OnClickRPC()
    {
        if (GameData.Instance.playerTurn.ToString() == "SelectedPenguin")
        {
            if (!GameData.Instance.SelectedPenguin.GetComponent<PenguinManager>().penguinData.Tiled && tileData.TileType == TileData.Tiletype.OneFish 
                && tileData.Usable)
            {
                gameObject.GetComponent<RawImage>().texture = nofish;
                tileData.Usable = false;
                GameData.Instance.SelectedTile = gameObject;
                GameData.Instance.playerTurn = GameData.PlayerTurn.SelectedTile;
                

            }
            else if (GameData.Instance.SelectedPenguin.GetComponent<PenguinManager>().penguinData.Tiled
                && GameData.Instance.SelectedPenguin.GetComponent<PenguinManager>().AvailableTiles.Contains(gameObject))
            {

                foreach (GameObject tile in GameData.Instance.availablespaces)
                {
                    Color color = tile.GetComponent<RawImage>().color;
                    color = new Color(color.r, color.g, color.b, 1f);
                    tile.GetComponent<RawImage>().color = color;
                }

                if (GameData.Instance.State == GameData.GameState.Player1)
                {
                    if (tileData.TileType == TileData.Tiletype.OneFish)
                    {
                        GameData.Instance.Score++;
                    }
                    if (tileData.TileType == TileData.Tiletype.TwoFish)
                    {
                        GameData.Instance.Score += 2;

                    }
                    if (tileData.TileType == TileData.Tiletype.ThreeFish)
                    {
                        GameData.Instance.Score += 3;

                    }
                }
                else if (GameData.Instance.State == GameData.GameState.Player2)
                {
                    if (tileData.TileType == TileData.Tiletype.OneFish)
                    {
                        GameData.Instance.Score2++;
                    }
                    if (tileData.TileType == TileData.Tiletype.TwoFish)
                    {
                        GameData.Instance.Score2 += 2;

                    }
                    if (tileData.TileType == TileData.Tiletype.ThreeFish)
                    {
                        GameData.Instance.Score2 += 3;

                    }
                }  

                GameData.Instance.SelectedTile = gameObject;
                GameData.Instance.playerTurn = GameData.PlayerTurn.SelectedTile;
                gameObject.GetComponent<RawImage>().texture = nofish;
                tileData.Usable = false;

                
            }
        }
    }

}
