namespace BlockTypes.hungry;

using Entities.Block;

using DIKUArcade;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Events;
using System.Security.Principal;
using System.Collections.Generic;
using System;

using ASCIILoader;
using Facade;
using Entities.Ball;
using BallTypes.normalBall;
using States.GameRunning;

///<summary>
///Class reponsible for the Hungry block, 
///</summary>

public class Hungry : block{
    string imageName;
    GameRunning caller;
    List<ball> ballEnts = new List<ball>();
    facade dikuArcadeBinding = new facade();
    public override void hit(uint damage, ball ballCol){
        if(ballCol == null){
            throw new Exception("Must get a ball to delete it");
        }
        foreach(ball i in ballEnts){
            if(i == ballCol){
                caller.getBalls.Remove(i);
                caller.getBalls.Add(dikuArcadeBinding.placeBall((caller.getPlayer.Shape.AsDynamicShape().Position.X,caller.getPlayer.Shape.AsDynamicShape().Position.Y+0.25f),(0.05f,0.05f),"ball.png",typeof(NormalBall),caller));
                break;
            }
        }
    }

    public Hungry(Shape shape, IBaseImage image, string imageName, GameRunning caller) : base(shape,image){
        this.caller = caller;
        base.CurrentHealth = 100;
        base.StartHealth = base.CurrentHealth;
        ballEnts = caller.getBalls;
        this.imageName = imageName;
    }
}
