namespace BlockTypes.hardened;

using Entities.Block;

using DIKUArcade;

using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Events;

using System.Security.Principal;
using System.Collections.Generic;
using System.IO;
using System;

using ASCIILoader;
using Facade;
using Entities.Ball;

using States.GameRunning;

///<summary>
///Class reponsible for the Hardened block, 
///</summary>

public class Hardened : block{
    string imageName;
    GameRunning caller;
    facade dikuArcadeBinding = new facade();
    public override void hit(uint damage, ball ballCol){
        //Console.WriteLine("hardened hit");
        base.CurrentHealth = base.CurrentHealth-(int)damage;
        if(base.CurrentHealth <= base.StartHealth/2){
            base.getSetImage = new Image(dikuArcadeBinding.getPath(imageName.Substring(0,imageName.Length-4)+"-damaged.png"));
        }
    }
    public Hardened(Shape shape, IBaseImage image, string imageName, GameRunning caller): base(shape,image){
        this.imageName = imageName;
        base.CurrentHealth = 200;
        base.StartHealth = base.CurrentHealth;
        base.Val = 2;
    }
}