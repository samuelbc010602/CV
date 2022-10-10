namespace BlockTypes.normalBlock;

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
///Class reponsible for the NormalBlock block, 
///</summary>
public class NormalBlock: block{
    string imageName;
    GameRunning caller;
    facade dikuArcadeBinding = new facade();
    public override void hit(uint damage, ball ballCol){
        base.CurrentHealth = base.CurrentHealth-(int)damage;
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
    public NormalBlock(Shape shape, IBaseImage image, string imageName, GameRunning caller): base(shape,image){
        this.caller = caller;
        this.imageName = imageName;
        base.CurrentHealth = 100;
        base.StartHealth = base.CurrentHealth;
    }
}