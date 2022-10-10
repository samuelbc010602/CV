namespace BlockTypes.unbreakable;

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
///Class reponsible for the Unbreakable block, 
///</summary>
public class Unbreakable : block{
    string imageName;
    GameRunning caller;
    facade dikuArcadeBinding = new facade();
    public override void hit(uint damage, ball ballCol){ 
        //Console.WriteLine("unbreakable hit"); 
        int count = 0;
        foreach(block i in caller.getBlocks){
            if(i is Unbreakable){
                count++;
            }else{
                break;
            }
        }
        if(count == caller.getBlocks.Count){
            base.CurrentHealth = base.CurrentHealth-(int)damage;
        }
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
    public Unbreakable(Shape shape, IBaseImage image, string imageName, GameRunning caller): base(shape,image){
        this.imageName = imageName;
        this.caller = caller;
        base.CurrentHealth = 100;
        base.StartHealth = base.CurrentHealth;
    }
}