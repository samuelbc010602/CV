namespace States.GameRunning;

using DIKUArcade;
using DIKUArcade.Input;
using System.Security.Principal;
using System.Collections.Generic;
using DIKUArcade.Events;
using DIKUArcade.State;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Timers;
using ASCIILoader;

using System;
using System.IO;
using System.Linq;

using Facade;
using Player;
using GetGameBus;
using Statemachine;

using States.MainMenu;
using States.GamePaused;
using States.GameOver;

using Entities.Ball;
using Entities.Block;


using BlockTypes.hardened;
using BlockTypes.unbreakable;
using BlockTypes.normalBlock;

using BallTypes.normalBall;
using BallTypes.powerUpBall;


///<summary>
///Class reponsible for the state GameRunning
///</summary>
public class GameRunning : IGameState{
    private static GameRunning instance = null;

    private asciiLoader asciiReader;
    private facade dikuArcadeBinding;
    private stateMachine stateHandler;

    private Type powerUpBlock;

    //Entities
    private List<ball> balls;
    private List<block> blocks;
    private player playerEntity;

    private List<Entity> lives;
    private List<Entity> emptyLives;

    //Clock
    private StaticTimer timeClock;
    private double startTime;
    private double countdown;
    private bool toAddCountdown;
    private int powerUpTime;
    private bool hasTime = false;

    //Level
    private int currentLevel = 1;
    private uint pointsAwarded;
    
    //Health
    private int health = 3;
    private int maxLives = 5;

    //PowerUpBalls
    private Dictionary<PowerUpBall,string> currentPowerUpBalls;
    private Dictionary<string,string> powerUpCategories;

    //events
    private uint messageCount;
    private List<uint> messages = new List<uint>();

    public static GameRunning getOrSetInstance(stateMachine stateHandler,bool createNew = false){
        if(createNew){
            instance = null;
        }
        if(instance == null){
            instance = new GameRunning(stateHandler);
        }
        return GameRunning.instance;
    }

    public void ProcessEvent(){}

    public void InitializeGameState(){
        playerEntity = dikuArcadeBinding.placePlayer((0.5f,0.1f), (0.15f,0.03f), "player.png");
        
        List<(float,float)> livesPos = new List<(float, float)>{(0.05f,0.1f),(0.1f,0.1f),(0.15f,0.1f),(0.2f,0.1f),(0.25f,0.1f)};
        List<(float,float)> livesShape = new List<(float,float)>{(0.05f,0.05f)};
        foreach(Entity i in dikuArcadeBinding.placeStationaryEntities(livesPos,livesShape, new List<string>{"heart_filled.png"})){
            lives.Add(i);
        }
        try{
            getGameBus.GetBus().InitializeEventBus(new List<GameEventType>{GameEventType.InputEvent,GameEventType.TimedEvent});
        }
        catch{}

        makeLevel($"level{currentLevel}.txt");
    }
    
    public GameRunning(stateMachine stateHandler){
        asciiReader = new asciiLoader();
        this.stateHandler = stateHandler;
        currentPowerUpBalls = new Dictionary<PowerUpBall,string>();
        dikuArcadeBinding = new facade();
        asciiReader = new asciiLoader();
        powerUpCategories = new Dictionary<string,string>{{"Extra Life","LifePickUp.png"},
                                                            {"Player Speed","SpeedPickUp.png"},
                                                            {"More Time","DamagePickUp.png"},
                                                            {"Invincible","DamagePickUp.png"},
                                                            {"Wide","WidePowerUp.png"},
                                                            {"Split","SplitPowerUp.png"},
                                                            {"Hard Ball","DamagePickUp.png"} };
                                              
                                    
        blocks = new List<block>();
        balls = new List<ball>();

        lives = new List<Entity>();
        emptyLives = new List<Entity>();

        pointsAwarded = 0;
        InitializeGameState();
        }
    //Loads the game-level
    public void makeLevel(string level){
        //precondition
        string firstFive = "";
        if(level.Length != 10){
            throw new Exception("Invalid level name");
        }
        if(level.Substring(0,4) == "level"){
            throw new Exception("Invalid level name");
        }
        if(!Char.IsDigit(level[5])){
            throw new Exception("Invalid level name");
        }else{
            if(int.Parse(level[5].ToString()) >= 5){
                throw new Exception("Invalid level name");
            }
        }
        
        


        blocks = new List<block>();
        balls = new List<ball>();

        asciiReader.loadASCII(level);

        if(asciiReader.getMetaData.ContainsKey("Time")){
            hasTime = true;
            countdown = Convert.ToDouble(asciiReader.getMetaData["Time"]);
            startTime = Convert.ToDouble(asciiReader.getMetaData["Time"]);
        }

        List<(float,float)> blockPositions = new List<(float,float)>();
        List<(float,float)> blockShapes = new List<(float,float)>();
        List<string> blockImages = new List<string>();
        List<Type> blockTypes = new List<Type>();

        float x = 0.1f;
        float y = 0.2f;

        foreach(string outer in asciiReader.getAsciiMap){
            foreach(char inner in outer){
                if(inner != '-'){
                    blockPositions.Add((x,y));
                    blockShapes.Add((0.07f,0.03f));
                    blockImages.Add(asciiReader.getPictureCategory[inner.ToString()]);
                    x = x+0.07f;
                    
                    bool added = false;
                    foreach(KeyValuePair<string,Type> i in asciiReader.getBlockTypes){
                        if(i.Key == inner.ToString()){
                            if(asciiReader.getMetaData.ContainsKey("PowerUp")){
                                if(inner.ToString() == asciiReader.getMetaData["PowerUp"]){
                                    powerUpBlock = i.Value;
                                }
                            }
                            blockTypes.Add(i.Value);
                            added = true;
                            break;
                        }
                    }
                    if(!added){
                        if(asciiReader.getMetaData.ContainsKey("PowerUp")){
                            if(inner.ToString() == asciiReader.getMetaData["PowerUp"]){
                                powerUpBlock = typeof(NormalBlock);
                            }
                        }
                        blockTypes.Add(typeof(NormalBlock));
                    }
                }else{
                    x=x+0.07f;
                }
            }
            y = y+0.03f;
            x = 0.1f;
        }
        foreach(block i in dikuArcadeBinding.placeBlocks(blockPositions,blockShapes,blockImages,blockTypes,this)){
            blocks.Add(i);
        }
        balls.Add(dikuArcadeBinding.placeBall((0.5f,0.5f),(0.05f,0.05f),("ball.png"),typeof(NormalBall),this));
        getGameBus.GetBus().Subscribe(GameEventType.InputEvent,balls.Last());
        getGameBus.GetBus().Subscribe(GameEventType.TimedEvent,balls.Last());
    }

    public void ResetState(){
    }

    public void UpdateState(){
        if(health <= 0){
            instance = null;
            stateHandler.ActiveState = GameOver.getOrSetInstance(stateHandler,pointsAwarded);
        }if(hasTime){
            if(countdown <= 0.0){
                instance = null;
                stateHandler.ActiveState = GameOver.getOrSetInstance(stateHandler,pointsAwarded);
            }
        }
        
    }
    public void placePowerUp(float x, float y){
        if(x < 0.0f || x > 0.95 || y < 0.1f || y > 0.95){
            throw new Exception("Invalid position for powerupball");
        }
        Random rnd = new Random();
        int randNumber = rnd.Next(0,powerUpCategories.Count);
        string powerUpPicture = powerUpCategories.ElementAt(randNumber).Value;
        string powerUpCategory = powerUpCategories.ElementAt(randNumber).Key;

        balls.Add(dikuArcadeBinding.placeBall((x,y),(0.08f,0.04f),powerUpPicture,typeof(PowerUpBall), this));
        currentPowerUpBalls.Add((PowerUpBall)balls.Last(),powerUpCategory);
    }

    public void gameLogic(){
        if(blocks.Count() == 0 && health > 0){
            currentLevel++;
            if(currentLevel == 5){
                stateHandler.ActiveState = GameOver.getOrSetInstance(stateHandler,pointsAwarded);
            }else{
                makeLevel($"level{currentLevel}.txt");
            }
        }else if(balls.Count() == 0  && health > 0){
            health--;
            balls.Add(dikuArcadeBinding.placeBall(new List<(float, float)>{(playerEntity.Shape.Position.X+0.05f,playerEntity.Shape.Position.Y+0.05f)},new List<(float, float)>{(0.05f,0.05f)},new List<string>{("ball.png")}, new List<Type>{typeof(NormalBall)},this)[0]);
        }
    }
    //Renders and deletes the different entites of the game
    public void RenderState(){
        UpdateState();
        gameLogic();
        if(hasTime){
            countdown = startTime-StaticTimer.GetElapsedSeconds();
            toAddCountdown = false;
            foreach(Text i in dikuArcadeBinding.loadText(new List<string>{"Timer: "+(Convert.ToInt32(countdown)).ToString()},new List<(float,float)>{(0.08f,0.01f)},new List<(float, float)>{(0.3f,0.3f)},new List<string>{"Comic Sans MS"})){
                i.RenderText();
            }
        }
        for(int i = 0;i<health;i++){
            lives[i].RenderEntity();
        }

        List<int> blockToRemove = new List<int>();
        for(int i = 0; i<blocks.Count;i++){
            if(blocks[i].CurrentHealth > 0){
                blocks[i].RenderEntity();
            }else{
                pointsAwarded = pointsAwarded+(uint)blocks[i].Val;
                blocks[i].DeleteEntity();
                if(blocks[i].GetType() == powerUpBlock){
                    placePowerUp(blocks[i].Shape.AsDynamicShape().Position.X,blocks[i].Shape.AsDynamicShape().Position.Y);
                }
                blockToRemove.Add(i); 
            }
        }
        foreach(int i in blockToRemove.OrderByDescending(v => v)){
            blocks.RemoveAt(i);
        }
        blockToRemove.Clear();
        
        for(int i = balls.Count-1; i>=0;i--){
            if(balls[i].GetOutOfGame){
                balls[i].DeleteEntity();
                balls.RemoveAt(i);
            }else{
                balls[i].RenderEntity();
                balls[i].ballMove();
            }
        }
        foreach(ball i in balls){
            getGameBus.GetBus().Subscribe(GameEventType.InputEvent,i);
            getGameBus.GetBus().Subscribe(GameEventType.TimedEvent,i);
        }
        dikuArcadeBinding.loadText(new List<string>{"Points: " + pointsAwarded.ToString()},new List<(float, float)>{(0.09f,0.05f)}, new List<(float, float)>{(0.3f,0.3f)},new List<string>{"Comic Sans MS"})[0].RenderText();
        playerEntity.move();
        playerEntity.RenderEntity();
        
    }
    public void HandleKeyEvent(KeyboardAction action,KeyboardKey key){
        if(action == KeyboardAction.KeyPress){
        switch (key){
                case (KeyboardKey.A):
                    getGameBus.GetBus().RegisterEvent(new GameEvent {
                    EventType = GameEventType.InputEvent, 
                    From = this,
                    To = playerEntity,
                    Message = "A"
                    });

                    break;
                case (KeyboardKey.D):
                    getGameBus.GetBus().RegisterEvent (new GameEvent {
                    EventType = GameEventType.InputEvent, 
                    From = this,
                    To = playerEntity,
                    Message = "D"
                    });
                    break;
                case (KeyboardKey.Escape):
                    if(hasTime){
                        StaticTimer.PauseTimer();
                    }
                    stateHandler.ActiveState = GamePaused.getOrSetInstance(stateHandler);
                    break;
              default:
                break;
        }
        }else{
            switch(key){
                case (KeyboardKey.A):
                    getGameBus.GetBus().RegisterEvent(new GameEvent {
                    EventType = GameEventType.InputEvent, 
                    From = this,
                    To = playerEntity,
                    Message = "A-stop"
                    });

                    break;
                case (KeyboardKey.D):
                    getGameBus.GetBus().RegisterEvent (new GameEvent {
                    EventType = GameEventType.InputEvent, 
                    From = this,
                    To = playerEntity,
                    Message = "D-stop"
                    });
                    break;
              default:
                break;
            }
        }
        getGameBus.GetBus().ProcessEventsSequentially();
    }

    public Dictionary<PowerUpBall,string> getCurrentPowerUpBalls{
        get{return currentPowerUpBalls;}
        set{currentPowerUpBalls = value;}
    }
    public int getHealth{
        get{return health;}
        set{if(value <= maxLives){
            health = value;
        }}
    }
    public List<block> getBlocks{
        get{return blocks;}
        set{getBlocks = value;}
    }
    public List<ball> getBalls{
        get{return balls;}
        set{balls = value;}
    }
    public player getPlayer{
        get{return playerEntity;}
    }
    public double getCountdown{
        get{return startTime;}
        set{startTime = value;}
    }
    public bool getHasTime{
        get{return hasTime;}
    }
    public int getPowerUpTime{
        set{powerUpTime = value;}
        get{return powerUpTime;}
    }
    public List<uint> getId{
        set{messages = value;}
        get{return messages;}
    }
    public uint getMessageCount{
        get{return messageCount;}
        set{messageCount = value;}
    }
    public int getCurrentLevel{
        get{return currentLevel;}
    } 
    public uint getPoints{
        get{return pointsAwarded;}
    }
    public Dictionary<string,string> getPowerUpCat{
        get{return powerUpCategories;}
        set{powerUpCategories = value;}
    }
}