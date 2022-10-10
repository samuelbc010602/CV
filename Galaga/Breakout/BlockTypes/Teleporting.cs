namespace BlockTypes.teleporting;

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

using BlockTypes.normalBlock;

using States.GameRunning;

///<summary>
///Class reponsible for the Teleporting block, 
///</summary>
public class Teleporting : block{
    string imageName;
    private bool teleported = false;
    GameRunning caller;
    List<ball> ballEnts = new List<ball>();
    facade dikuArcadeBinding = new facade();
    List<(float,float)> emptySpace = new List<(float,float)>();
    asciiLoader asciiReader = new asciiLoader();
    Random rnd = new Random();
    float x = 0.1f;
    float y = 0.2f;

    public override void hit(uint damage, ball ballCol){
        //Console.WriteLine("teleporting hit");
        asciiReader.loadASCII($"level{caller.getCurrentLevel}.txt");
        if(!teleported){
            for(int i = 0;i<asciiReader.getAsciiMap.Count;i++){
                if(i >= 15){
                    foreach(char inner in asciiReader.getAsciiMap[i]){
                        if(inner == '-'){
                            bool actualEmpty = true;
                            foreach(block checkBlock in caller.getBlocks){
                                if(checkBlock.Shape.AsDynamicShape().Position.X == x && checkBlock.Shape.AsDynamicShape().Position.Y == y){
                                    actualEmpty = true;
                                }
                            }
                            if(actualEmpty){
                                emptySpace.Add((x,y));
                                x=x+0.07f;
                            }
                        }else{
                            x = x+0.07f;
                        }
                    }
                    y = y+0.03f;
                    x = 0.1f;
                }else{
                    continue;
                }
            }
            
            if(emptySpace.Count != 0){
                caller.getBlocks.Add(dikuArcadeBinding.placeBlocks((emptySpace[rnd.Next(0,asciiReader.getAsciiMap.Count-1)].Item1,emptySpace[rnd.Next(0,asciiReader.getAsciiMap.Count-1)].Item2),(0.07f,0.03f),imageName,typeof(NormalBlock),caller));
            }
        }
        base.CurrentHealth = base.CurrentHealth-(int)damage;
    }

    public Teleporting(Shape shape, IBaseImage image, string imageName, GameRunning caller) : base(shape,image){
        this.caller = caller;
        ballEnts = caller.getBalls;

        base.CurrentHealth = 100;
        base.StartHealth = base.CurrentHealth;

        this.imageName = imageName;
    }
}
