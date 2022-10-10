namespace BreakoutTests;

using NUnit.Framework;
using Statemachine;
using States.GameRunning;

using System;

using DIKUArcade.GUI;
using DIKUArcade.Entities;
using DIKUArcade.Events;
using DIKUArcade.Math;
using DIKUArcade.Input;

using ASCIILoader;
using Entities.Block;
using Player;
using Game;

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

public class BlockTest
{
    private stateMachine stateHandler;
    private game startPoint;
    private player playerEnt;
    private GameRunning gameRun;
    [SetUp]
    public void Setup()
    {
        var windowArgs = new WindowArgs() { Title = "Galaga v0.1" };
        game startPoint = new game(windowArgs); 
        stateHandler = new stateMachine();
        playerEnt = GameRunning.getOrSetInstance(stateHandler).getPlayer;
    }

    [Test]
    public void checkAllBlocks(){
        foreach(block i in GameRunning.getOrSetInstance(stateHandler).getBlocks){
            Assert.True(i is Entity == true);
        }
    }
    [Test]
    public void valueHealthProperty(){
        foreach(block i in GameRunning.getOrSetInstance(stateHandler).getBlocks){
            Assert.That(() => i.Val, Throws.Nothing);
            Assert.That(() => i.StartHealth, Throws.Nothing);
            //Assert.DoesNotThrow(() => {i.Val == 500;});
        }
    }
    [Test]
    public void checkHit(){
        foreach(block i in GameRunning.getOrSetInstance(stateHandler).getBlocks){
            i.hit(50);
            Assert.True(i.StartHealth != i.CurrentHealth);
        }
    }


}