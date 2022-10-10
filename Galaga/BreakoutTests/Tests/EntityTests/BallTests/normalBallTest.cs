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
using Facade;

using States.GameOver;
using States.GamePaused;
using States.GameRunning;
using States.MainMenu;

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

using BallTypes.normalBall;
using Entities.Ball;

public class normalBallTest
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
        playerEnt = GameRunning.getOrSetInstance(stateHandler).getPlayer;

        menu = new MainMenu(stateHandler);
        gameIsOver = new GameOver(stateHandler);
        gameRunning = new GameRunning(stateHandler);
        pause = new GamePaused(stateHandler);
        dikuArcadeBiding = new facade();

    }

    [Test]
    public void blockColLeft(){
        stateHandler.ActiveState = gameRunning;
        bool keepRendering = true;
        while(keepRendering){
            stateHandler.ActiveState.RenderState();
            stateHandler.ActiveState.UpdateState();
            foreach(ball i in gameRunning.getBalls){
                i.ballMove();
            }
            if(((NormalBall)gameRunning.getBalls[0]).setFirstCollision){
                gameRunning.getBalls[0].Shape.Position = gameRunning.getBlocks[0].Shape.Position;
                gameRunning.getBalls[0].ballMove();
                Console.WriteLine(gameRunning.getBalls[0].Shape.AsDynamicShape().Direction);
                Assert.True(gameRunning.getBalls[0].Shape.AsDynamicShape().Direction.X == -0.005f);
                keepRendering = false;
            }

        }
    }
    [Test]
    public void blockColMiddle(){
        stateHandler.ActiveState = gameRunning;
        bool keepRendering = true;
        while(keepRendering){
            stateHandler.ActiveState.RenderState();
            stateHandler.ActiveState.UpdateState();
            foreach(ball i in gameRunning.getBalls){
                i.ballMove();
            }
            if(((NormalBall)gameRunning.getBalls[0]).setFirstCollision){
                gameRunning.getBalls[0].Shape.Position = gameRunning.getBlocks[0].Shape.Position;


                gameRunning.getBalls[0].Shape.Position.X = gameRunning.getBalls[0].Shape.Position.X+(gameRunning.getBlocks[0].Shape.Extent.X/2)/2;
                
                
                gameRunning.getBalls[0].ballMove();
                Console.WriteLine(gameRunning.getBalls[0].Shape.AsDynamicShape().Direction);
                Assert.True(gameRunning.getBalls[0].Shape.AsDynamicShape().Direction.X == 0.0f);
                keepRendering = false;
            }

        }
    }

    [Test]
    public void blockColRight(){
        stateHandler.ActiveState = gameRunning;
        bool keepRendering = true;
        while(keepRendering){
            stateHandler.ActiveState.RenderState();
            stateHandler.ActiveState.UpdateState();
            foreach(ball i in gameRunning.getBalls){
                i.ballMove();
            }
            if(((NormalBall)gameRunning.getBalls[0]).setFirstCollision){
                gameRunning.getBalls[0].Shape.Position = gameRunning.getBlocks[0].Shape.Position;


                gameRunning.getBalls[0].Shape.Position.X = gameRunning.getBalls[0].Shape.Position.X+(gameRunning.getBlocks[0].Shape.Extent.X/2);
                
                
                gameRunning.getBalls[0].ballMove();
                Console.WriteLine(gameRunning.getBalls[0].Shape.AsDynamicShape().Direction);
                Assert.True(gameRunning.getBalls[0].Shape.AsDynamicShape().Direction.X == 0.005f);
                keepRendering = false;
            }

        }
    }


    [TestCase(-0.1f,0.2f,0.005f,0.015f)]
    [TestCase(0.95f,0.2f,-0.005f,0.015f)]
    public void checkBorderCollision(float x, float y, float testx, float testy){
        stateHandler.ActiveState = GameRunning.getOrSetInstance(stateHandler);
        bool keepRendering = true;
        while(keepRendering){
            stateHandler.ActiveState.RenderState();
            stateHandler.ActiveState.UpdateState();
            foreach(ball i in gameRunning.getBalls){
                i.ballMove();
            }
            if(((NormalBall)gameRunning.getBalls[0]).setFirstCollision){
                gameRunning.getBalls[0].Shape.Position.X = x;
                gameRunning.getBalls[0].Shape.Position.Y = y;


                gameRunning.getBalls[0].ballMove();
                Console.WriteLine(gameRunning.getBalls[0].Shape.AsDynamicShape().Direction);
                Assert.True(gameRunning.getBalls[0].Shape.AsDynamicShape().Direction.X == testx && gameRunning.getBalls[0].Shape.AsDynamicShape().Direction.Y == testy);
                keepRendering = false;
            }

        }
    }

}

