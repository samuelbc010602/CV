namespace BreakoutTests;

using NUnit.Framework;
using Statemachine;
using States.GameRunning;

using System;

using DIKUArcade.GUI;
using DIKUArcade.Entities;
using DIKUArcade.Events;
using DIKUArcade.Math;
using DIKUArcade.State;
using DIKUArcade.Input;

using ASCIILoader;
using Entities.Block;
using Player;
using Game;

using States.GameOver;
using States.GamePaused;
using States.GameRunning;
using States.MainMenu;

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

public class StateTests
{
    private stateMachine stateHandler;
    private game startPoint;
    private player playerEnt;

    private MainMenu menu;
    private GameOver gameIsOver;
    private GameRunning gameRunning;
    private GamePaused pause;
    [SetUp]
    public void Setup()
    {
        var windowArgs = new WindowArgs() { Title = "Galaga v0.1" };
        game startPoint = new game(windowArgs); 
        stateHandler = new stateMachine();
        playerEnt = GameRunning.getOrSetInstance(stateHandler).getPlayer;

        menu = new MainMenu(stateHandler);
        gameIsOver = new GameOver(stateHandler);
        gameRunning = new GameRunning(stateHandler);
        pause = new GamePaused(stateHandler);

    }

    [Test]
    
    public void checkIfIGameState(){

        GameOver testState1 = new GameOver(stateHandler);
        GamePaused testState2 = new GamePaused(stateHandler);
        GameRunning testState3 = new GameRunning(stateHandler);
        MainMenu testState4 = new MainMenu(stateHandler);

        Assert.True(testState1 is IGameState == true);
        Assert.True(testState2 is IGameState == true);
        Assert.True(testState3 is IGameState == true);
        Assert.True(testState4 is IGameState == true);
    }

    [Test]
    public void stateSequence(){
        Assert.True(stateHandler.ActiveState is MainMenu == true);
        stateHandler.ActiveState.HandleKeyEvent(KeyboardAction.KeyPress,KeyboardKey.Up);
        stateHandler.ActiveState.HandleKeyEvent(KeyboardAction.KeyPress,KeyboardKey.Enter);

        //stateHandler.ActiveState.getHealth = 0;
        //stateHandler.ActiveState.UpdateState();

        
        
        //stateHandler.ActiveState.HandleKeyEvent(KeyboardAction.KeyPress,KeyboardKey.D);


    }

}


    
