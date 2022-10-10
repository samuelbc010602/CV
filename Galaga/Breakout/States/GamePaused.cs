namespace States.GamePaused;
using States.GameRunning;

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
using System.Windows;

using Facade;
using Player;
using Entities.Block;
using GetGameBus;
using Statemachine;

///<summary>
///Class reponsible for the state GamePaused
///</summary>
public class GamePaused : IGameState{
    private static GamePaused instance = null;

    private Entity backGroundImage;
    private List<Text> menuButtons;
    private List<Text> toRender;
    private int activeMenuButton = 0;
    private stateMachine stateHandler;

    private facade dikuArcadeBinding = new facade();

    private GameRunning caller;

    public static GamePaused getOrSetInstance(stateMachine stateMachineToPass) {
        if (GamePaused.instance == null) {
            GamePaused.instance = new GamePaused(stateMachineToPass);
        }
        return GamePaused.instance;
    }

    public GamePaused(stateMachine stateHandler){
        this.stateHandler = stateHandler;
        menuButtons = new List<Text>();
        toRender = new List<Text>();

        caller = GameRunning.getOrSetInstance(stateHandler);
        /*foreach(uint i in caller.getId){
            getGameBus.GetBus().CancelTimedEvent(i);
        }*/
        InitializeGameState();
    }

    private void InitializeGameState(){
        List<string> textHolder = new List<string>{"Pause","New Game","Continue","Quit"};
        List<(float,float)> positionHolder = new List<(float,float)>{(0.4f,0.3f),(0.1f,0.15f),(0.1f,0.1f),(0.1f,0.05f)};
        List<(float,float)> shapeHolder = new List<(float,float)>{(0.55f,0.55f),(0.4f,0.4f),(0.4f,0.4f),(0.4f,0.4f)};
        foreach(Text i in dikuArcadeBinding.loadText(textHolder,positionHolder,shapeHolder,new List<string>{"Comic Sans MS"})){
            toRender.Add(i);
            menuButtons.Add(i);
        }
        menuButtons.RemoveAt(0);
        backGroundImage = new Entity(new StationaryShape(0.0f,0.0f,1f,1f),new Image(dikuArcadeBinding.getPath("SpaceBackground.png")));
    }


    public void ResetState(){
    }
    
    public void UpdateState(){

    }
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
                    if(activeMenuButton != 2){
                        activeMenuButton++;
                    }
                    menuButtons[activeMenuButton].SetColor(255,124,252,0);
                    break;
                case (KeyboardKey.Enter):
                        if(activeMenuButton == 0){
                            StaticTimer.RestartTimer();
                            stateHandler.ActiveState = GameRunning.getOrSetInstance(stateHandler,true);
                        }
                        else if(activeMenuButton == 1){
                            StaticTimer.ResumeTimer();
                            stateHandler.ActiveState = GameRunning.getOrSetInstance(stateHandler);
                        }else{
                            getGameBus.GetBus().RegisterEvent(new GameEvent{
                                EventType = GameEventType.GraphicsEvent,
                                Message = "Close Window"
                            });
                            getGameBus.GetBus().ProcessEvents();
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
            /*switch(key){
                case (KeyboardKey.Up):
                    if(activeMenuButton == 1){
                        menuButtons[0].SetColor(255,255,255,255);
                    }else{
                        menuButtons[1].SetColor(255,255,255,255);
                    }
                    break;

                case (KeyboardKey.Down):
                    if(activeMenuButton == 1){
                        menuButtons[0].SetColor(255,255,255,255);
                    }else{
                        menuButtons[1].SetColor(255,255,255,255);
                        menuButtons[2].SetColor(255,255,255,255);
                    }
                    break;

                default:
                    break;
                    */
        }
    }
    
    public void RenderState(){
        backGroundImage.RenderEntity();
        foreach(Text i in toRender){
            i.RenderText();
        }
    }
}