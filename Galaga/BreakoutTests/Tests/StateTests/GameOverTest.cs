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
using BlockTypes.teleporting;
using BlockTypes.hardened;

using States.GameOver;
using States.GamePaused;
using States.GameRunning;
using States.MainMenu;

using GetGameBus;

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

public class gameOverTest
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
        startPoint = new game(windowArgs); 
        stateHandler = new stateMachine();
        playerEnt = GameRunning.getOrSetInstance(stateHandler).getPlayer;

        menu = new MainMenu(stateHandler);
        gameIsOver = new GameOver(stateHandler);
        gameRunning = new GameRunning(stateHandler);
        pause = new GamePaused(stateHandler);

    }

    [Test]

    public void sendUp(){
        stateHandler.ActiveState = gameIsOver;
        Assert.DoesNotThrow(() => {gameIsOver.HandleKeyEvent(KeyboardAction.KeyPress,KeyboardKey.Down);});
        Assert.DoesNotThrow(() => {gameIsOver.HandleKeyEvent(KeyboardAction.KeyPress,KeyboardKey.Up);});
    }

    [Test]

    public void sendDown(){
        stateHandler.ActiveState = gameIsOver;
        Assert.DoesNotThrow(() => {gameIsOver.HandleKeyEvent(KeyboardAction.KeyPress,KeyboardKey.Down);});
    }
    [Test]

    public void sendEnterFirstIf(){
        stateHandler.ActiveState = gameIsOver;
        Assert.DoesNotThrow(() => {gameIsOver.HandleKeyEvent(KeyboardAction.KeyPress,KeyboardKey.Up);});
        Assert.DoesNotThrow(() => {gameIsOver.HandleKeyEvent(KeyboardAction.KeyPress,KeyboardKey.Enter);});
        Assert.True(Convert.ToString(stateHandler.ActiveState) is "States.GameRunning.GameRunning");
    }
    
    [Test]

    public void sendEnterElse(){
        stateHandler.ActiveState = gameIsOver;
        gameIsOver.HandleKeyEvent(KeyboardAction.KeyPress,KeyboardKey.Down);
        Assert.DoesNotThrow(() => {gameIsOver.HandleKeyEvent(KeyboardAction.KeyPress,KeyboardKey.Enter);});
        Assert.True(startPoint.getClosed == true);
    }

    [Test]

    public void sendRelease(){
        stateHandler.ActiveState = gameIsOver;
        gameIsOver.HandleKeyEvent(KeyboardAction.KeyPress,KeyboardKey.Down);
        Assert.DoesNotThrow(() => {gameIsOver.HandleKeyEvent(KeyboardAction.KeyRelease,KeyboardKey.Enter);});
    }

    [Test]

    public void renderStateTest(){
        stateHandler.ActiveState = gameIsOver;
        Assert.DoesNotThrow(() => {gameIsOver.RenderState();});
    }

    [Test]
    public void instanceTest(){
        Assert.DoesNotThrow(() => {stateHandler.ActiveState = GameOver.getOrSetInstance(stateHandler);});
    }

}