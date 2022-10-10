namespace States.MainMenu;
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

using System;
using System.IO;

using Facade;
using Player;
using Entities.Block;
using GetGameBus;
using Statemachine;
///<summary>
///Class reponsible for the state MainMenu
///</summary>
public class MainMenu : IGameState{
    private static MainMenu instance = null;

    private Entity backGroundImage;
    private List<Text> menuButtons;
    private int activeMenuButton = 0;
    private stateMachine stateHandler;

    private facade dikuArcadeBinding = new facade();

    public static MainMenu getOrSetInstance(stateMachine stateMachineToPass) {
        if (MainMenu.instance == null) {
            MainMenu.instance = new MainMenu(stateMachineToPass);
        }
        return MainMenu.instance;
    }

    public MainMenu(stateMachine stateHandler){
        this.stateHandler = stateHandler;
        menuButtons = new List<Text>();
        InitializeGameState();
    }

    private void InitializeGameState(){
        List<string> textHolder = new List<string>{"New Game","Quit"};
        List<(float,float)> positionHolder = new List<(float,float)>{(0.1f,0.2f),(0.1f,0.03f)};
        List<(float,float)> shapeHolder = new List<(float,float)>{(0.45f,0.5f),(0.45f,0.5f)};
        foreach(Text i in dikuArcadeBinding.loadText(textHolder,positionHolder,shapeHolder,new List<string>{"Comic Sans MS"})){
            menuButtons.Add(i);
        }
        backGroundImage = dikuArcadeBinding.placeStationaryEntities((0.0f,0.0f),(1f,1f),"BreakoutTitleScreen.png");
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
        }
    }
    
    public void RenderState(){
        backGroundImage.RenderEntity();
        foreach(Text i in menuButtons){
            //Console.WriteLine("Rendering text buttons");
            i.RenderText();
        }
    }
}