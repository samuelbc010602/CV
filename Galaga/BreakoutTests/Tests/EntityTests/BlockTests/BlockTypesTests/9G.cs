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
using DIKUArcade.Timers;

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
using BlockTypes.unbreakable;
using BlockTypes.invinsible;
using BlockTypes.teleporting;
using BlockTypes.hungry;
using BlockTypes.healing;

using Facade;




public class BlockTypesTests
{
    private stateMachine stateHandler;
    private game startPoint;
    private player playerEnt;

    private MainMenu menu;
    private GameOver gameIsOver;
    private GameRunning gameRunning;
    private GamePaused pause;
    private facade dikuArcadeBinding;
    [SetUp]
    public void Setup()
    {
        var windowArgs = new WindowArgs() { Title = "Galaga v0.1" };
        game startPoint = new game(windowArgs); 
        stateHandler = new stateMachine();
        playerEnt = GameRunning.getOrSetInstance(stateHandler).getPlayer;

        dikuArcadeBinding = new facade();

        menu = new MainMenu(stateHandler);
        gameIsOver = new GameOver(stateHandler);
        gameRunning = new GameRunning(stateHandler);
        pause = new GamePaused(stateHandler);

    }

    [Test]

    public void testHardened(){
        stateHandler.ActiveState = gameRunning;
        gameRunning.makeLevel("level4.txt");
        foreach(block i in gameRunning.getBlocks){
            if(i is Hardened){
                Image prevImage = i.getSetImage;
                i.hit(100,gameRunning.getBalls[0]);
                Assert.True(i.CurrentHealth == i.StartHealth/2 && i.getSetImage != prevImage);
            }
        }
    }

    [Test]

    public void testUnbreakable(){
        stateHandler.ActiveState = gameRunning;
        gameRunning.makeLevel("level4.txt");
        Unbreakable toReplace;
        foreach(block i in gameRunning.getBlocks){
            if(i is Unbreakable){
                i.hit(100,gameRunning.getBalls[0]);
                Assert.True(i.CurrentHealth == i.StartHealth);
            }
        }
    }

    [Test]
    public void testInvisible(){
        stateHandler.ActiveState = gameRunning;
        gameRunning.makeLevel("level4.txt");
        foreach(block i in gameRunning.getBlocks){
            if(i is Invinsible){
                Image prevImage = i.getSetImage;
                i.hit(50,gameRunning.getBalls[0]);
                Assert.True(i.getSetImage != prevImage);
            }
        }
    }

    //[Test]
    public void testTeleporting(){
        stateHandler.ActiveState = gameRunning;
        gameRunning.makeLevel("level4.txt");
        foreach(block i in gameRunning.getBlocks){
            if(i is Teleporting){
                float prevPos = i.Shape.AsDynamicShape().Position.X;
                Console.WriteLine(prevPos);
                i.hit(100,gameRunning.getBalls[0]);
                Console.WriteLine(i.Shape.AsDynamicShape().Position.X);
                Assert.True(i.Shape.AsDynamicShape().Position.X != prevPos);
            }
        }
    }

    [Test]
    public void testHealing(){
        stateHandler.ActiveState = gameRunning;
        gameRunning.makeLevel("level4.txt");
        foreach(block i in gameRunning.getBlocks){
            if(i is Healing){
                i.hit(75,gameRunning.getBalls[0]);
            }
        }
        StaticTimer.RestartTimer();
        if(StaticTimer.GetElapsedSeconds() > 15.0){
            foreach(block i in gameRunning.getBlocks){
                if(i is Healing){
                    Assert.True(i.CurrentHealth > 30);
                }
            }
        }
    }
    
    

}