using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyEventState
{
    public WorldState.KeyEvent keyEvent; 
    public int state;

    public KeyEventState(WorldState.KeyEvent keyEvent, int state)
    {
        this.keyEvent = keyEvent;
        this.state = state;
    }
}
