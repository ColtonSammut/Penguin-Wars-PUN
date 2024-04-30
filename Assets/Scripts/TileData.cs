using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData
{
    public int Tileposx;
    public int Tileposy;
   public enum Tiletype
    {
        OneFish,
        TwoFish,
        ThreeFish,
    }



   public bool Usable{ get; set; } 
    public Tiletype TileType{ get; set; }

    public TileData(Tiletype tiletype, bool usable)
    {
        TileType = tiletype;
        Usable = usable;
    }
}
