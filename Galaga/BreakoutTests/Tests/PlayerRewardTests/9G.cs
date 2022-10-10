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

using BlockTypes.hardened;

public class RewardTest
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
    public void testIfPositive(){
        Assert.True(gameRunning.getPoints is uint);
    }
    [Test]
    public void pointsAwardedTest(){
        stateHandler.ActiveState = gameRunning;
        gameRunning.makeLevel("level4.txt");
        foreach(block i in gameRunning.getBlocks){
            if(i is Hardened){
                i.hit(200,gameRunning.getBalls[0]);
                break;
            }
        }
        gameRunning.RenderState();
        Console.WriteLine(gameRunning.getPoints);
        Assert.True(gameRunning.getPoints == 2);
    }







}