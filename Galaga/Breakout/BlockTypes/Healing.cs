namespace BlockTypes.healing;

using Entities.Block;
using Entities.Ball;

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

using States.GameRunning;
using DIKUArcade.Timers;

///<summary>
///Class reponsible for the Healing block, 
///</summary>
public class Healing : block{
    string imageName;
    GameRunning caller;
    facade dikuArcadeBinding = new facade();
    public override void hit(uint damage, ball ballCol){
        //Console.WriteLine("healing hit");
        if(StaticTimer.GetElapsedSeconds() >= 15.0 && StaticTimer.GetElapsedSeconds() <= 25.0){
            base.CurrentHealth +=15;
        }
        base.CurrentHealth = base.CurrentHealth-(int)damage;
        //Console.WriteLine($"healing hit called {base.CurrentHealth}");
        if(base.CurrentHealth <= base.StartHealth/2){
            base.getSetImage = new Image(dikuArcadeBinding.getPath(imageName.Substring(0,imageName.Length-4)+"-damaged.png"));
        }
    }
    public Healing(Shape shape, IBaseImage image, string imageName, GameRunning caller): base(shape,image){
        StaticTimer.RestartTimer();
        this.imageName = imageName;
        base.CurrentHealth = 100;
        base.StartHealth = base.CurrentHealth;
    }
}