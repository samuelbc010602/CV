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

using Entities.Ball;

public class abstractBallTest
{
    private stateMachine stateHandler;
    private game startPoint;
    private player playerEnt;

    private MainMenu menu;
    private GameOver gameIsOver;
    private GameRunning gameRunning;
    private GamePaused pause;
    public abstractBallTest(){
        Setup();
    }
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
    public void testProcessEvent(){
        stateHandler.ActiveState = gameRunning;
        foreach(ball i in gameRunning.getBalls){
            i.ProcessEvent(new GameEvent{
                Message = "Invincible-second",
                Id = 1
            });
            i.ProcessEvent(new GameEvent{
                Message = "Invincible-second",
                Id = 1
            });
            Assert.True(i.GetTakeLife == true);
        }

        foreach(ball i in gameRunning.getBalls){
            i.ProcessEvent(new GameEvent{
                Message = "Hard Ball-first",
                Id = 1
            });
            Assert.True(i.getHardBall == true);
            i.ProcessEvent(new GameEvent{
                Message = "Hard Ball-second",
                Id = 1
            });
            Assert.True(i.getHardBall == false);
        }

    }
}

