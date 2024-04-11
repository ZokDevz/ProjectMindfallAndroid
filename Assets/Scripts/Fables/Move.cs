using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public MoveBase Base { get; set; }
    private int pp;

    public int PP
    {
        get { return pp; }
        set { pp = value; }
    }

    public Move(MoveBase pBase)
    {
        Base = pBase;
        PP = pBase.PP;
    }
}



