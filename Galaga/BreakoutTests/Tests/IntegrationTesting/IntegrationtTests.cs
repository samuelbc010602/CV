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
using Entities.Ball;
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

public class Integrationtests
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
    /*
    //Integration with powerups
    public void placePowerUps(){
        stateHandler.ActiveState = gameRunning;
        gameRunning.makeLevel("level4.txt");
        for(int i = gameRunning.getBlocks.Count-1;i>=0;i--){
            gameRunning.getBlocks[i].hit(201,gameRunning.getBalls[0]);
        }
        gameRunning.RenderState();
        Console.WriteLine(gameRunning.getCurrentPowerUpBalls.Count);
        foreach(ball i in gameRunning.getBalls){
            i.Shape.AsDynamicShape().Position.X = gameRunning.getPlayer.Shape.AsDynamicShape().Position.X;
            i.Shape.AsDynamicShape().Position.Y = gameRunning.getPlayer.Shape.AsDynamicShape().Position.Y;
            Console.WriteLine("before");
            Console.WriteLine(gameRunning.getPlayer.Shape.AsDynamicShape().Position.Y);
            Console.WriteLine(i.Shape.AsDynamicShape().Position.Y);
            Console.WriteLine("after");
            i.ballMove();
        }
        Console.WriteLine(gameRunning.getBalls.Count);
        gameRunning.RenderState();
        Assert.True(gameRunning.getCurrentPowerUpBalls.Count == 50);
        Assert.True(gameRunning.getCurrentPowerUpBalls.Count == 23);
        

        
    }
    */


}