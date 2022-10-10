namespace Facade;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Security.Principal;

using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Input;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;
using DIKUArcade.Physics;

using Entities.Block;
using Player;
using Entities.Ball;
using GetGameBus;
using BallTypes.powerUpBall;

using States.GameRunning;
using System.Collections;

///<summary>
///Class reponsible for communication with the DIKUArcade game-engine
///</summary>
public class facade{

    public string getPath(string fileName){
        string pathToInsert = Path.Combine("Assets","Images",$"{fileName.Replace(" ","")}");
        if(Directory.GetCurrentDirectory().Split(@"\").Last() != "Breakout"){
            pathToInsert = Directory.GetCurrentDirectory().Remove(Directory.GetCurrentDirectory().Length-22);
            pathToInsert = pathToInsert+$@"\Assets\Images\{fileName}";
        }
        return pathToInsert;
    }
    public List<Text> loadText(List<string> textToPrint, List<(float,float)> positions, List<(float,float)> shapes,List<string> fonts){
        List<Text> textRet = new List<Text>();
        for(int i = 0;i<textToPrint.Count;i++){
            textRet.Add(new Text(textToPrint[i], new DIKUArcade.Math.Vec2F(positions[i].Item1,positions[i].Item2),new DIKUArcade.Math.Vec2F(shapes[i].Item1,shapes[i].Item2)));
            textRet[i].SetColor(255,255,255,255);
            textRet[i].SetFont(fonts[i%fonts.Count()]);
        }
        return textRet;
    }
    public List<ball> placeBall(List<(float,float)> positions, List<(float,float)> shapes, List<string> imagePath, List<Type> specialities, GameRunning caller){
        List<ball> entitiesContainer = new List<ball>();
        for(int i = 0;i<positions.Count();i++){
            entitiesContainer.Add((ball)Activator.CreateInstance(specialities[i%specialities.Count],new DynamicShape(positions[i].Item1,positions[i].Item2,shapes[i%shapes.Count].Item1,shapes[i%shapes.Count].Item2), new Image(getPath(imagePath[i%imagePath.Count])),caller));
            //getGameBus.GetBus().Subscribe(GameEventType.InputEvent,entitiesContainer.Last());
            //getGameBus.GetBus().Subscribe(GameEventType.TimedEvent,entitiesContainer.Last());
        }
        return entitiesContainer;
    }
    public ball placeBall((float,float) positions, (float,float) shapes, string imagePath, Type specialities, GameRunning caller){
        return placeBall(new List<(float, float)>{(positions.Item1,positions.Item2)}, new List<(float, float)>{(shapes.Item1,shapes.Item2)}, new List<string>{imagePath},new List<Type>{specialities},caller)[0];
    }

    public player placePlayer((float,float) positions,(float,float) shapes, string imagePath){
        player ret = new player(new DynamicShape(positions.Item1,positions.Item2,shapes.Item1,shapes.Item2), new Image(getPath(imagePath)));
        getGameBus.GetBus().Subscribe(GameEventType.InputEvent,ret);
        getGameBus.GetBus().Subscribe(GameEventType.TimedEvent,ret);
        return ret;
    }
    public List<Entity> placeStationaryEntities(List<(float,float)> positions, List<(float,float)> shapes, List<string> imagePath){
        List<Entity> entitiesContainer = new List<Entity>();
        List<object> argumentList = new List<object>();
        for(int i = 0;i<positions.Count();i++){
            entitiesContainer.Add(new Entity(new StationaryShape(positions[i].Item1,positions[i].Item2,shapes[i%shapes.Count()].Item1,shapes[i%shapes.Count()].Item2), new Image(getPath(imagePath[i%imagePath.Count()]))));
        }
        return entitiesContainer;
    }
    public Entity placeStationaryEntities((float,float) position,(float,float) shape, string imagePath){
        return placeStationaryEntities(new List<(float,float)>{position},new List<(float,float)>{shape},new List<string>{imagePath})[0];
    }

    public List<block> placeBlocks(List<(float,float)> positions, List<(float,float)> shapes, List<string> imagePath, List<Type> specialities, GameRunning caller){
        List<block> entitiesContainer = new List<block>();
        for(int i = 0;i<positions.Count();i++){
            entitiesContainer.Add((block)Activator.CreateInstance(specialities[i%specialities.Count],new StationaryShape(positions[i].Item1,positions[i].Item2,
                                                                                                    shapes[i%shapes.Count].Item1,shapes[i%shapes.Count].Item2), 
                                                                                                    new Image(getPath(imagePath[i%imagePath.Count])), 
                                                                                                    imagePath[i%imagePath.Count], caller));
        }
        return entitiesContainer;
    }

    
    public block placeBlocks((float,float) positions, (float,float) shapes, string imagePath, Type specialities, GameRunning caller){
        return placeBlocks(new List<(float, float)>{(positions.Item1,positions.Item2)}, new List<(float, float)>{(shapes.Item1,shapes.Item2)}, new List<string>{imagePath},new List<Type>{specialities},caller)[0];
    }

    public (T,bool,float,float,CollisionDirection) checkForCollision<T>(DynamicShape actor,List<T> collidingElements) where T : Entity{
        foreach(T i in collidingElements){
            CollisionData data = CollisionDetection.Aabb(actor,i.Shape);
            if(data.Collision){
                return (i,true,data.DirectionFactor.X,data.DirectionFactor.Y,data.CollisionDir);
            }
        }
        return (default,false,0.0f,0.0f,CollisionDirection.CollisionDirUnchecked);
    }

    public bool checkForNewPosition<T>((float,float,float,float) positionAndShape,List<T> collidingElements, (float,float) directionToMove) where T : Entity{
        DynamicShape testActor = new DynamicShape(positionAndShape.Item1,positionAndShape.Item2,positionAndShape.Item3,positionAndShape.Item4);
        testActor.Direction = new Vec2F(directionToMove.Item1,directionToMove.Item2);
        testActor.Move();
        foreach(T i in collidingElements){
            CollisionData data = CollisionDetection.Aabb(testActor,i.Shape);
            if(data.Collision){
                return true;
            }
        }
        return false;
    }

}
