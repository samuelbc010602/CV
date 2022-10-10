namespace BallTypes.normalBall;

using Entities.Ball;
using Entities.Block;
using Player;

using DIKUArcade;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Physics;
using DIKUArcade.Math;
using DIKUArcade.Events;

using System.Security.Principal;
using System.Collections.Generic;
using System.Linq;
using System;

using ASCIILoader;
using Facade;

using BallTypes.powerUpBall;

using States.GameRunning;

///<summary>
///Class reponsible for the NormalBall, which is the one primarily used within the game
///</summary>
public class NormalBall: ball{
    GameRunning caller;
    player playerEnt;
    List<block> blocksEnt;
    facade dikuArcadeBinding = new facade();
    private bool firstCollision;
    private bool inCollision;

    //Handles the movement and collision with other entities
    public override void ballMove(){
        
        Random rnd = new Random();
        inCollision = false;
        Entity collidingElement = null;
        (float,float) prevPos = default;

        (player,bool,float,float,CollisionDirection) playerData = dikuArcadeBinding.checkForCollision<player>(Shape.AsDynamicShape(),new List<player>{playerEnt});
        (block,bool,float,float,CollisionDirection) blockData = dikuArcadeBinding.checkForCollision<block>(Shape.AsDynamicShape(),blocksEnt);

        if(playerData.Item2){
            prevPos = (Shape.AsDynamicShape().Direction.Y,Shape.AsDynamicShape().Direction.Y);
            firstCollision = true;
            inCollision = true;
            collidingElement = playerData.Item1;
        }else if(blockData.Item2){
            prevPos = (Shape.AsDynamicShape().Direction.Y,Shape.AsDynamicShape().Direction.Y);
            if(base.getHardBall){
                blockData.Item1.hit(201,this);
            }else{
                blockData.Item1.hit(100,this);
            }
            inCollision = true;
            collidingElement = blockData.Item1;
        }

        //Border collision
        if(Shape.AsDynamicShape().Position.X <= 0.0f){
            if(Shape.AsDynamicShape().Direction.Y < 0.0f){
                Shape.AsDynamicShape().Direction = new Vec2F(0.005f,-0.015f);
            }else{
                Shape.AsDynamicShape().Direction = new Vec2F(0.005f,0.015f);
            }
        }
        else if(Shape.AsDynamicShape().Position.X >= 0.95f){
            if(Shape.AsDynamicShape().Direction.Y < 0.0f){
                Shape.AsDynamicShape().Direction = new Vec2F(-0.005f,-0.015f);
            }else{
                Shape.AsDynamicShape().Direction = new Vec2F(-0.005f,0.015f);
            }
        }
        else if(Shape.AsDynamicShape().Position.Y >= 0.95f){
            if(Shape.AsDynamicShape().Direction.X < 0.0f){
                Shape.AsDynamicShape().Direction = new Vec2F(-0.005f,-0.015f);
            }else{
                Shape.AsDynamicShape().Direction = new Vec2F(0.005f,-0.015f);
            }
        }
        else if(Shape.AsDynamicShape().Position.Y < 0.1f){
            caller.getHealth = caller.getHealth--;
            int count = 0;
            foreach(ball i in caller.getBalls){
                if(i.GetType() != typeof(PowerUpBall)){
                    count++;
                }
            }
            if(!base.GetTakeLife && count == 1){
                Shape.AsDynamicShape().Position.X = playerEnt.Shape.AsDynamicShape().Position.X+0.05f;
                Shape.AsDynamicShape().Position.Y = playerEnt.Shape.AsDynamicShape().Position.Y+0.05f;
            }else{
                base.GetOutOfGame = true;
            }
        }

        //Collision checking
        if(inCollision){
            float centerOfCollidingElement = collidingElement.Shape.AsDynamicShape().Position.X;
            if(Shape.AsDynamicShape().Direction.Y < 0.0f){
                if(Shape.AsDynamicShape().Position.X < centerOfCollidingElement+(collidingElement.Shape.Extent.X/3)/2){
                Shape.AsDynamicShape().Direction = new Vec2F(-0.005f,0.015f);
                }else if(Shape.AsDynamicShape().Position.X > centerOfCollidingElement+(collidingElement.Shape.Extent.X/3)/2 && Shape.AsDynamicShape().Position.X < centerOfCollidingElement+(collidingElement.Shape.Extent.X/3)){
                    Shape.AsDynamicShape().Direction = new Vec2F(0.0f,0.015f);
                }else{
                    Shape.AsDynamicShape().Direction = new Vec2F(0.005f,0.015f);
                }
            }else{
                if(Shape.AsDynamicShape().Position.X < centerOfCollidingElement+(collidingElement.Shape.Extent.X/3)/2){
                    Shape.AsDynamicShape().Direction = new Vec2F(-0.005f,-0.015f);
                }else if(Shape.AsDynamicShape().Position.X > centerOfCollidingElement+(collidingElement.Shape.Extent.X/3)/2 && Shape.AsDynamicShape().Position.X < centerOfCollidingElement+(collidingElement.Shape.Extent.X/3)){
                    Shape.AsDynamicShape().Direction = new Vec2F(0.0f,-0.015f);
                }else{
                    Shape.AsDynamicShape().Direction = new Vec2F(0.005f,-0.015f);
                }
            }
        }
        
        else if(firstCollision && !inCollision){
            Shape.AsDynamicShape().Direction = new DIKUArcade.Math.Vec2F(Shape.AsDynamicShape().Direction.X*base.getExtraSpeed,Shape.AsDynamicShape().Direction.Y*base.getExtraSpeed);
        }
        else{
            Shape.AsDynamicShape().Direction = new DIKUArcade.Math.Vec2F(0.0f,-0.015f);
        }

        if(prevPos != default){
            if(dikuArcadeBinding.checkForNewPosition<block>((Shape.AsDynamicShape().Position.X,Shape.AsDynamicShape().Position.Y,Shape.Extent.X,Shape.Extent.Y),blocksEnt,(Shape.AsDynamicShape().Direction.X,Shape.AsDynamicShape().Direction.Y))){
                Shape.AsDynamicShape().Direction = new DIKUArcade.Math.Vec2F(-1.0f*prevPos.Item1,-1.0f*prevPos.Item2);
            }
        }
        Shape.Move();
    }
    public NormalBall(Shape shape, IBaseImage image, GameRunning caller) : base(shape,image,caller){
        this.caller = caller;
        playerEnt = caller.getPlayer;
        blocksEnt = caller.getBlocks;
    }

    public void setSplit(){
        base.getIsSplitBall = false;
    }

    public bool setFirstCollision{
        get{return firstCollision;}
        set{firstCollision = value;}
    }
}