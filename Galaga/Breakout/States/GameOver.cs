namespace States.GameOver;
using States.GameRunning;
using States.MainMenu;

using DIKUArcade;
using DIKUArcade.Input;
using System.Security.Principal;
using System.Collections.Generic;
using DIKUArcade.Events;
using DIKUArcade.State;
using ASCIILoader;

using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.Timers;

using System;
using System.IO;
using System.Linq;

using Facade;
using Player;
using Entities.Block;
using GetGameBus;
using Statemachine;

///<summary>
///Class reponsible for the state GameOver
///</summary>

public class GameOver : IGameState{
    private static GameOver instance = null;

    private Entity backGroundImage;
    private List<Text> menuButtons;
    private List<Text> toRender;
    private int activeMenuButton = 0;
    private stateMachine stateHandler;
    private uint score = 0;
    private facade dikuArcadeBinding = new facade();

    public static GameOver getOrSetInstance(stateMachine stateMachineToPass,uint score = 0) {
        if (GameOver.instance == null) {
            GameOver.instance = new GameOver(stateMachineToPass,score);
        }
        return GameOver.instance;
    }

    public GameOver(stateMachine stateHandler,uint score = 0){
        this.stateHandler = stateHandler;
        this.score = score;
        menuButtons = new List<Text>();
        toRender = new List<Text>();
        InitializeGameState();
    }

    private void InitializeGameState(){
        List<string> textHolder = new List<string>{"Game over",$"Score: {score}","Start new game","Main Menu"};
        List<(float,float)> positionHolder = new List<(float,float)>{(0.35f,0.3f),(0.4f,0.28f),(0.15f,0.15f),(0.15f,0.1f)};
        List<(float,float)> shapeHolder = new List<(float,float)>{(0.55f,0.55f),(0.4f,0.48f),(0.4f,0.4f),(0.4f,0.4f)};
        foreach(Text i in dikuArcadeBinding.loadText(textHolder,positionHolder,shapeHolder, new List<string>{"Comic Sans MS"})){
            toRender.Add(i);
            menuButtons.Add(i);
        }
        menuButtons.RemoveAt(0);
        menuButtons.RemoveAt(0);
        backGroundImage = new Entity(new StationaryShape(0.0f,0.0f,1f,1f),new Image(dikuArcadeBinding.getPath("SpaceBackground.png")));
    }


    public void ResetState(){
    }
    
    public void UpdateState(){}
    public void HandleKeyEvent(KeyboardAction action, KeyboardKey key){
        if(action == KeyboardAction.KeyPress){
            switch(key){
                case (KeyboardKey.Up):
                    if(activeMenuButton != 0){
                        activeMenuButton--;
                    }
                    menuButtons[activeMenuButton].SetColor(255,124,252,0);
                    break;
                case (KeyboardKey.Down):
                    if(activeMenuButton != 1){
                        activeMenuButton++;
                    }
                    menuButtons[activeMenuButton].SetColor(255,124,252,0);
                    break;
                case (KeyboardKey.Enter):
                    if(activeMenuButton == 0){
                        StaticTimer.RestartTimer();
                        stateHandler.ActiveState = GameRunning.getOrSetInstance(stateHandler,true);
                    }else{
                        stateHandler.ActiveState = MainMenu.getOrSetInstance(stateHandler);
                    }
                    break;
                default:
                    break;
            }
        }else{
            for(int i = 0;i<menuButtons.Count;i++){
                if(i != activeMenuButton){
                    menuButtons[i].SetColor(255,255,255,255);
                }
            }
        }
    }
    
    public void RenderState(){
        backGroundImage.RenderEntity();
        foreach(Text i in toRender){
            i.RenderText();
        }
    }
}