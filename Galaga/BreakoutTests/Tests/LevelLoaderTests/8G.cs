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
using ASCIILoader;

using States.GameOver;
using States.GamePaused;
using States.GameRunning;
using States.MainMenu;

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

public class levelLoaderTest
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

    public void invalidLineLength(){
        asciiLoader reader = new asciiLoader();
        var ex = Assert.Throws<Exception>(() => reader.loadASCII("invalidLength.txt"));
        Assert.That(ex.Message, Is.EqualTo("Invalid length in ASCII map"));
    }

    [Test]
    public void invalidSize(){
        asciiLoader reader = new asciiLoader();
        var ex = Assert.Throws<Exception>(() => reader.loadASCII("invalidSize.txt"));
        Assert.That(ex.Message, Is.EqualTo("Invalid size of ASCII map"));
    }

    [Test]

    public void invalidMetaData(){
        asciiLoader reader = new asciiLoader();
        var ex = Assert.Throws<Exception>(() => reader.loadASCII("invalidMeta.txt"));
        Assert.That(ex.Message, Is.EqualTo("Invalid meta data"));
    }
}