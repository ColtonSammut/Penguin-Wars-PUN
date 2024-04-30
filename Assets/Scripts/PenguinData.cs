using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinData
{

    string OwnerName;
    public string ownerName { get { return OwnerName; } set { OwnerName = value; } }
    public enum PenguinState
    {
        None,
        Selected
    }

    public GameObject CurrentTile { get; set; }
    bool usable { set; get; }

    private PenguinState _PenguinState { get; set; }
    public PenguinState penguinState{ get { return _PenguinState; } set { _PenguinState = value; } }

    bool tiled;

    public bool Tiled { get { return tiled; } set { tiled = value; } }
    public PenguinData(PenguinState penguinState, bool usable, string owner, bool tiled)
    {
        _PenguinState = penguinState;
        this.usable = usable;
        OwnerName = owner;
        this.tiled = tiled;
    }
}
