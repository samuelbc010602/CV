namespace Player;

using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;

using Facade;

using System;
///<summary>
///Class reponsible for the player
///</summary>
public class player : Entity, IGameEventProcessor{

    private float moveLeft = 0.0f;
    private float moveRight = 0.0f;
    private float MOVEMENT_SPEED = 0.01f;
    private facade dikuArcadeBiding = new facade();
    private DynamicShape shape;
    private Shape oldShape;

    //keeps track of messages
    private uint playerSpeedTracker;
    private uint wideTracker;

    private void widen(){
        base.Shape = new DynamicShape(Shape.AsDynamicShape().Position.X-0.03f,Shape.AsDynamicShape().Position.Y,0.21f,0.03f);
        shape = base.Shape.AsDynamicShape();
    }
    public player(Shape shape, IBaseImage image) : base(shape.AsDynamicShape(),image){
        oldShape = shape;
        this.shape = shape.AsDynamicShape();
    }
    public void move() {  
            shape.Move();
            if (shape.Position.X <= 0.0f) {
                shape.Position.X += 0.01f;
            }
            else if (shape.Position.X >= 1.0f - shape.Extent.X) {
                shape.Position.X -= 0.01f;
            }
            else {
                shape.Move();
            }
    }
    public void ProcessEvent(GameEvent gameEvent){
        switch (gameEvent.Message) {
            case ("A"):
                if(Shape.AsDynamicShape().Position.X > 0.0f){
                    SetMoveLeft(true);
                }
                break;
            case("D"):
                if(Shape.AsDynamicShape().Position.X <= 0.84f){
                    SetMoveRight(true);
                }
                break;
            case("A-stop"):
                SetMoveLeft(false);
                break;
            case("D-stop"):
                SetMoveRight(false);
                break;
            case("Player Speed-first"):
                MOVEMENT_SPEED = 0.03f;
                playerSpeedTracker = gameEvent.Id;
                break;
            case("Player Speed-second"):
                if(gameEvent.Id == playerSpeedTracker){
                    MOVEMENT_SPEED = 0.01f;
                }
                break;
            case("Wide-first"):
                wideTracker = gameEvent.Id;
                widen();
                break;
            case("Wide-second"):
                if(gameEvent.Id == wideTracker){
                    base.Shape = oldShape;
                    shape = base.Shape.AsDynamicShape();
                }
                break;
            default:
                break;
        }
    }
    private void UpdateDirection () {
        Vec2F direction = new Vec2F(moveLeft+moveRight,0.0f);
        shape.ChangeDirection(direction);
    }
    private void SetMoveLeft(bool val) {
        // TODO:set moveLeft appropriately and call UpdateMovement()
            if (val) {
                moveLeft = -MOVEMENT_SPEED;
            }else {
                moveLeft = 0.0f;
            }
            UpdateDirection();
        }
    private void SetMoveRight(bool val) {
    // TODO:set moveRight appropriately and call UpdateMovement()
        if (val) {
            moveRight = MOVEMENT_SPEED;
        }else {
            moveRight = 0.0f;
        }
        UpdateDirection();
    }
    public float getMovementspeed{
        get{return MOVEMENT_SPEED;}
        set{MOVEMENT_SPEED = value;}
    }
    public DynamicShape getShape{
        get{return shape;}
        set{shape = value;}
    }
    public float getMoveLeft{
        get{return moveLeft;}
    }
    public float getMoveRight{
        get{return moveRight;}
    }
}