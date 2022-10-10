namespace Statemachine;

using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Input;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;
using DIKUArcade.Physics;
using DIKUArcade.State;

using System;
using System.Collections.Generic;

using States.GameRunning;
using States.MainMenu;
using Entities.Block;

///<summary>
///The statemachine responsible for switching between different states of the game
///</summary>
public class stateMachine{

    private IGameState activeState;
    public IGameState ActiveState{
        get{return activeState;}
        set{activeState = value;}
    }

    public stateMachine(){
        activeState = MainMenu.getOrSetInstance(this);
    }
}


