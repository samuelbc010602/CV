namespace BallTypes.powerUpBall;

using Entities.Ball;

using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Timers;
using DIKUArcade.Events;

using System.Security.Principal;
using System.Collections.Generic;
using System.IO;
using System;

using ASCIILoader;
using Facade;
using States.GameRunning;

using Entities.Block;

using Player;

using GetGameBus;


///<summary>
///Class reponsible for the PowerUpBall and powerups
///</summary>

public class PowerUpBall : ball{
    GameRunning caller;
    player playerEnt;
    List<ball> ballEnts = new List<ball>();
    facade dikuArcadeBinding = new facade();
    //Used to send messages to the event-bus
    public void sendMessage(string message,bool timed,int time = 15){
        if(timed){
            getGameBus.GetBus().RegisterTimedEvent(new DIKUArcade.Events.GameEvent{
                    EventType = DIKUArcade.Events.GameEventType.TimedEvent,
                    Message = message,
                    Id = caller.getMessageCount
                },TimePeriod.NewSeconds(time));
                caller.getId.Add(caller.getMessageCount);
        }else{
            getGameBus.GetBus().RegisterEvent(new GameEvent{
                    EventType = DIKUArcade.Events.GameEventType.InputEvent,
                    Message = message,
                    Id = caller.getMessageCount
                });
                caller.getId.Add(caller.getMessageCount);
        }
    }
    //Instantiates the different powerups
    public void powerUp(){
        if(caller.getCurrentPowerUpBalls.ContainsKey(this)){
            if(caller.getCurrentPowerUpBalls[this] == "Extra Life"){
                Console.WriteLine("extra life");
                caller.getHealth = caller.getHealth+1;
            }

            else if(caller.getCurrentPowerUpBalls[this] == "Player Speed"){
                Console.WriteLine("player speed");
                caller.getMessageCount++;
                sendMessage("Player Speed-first",false);
                sendMessage("Player Speed-second",true);
            }

            else if(caller.getCurrentPowerUpBalls[this] == "More Time"){
                Console.WriteLine("More Time");
                if(caller.getHasTime){
                    caller.getCountdown = caller.getCountdown+10.0f;
                }else{
                    caller.getCurrentPowerUpBalls[this] = "Hard Ball";
                }
            }
            else if(caller.getCurrentPowerUpBalls[this] == "Invincible"){
                Console.WriteLine("Invincible");
                caller.getMessageCount++;
                Console.WriteLine("Invincible");
                sendMessage("Invincible-first",false);
                sendMessage("Invincible-second",true);
            }
            else if(caller.getCurrentPowerUpBalls[this] == "Wide"){
                Console.WriteLine("wide");
                caller.getMessageCount++;
                sendMessage("Wide-first",false);
                sendMessage("Wide-second",true);
            }
            else if(caller.getCurrentPowerUpBalls[this] == "Split"){
                Console.WriteLine("wide");
                caller.getMessageCount++;
                sendMessage("Split-first",false);
            }
            else if(caller.getCurrentPowerUpBalls[this] == "Hard Ball"){
                Console.WriteLine("hard ball");
                caller.getMessageCount++;
                sendMessage("Hard Ball-first",false);
                sendMessage("Hard Ball-second",true);
            }
        }
        getGameBus.GetBus().ProcessEvents();
        caller.getCurrentPowerUpBalls.Remove(this);
    }
    
    //Handles the movement and collision with the player

    public override void ballMove(){
        Shape.AsDynamicShape().Direction = new DIKUArcade.Math.Vec2F(0.0f,-0.015f);
        Shape.Move();
        if(dikuArcadeBinding.checkForCollision(Shape.AsDynamicShape(),new List<player>{playerEnt}).Item2){
            powerUp();
            base.GetOutOfGame = true;
        }else if(Shape.AsDynamicShape().Position.Y < 0.0f){
            base.GetOutOfGame = true;
        }
    }
    public PowerUpBall(Shape shape, IBaseImage image, GameRunning caller) : base(shape,image,caller){
        this.caller = caller;
        base.GetTakeLife = false;
        playerEnt = caller.getPlayer;
        ballEnts = caller.getBalls;
    }
}