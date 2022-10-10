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

public class gameRuntests
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
    //Integration with powerups
    public void placePowerUps(){
        stateHandler.ActiveState = gameRunning;
        gameRunning.makeLevel("level4.txt");
        for(int i = gameRunning.getBlocks.Count-1;i>=0;i--){
            gameRunning.getBlocks[i].hit(201,gameRunning.getBalls[0]);
        }
        gameRunning.RenderState();
        Console.WriteLine(gameRunning.getCurrentPowerUpBalls.Count);
        Assert.True(gameRunning.getCurrentPowerUpBalls.Count == 23);
    }

    [Test]
    public void spawningNewBall(){
        stateHandler.ActiveState = gameRunning;
        gameRunning.makeLevel("level4.txt");
        /*for(int i = gameRunning.getBlocks.Count-1;i>=1;i--){
            gameRunning.getBlocks[i].hit(201,gameRunning.getBalls[0]);
        }*/
        gameRunning.getBalls[0].GetTakeLife = true;
        gameRunning.RenderState();
        Assert.AreEqual(gameRunning.getBalls.Count,1);
    }
    /*
    [Test]
    public void increasingLevel(){
        stateHandler.ActiveState = gameRunning;
        int prevLevel = gameRunning.getCurrentLevel;
        for(int i = gameRunning.getBlocks.Count-1;i>=0;i--){
            gameRunning.getBlocks[i].hit(201,gameRunning.getBalls[0]);
        }
        gameRunning.gameLogic();
        Assert.AreNotEqual(1,gameRunning.getCurrentLevel);
    }
    */
    [Test]
    public void sendA(){
        gameRunning.HandleKeyEvent(KeyboardAction.KeyPress,KeyboardKey.A);
        Console.WriteLine(gameRunning.getPlayer.Shape.Position.X);
        getGameBus.GetBus().ProcessEvents();
        gameRunning.getPlayer.move();
        Assert.True(gameRunning.getPlayer.Shape.Position.X < 0.5f);
    }
    [Test]
    public void sendD(){
        gameRunning.HandleKeyEvent(KeyboardAction.KeyPress,KeyboardKey.D);
        Console.WriteLine(gameRunning.getPlayer.Shape.Position.X);
        getGameBus.GetBus().ProcessEvents();
        gameRunning.getPlayer.move();
        Assert.True(gameRunning.getPlayer.Shape.Position.X > 0.5f);
    }
    [Test]
    public void sendEscape(){
        stateHandler.ActiveState = gameRunning;
        gameRunning.HandleKeyEvent(KeyboardAction.KeyPress,KeyboardKey.Escape);
        Console.WriteLine(gameRunning.getPlayer.Shape.Position.X);
        getGameBus.GetBus().ProcessEvents();
        Console.WriteLine(stateHandler.ActiveState);
        Assert.True(Convert.ToString((stateHandler.ActiveState.GetType())) == "States.GamePaused.GamePaused");
    }

    [Test]
    public void sendARelease(){
        stateHandler.ActiveState = gameRunning;
        gameRunning.HandleKeyEvent(KeyboardAction.KeyRelease,KeyboardKey.A);
        Console.WriteLine(gameRunning.getPlayer.Shape.Position.X);
        getGameBus.GetBus().ProcessEvents();
        Console.WriteLine(stateHandler.ActiveState);
        Assert.True(gameRunning.getPlayer.getMoveLeft == 0.0f);
    }
    [Test]
    public void sendDRelease(){
        stateHandler.ActiveState = gameRunning;
        gameRunning.HandleKeyEvent(KeyboardAction.KeyRelease,KeyboardKey.D);
        Console.WriteLine(gameRunning.getPlayer.Shape.Position.X);
        getGameBus.GetBus().ProcessEvents();
        Console.WriteLine(stateHandler.ActiveState);
        Assert.True(gameRunning.getPlayer.getMoveRight == 0.0f);
    }
}