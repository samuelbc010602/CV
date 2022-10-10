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
using DIKUArcade.Graphics;

using ASCIILoader;
using Entities.Block;
using Player;
using Game;
using Facade;
using GetGameBus;

using States.GameOver;
using States.GamePaused;
using States.GameRunning;
using States.MainMenu;

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

using BallTypes.normalBall;
using BallTypes.powerUpBall;

using BlockTypes.invinsible;
using BlockTypes.hungry;
using BlockTypes.teleporting;
using BlockTypes.unbreakable;
public class unbreakableTest
{
    private stateMachine stateHandler;
    private game startPoint;
    private player playerEnt;

    private MainMenu menu;
    private GameOver gameIsOver;
    private GameRunning gameRunning;
    private GamePaused pause;
    private facade dikuArcadeBiding;
    [SetUp]
    public void Setup()
    {
        var windowArgs = new WindowArgs() { Title = "Galaga v0.1" };
        game startPoint = new game(windowArgs); 
        stateHandler = new stateMachine();
        

        menu = new MainMenu(stateHandler);
        gameIsOver = new GameOver(stateHandler);
        gameRunning = new GameRunning(stateHandler);
        playerEnt = gameRunning.getPlayer;
        pause = new GamePaused(stateHandler);
        dikuArcadeBiding = new facade();

    }
    [Test]
    public void testHit(){
        stateHandler.ActiveState = gameRunning;
        Unbreakable testBlock = (Unbreakable)dikuArcadeBiding.placeBlocks((0.2f,0.2f),(0.03f,0.04f),"brown-block.png",typeof(Unbreakable),gameRunning);
        gameRunning.getBlocks.Clear();
        gameRunning.getBlocks.Add(testBlock);
        testBlock.hit(50,gameRunning.getBalls[0]);
        Assert.True(testBlock.CurrentHealth == 50);
    }
}