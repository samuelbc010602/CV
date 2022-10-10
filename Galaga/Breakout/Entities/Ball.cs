namespace Entities.Ball;

using DIKUArcade;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Physics;
using DIKUArcade.Math;
using DIKUArcade.Events;

using System.Security.Principal;
using System.Collections.Generic;
using System;

using ASCIILoader;
using Facade;
using Player;
using Block;
using States.GameRunning;
using Statemachine;
using GetGameBus;

using BallTypes.normalBall;

///<summary>
///Class reponsible for all balls 
///</summary>
public abstract class ball : Entity, IGameEventProcessor{
    private GameRunning caller;
    private bool outOfGame = false;
    private bool takeLife = true;
    private bool isSplitBall = true;

    private bool hardball = false;
    private float extraSpeed = 1.0f;
    private facade dikuArcadeBinding;
    public abstract void ballMove();

    //keep track of messages
    private uint hardballTracker;
    private uint InvincibleTracker;
    private int maxLength = 3;
    public void split(){
        List<(float,float)> newPos = new List<(float, float)>{(base.Shape.Position.X,base.Shape.Position.Y),(base.Shape.Position.X,base.Shape.Position.Y)};
        List<(float,float)> newShapes = new List<(float, float)>{(0.05f,0.05f)};
        List<Vec2F> newDir = new List<Vec2F>{new Vec2F(-0.005f,0.025f),new Vec2F(0.002f,0.015f), new Vec2F(0.005f,0.115f)};
        List<ball> newBalls = dikuArcadeBinding.placeBall(newPos,newShapes,new List<string>{"ball.png"},new List<Type>{(typeof(NormalBall))},caller);
        for(int i = 0;i<newBalls.Count;i++){
            newBalls[i].Shape.AsDynamicShape().Direction = newDir[i];
            ((NormalBall)newBalls[i]).setFirstCollision = true;
            //getGameBus.GetBus().Subscribe(GameEventType.InputEvent,newBalls[i]);
            //getGameBus.GetBus().Subscribe(GameEventType.TimedEvent,newBalls[i]);
            caller.getBalls.Add(newBalls[i]);            
        }
    }
    //Recieves messages that are used to spawn powerups
    public void ProcessEvent(GameEvent gameEvent){
        switch(gameEvent.Message){
            case("Invincible-first"):
                InvincibleTracker = gameEvent.Id;
                takeLife = false;
                break;
            case("Invincible-second"):
                if(gameEvent.Id == InvincibleTracker){
                    takeLife = true;
                }
                break;
            case("Split-first"):
                if(caller.getBalls.Count <= 5){
                    split();
                }
                /*foreach(ball i in caller.getBalls){
                    getGameBus.GetBus().Subscribe(GameEventType.InputEvent,i);
                    getGameBus.GetBus().Subscribe(GameEventType.TimedEvent,i);
                }*/
                break;
            case("Hard Ball-first"): 
                hardballTracker = gameEvent.Id;
                hardball = true;
                break;
            case("Hard Ball-second"):
                if(gameEvent.Id == hardballTracker){
                    hardball = false;
                }
                break;
            default:
                break;
        }
    }
    public bool GetOutOfGame{
        get{return outOfGame;}
        protected set{outOfGame = value;}
    }
    public bool GetTakeLife{
        get{return takeLife;}
        set{takeLife = value;}
    }
    
    public float getExtraSpeed{
        get{return extraSpeed;}
    }
    public bool getHardBall{
        get{return hardball;}
    }
    public bool getIsSplitBall{
        get{return isSplitBall;}
        set{isSplitBall = value;}
    }
    public ball(Shape shape, IBaseImage image, GameRunning caller):base(shape.AsDynamicShape(),image){
        dikuArcadeBinding = new facade();
        this.caller = caller;
    }
}