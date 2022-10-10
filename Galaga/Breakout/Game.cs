using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Input;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.Security.Principal;
using System.Collections.Generic;
using DIKUArcade.Events;
using System;
using DIKUArcade.Physics;

using ASCIILoader;
using Facade;
using GetGameBus;

using Statemachine;
using Entities.Block;

namespace Game
{
    public class game : DIKUGame, IGameEventProcessor
    {
        private stateMachine stateHandler;
        private bool windowIsClosed = false;
        public game(WindowArgs windowArgs) : base(windowArgs) {
            stateHandler = new stateMachine();
            window.SetKeyEventHandler(keyHandle);
            try{getGameBus.GetBus().InitializeEventBus(new List<GameEventType>{GameEventType.InputEvent,GameEventType.TimedEvent,GameEventType.GraphicsEvent});}
            catch{}
            getGameBus.GetBus().Subscribe(GameEventType.GraphicsEvent,this);

        }

        public void keyHandle(KeyboardAction action,KeyboardKey key){
            stateHandler.ActiveState.HandleKeyEvent(action,key);
        }
        public void ProcessEvent(GameEvent gameEvent) {
            if(gameEvent.Message == "Close Window"){
                windowIsClosed = true;
                window.CloseWindow();
            }
        }
        public override void Render()
        {
            window.Clear();
            stateHandler.ActiveState.RenderState();
        }
        public override void Update()
        {
            window.PollEvents();
            stateHandler.ActiveState.UpdateState();
        }

        public stateMachine getStateMachine{
            get{return stateHandler;}
        }

        public bool getClosed{
            get{return windowIsClosed;}
        }
    }
}