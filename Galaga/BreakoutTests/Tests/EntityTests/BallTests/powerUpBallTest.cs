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
public class powerUpTest
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
    public void testSendTimedMessage(){
        stateHandler.ActiveState = gameRunning;
        gameRunning.placePowerUp(0.2f,0.3f);
        Assert.DoesNotThrow(() => {gameRunning.getCurrentPowerUpBalls.Keys.First().sendMessage("test",true);});
        
    }

    [Test]
    public void testSendMessage(){
        stateHandler.ActiveState = gameRunning;
        gameRunning.placePowerUp(0.2f,0.3f);
        Assert.DoesNotThrow(() => {gameRunning.getCurrentPowerUpBalls.Keys.First().sendMessage("Player Speed-first",false);});
        getGameBus.GetBus().ProcessEvents();
        Assert.True(playerEnt.getMovementspeed == 0.03f);
    }

    [Test]
    public void testExtraLife(){
        stateHandler.ActiveState = gameRunning;
        gameRunning.getPowerUpCat = new Dictionary<string,string>{{"Extra Life","LifePickUp.png"}};
        gameRunning.placePowerUp(0.2f,0.3f);
        gameRunning.getCurrentPowerUpBalls.Keys.First().powerUp();
       
        Assert.True(gameRunning.getHealth != 1);
    }

    [Test]
    public void testPlayerSpeed(){
        stateHandler.ActiveState = gameRunning;
        gameRunning.getPowerUpCat = new Dictionary<string,string>{{"Player Speed","LifePickUp.png"}};
        gameRunning.placePowerUp(0.2f,0.3f);
        gameRunning.getCurrentPowerUpBalls.Keys.First().powerUp();
        
        Assert.True(playerEnt.getMovementspeed == 0.03f);

    }

    [Test]
    public void testTime(){
        stateHandler.ActiveState = gameRunning;
        gameRunning.getPowerUpCat = new Dictionary<string,string>{{"More Time","LifePickUp.png"}};
        gameRunning.placePowerUp(0.2f,0.3f);
        gameRunning.getCurrentPowerUpBalls.Keys.First().powerUp();
        
        Assert.True(gameRunning.getCountdown != 300);

    }

    [Test]
    public void testInvincible(){
        stateHandler.ActiveState = gameRunning;
        gameRunning.getPowerUpCat = new Dictionary<string,string>{{"Invincible","LifePickUp.png"}};
        gameRunning.placePowerUp(0.2f,0.3f);
        gameRunning.getCurrentPowerUpBalls.Keys.First().powerUp();
        
        Assert.True(gameRunning.getBalls[0].GetTakeLife == false);

    }

    [Test]
    public void testWide(){
        stateHandler.ActiveState = gameRunning;
        gameRunning.getPowerUpCat = new Dictionary<string,string>{{"Wide","LifePickUp.png"}};
        gameRunning.placePowerUp(0.2f,0.3f);
        gameRunning.getCurrentPowerUpBalls.Keys.First().powerUp();
        
        Assert.True(gameRunning.getPlayer.getShape.Extent.X > 0.15f);

    }

    [Test]
    public void testSplit(){
        stateHandler.ActiveState = gameRunning;
        gameRunning.getPowerUpCat = new Dictionary<string,string>{{"Split","LifePickUp.png"}};
        gameRunning.placePowerUp(0.2f,0.3f);
        gameRunning.getCurrentPowerUpBalls.Keys.First().powerUp();
        
        Assert.True(gameRunning.getBalls.Count != 1);

    }

    [Test]
    public void testHardball(){
        stateHandler.ActiveState = gameRunning;
        gameRunning.getPowerUpCat = new Dictionary<string,string>{{"Hard Ball","LifePickUp.png"}};
        gameRunning.placePowerUp(0.2f,0.3f);
        gameRunning.getCurrentPowerUpBalls.Keys.First().powerUp();
        
        Assert.True(gameRunning.getBalls[0].getHardBall == true);

    }

}
