namespace BreakoutTests;

using NUnit.Framework;
using Statemachine;
using States.GameRunning;

using System;

using DIKUArcade.GUI;
using DIKUArcade.Entities;
using DIKUArcade.Events;
using DIKUArcade.Math;

using ASCIILoader;
using Entities.Block;
using Player;
using Game;

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

using States.GameOver;
using States.GamePaused;
using States.GameRunning;
using States.MainMenu;

public class PlayerTest
{
    private stateMachine stateHandler;
    private game startPoint;
    private player playerEnt;
    [SetUp]
    public void Setup()
    {
        var windowArgs = new WindowArgs() { Title = "Galaga v0.1" };
        game startPoint = new game(windowArgs); 
        stateHandler = new stateMachine();
        playerEnt = GameRunning.getOrSetInstance(stateHandler).getPlayer;
        /*
        menu = new MainMenu(stateHandler);
        gameIsOver = new GameOver(stateHandler);
        gameRunning = new GameRunning(stateHandler);
        pause = new GamePaused(stateHandler);
        */

    }
    [Test]
    public void checkForPlayerPosition(){
        Assert.True(playerEnt.Shape.AsDynamicShape().Position.X == 0.5f);
    }
    [TestCase("A")]
    [TestCase("B")]
    public void checkMovement(string messageSend){
        GameEvent eventCaller = new GameEvent{
            Message = messageSend
        };
        playerEnt.ProcessEvent(eventCaller);
        playerEnt.move();

        Assert.True(playerEnt.getShape.Direction != new Vec2F() && playerEnt.getShape.Position.X != 0.5f);
    }

    [Test]
    public void playerIsEntity(){
        Assert.True(playerEnt is Entity == true);
    }
    [Test]
    public void existInBottomHalf(){
        Assert.True(playerEnt.getShape.Position.Y < 0.5f);
    }
    /*
    [Test]
    public void cantLeaveLeft(){
        stateHandler.ActiveState = gameRunning;
        bool stop = true;
        
        while(stop){
            gameRunning.getPlayer.ProcessEvent(new GameEvent{
            Message = "A"
            });
            gameRunning.getPlayer.Move();
            if(gameRunning.getPlayer.Shape.AsDynamicShape().Position.X == 0.0f){
                gameRunning.getPlayer.ProcessEvent(new GameEvent{
                Message = "A"
                });
                gameRunning.getPlayer.Move();
                Assert.True(gameRunning.getPlayer.Shape.AsDynamicShape().Position.X == 0.0f);
            }
        }
    }
    */
    /*
    [Test]
    public void cantLeaveRight(){
        stateHandler.ActiveState = gameRunning;
        bool stop = true;
        
        while(stop){
            gameRunning.getPlayer.ProcessEvent(new GameEvent{
            Message = "D"
            });
            gameRunning.getPlayer.Move();
            if(gameRunning.getPlayer.Shape.AsDynamicShape().Position.X == 0.0f){
                gameRunning.getPlayer.ProcessEvent(new GameEvent{
                Message = "D"
                });
                gameRunning.getPlayer.Move();
                Assert.True(gameRunning.getPlayer.Shape.AsDynamicShape().Position.X == 1.0f);
            }
        }
    }
    */

}