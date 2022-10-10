namespace BlockTypes.invinsible;

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
using States.GameRunning;
///<summary>
///Class reponsible for the Invinsible block, 
///</summary>
public class Invinsible : block{
    string imageName;
    GameRunning caller;
    facade dikuArcadeBinding;
    Image visibleImage;
    public override void hit(uint damage, ball ballCol){
        //Console.WriteLine("invisible hit");
        base.getSetImage = visibleImage;
        base.CurrentHealth = base.CurrentHealth-(int)damage;
        //Console.WriteLine($"insible hit called {base.CurrentHealth}");
        if(base.CurrentHealth <= base.StartHealth/2){
            base.getSetImage = new Image(dikuArcadeBinding.getPath(imageName.Substring(0,imageName.Length-4)+"-damaged.png"));
        }
    }
    public Shape GetShapeFromBase{
        get{return base.GetShape;}
    }
    public int getVal{
        get{return base.Val;}
    }
    public int getCurrentHealth{
        get{return base.CurrentHealth;}
    }
    public Invinsible(Shape shape, IBaseImage image, string imageName, GameRunning caller): base(shape,image){

        dikuArcadeBinding = new facade();

        visibleImage = (Image)image;
        //Console.WriteLine("setting image");
        base.getSetImage = new Image(dikuArcadeBinding.getPath("invisible-block.png"));

        this.imageName = imageName;
        base.CurrentHealth = 150;
        base.StartHealth = base.CurrentHealth;
    }
}