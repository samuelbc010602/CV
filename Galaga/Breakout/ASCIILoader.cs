namespace ASCIILoader;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

using BlockTypes.hardened;
using BlockTypes.normalBlock;
using BlockTypes.unbreakable;
using BlockTypes.invinsible;
using BlockTypes.hungry;
using BlockTypes.healing;
using BlockTypes.teleporting;

///<summary>
///Class reponsible for the asciiLoader which is used to read from different level files
///</summary>
public class asciiLoader{
    private List<string> asciiMap = new List<string>();   
    private Dictionary<string,string> metaData = new Dictionary<string,string>();
    private Dictionary<string,Type> blockTypes = new Dictionary<string,Type>();
    private Dictionary<string,string> pictureCategory = new Dictionary<string,string>();
    public void loadASCII(string fileName){
        asciiMap = new List<string>();
        metaData = new Dictionary<string,string>();
        blockTypes = new Dictionary<string,Type>();
        pictureCategory = new Dictionary<string,string>();
        
        string pathToInsert = Path.Combine("Assets","Levels",$"{fileName.Replace(" ","")}");
        if(Directory.GetCurrentDirectory().Split(@"\").Last() != "Breakout"){
            pathToInsert = Directory.GetCurrentDirectory().Remove(Directory.GetCurrentDirectory().Length-22);
            pathToInsert = pathToInsert+$@"\Assets\Levels\{fileName}";
        }
        using(StreamReader reader = File.OpenText(pathToInsert)){ 
            while(!reader.EndOfStream){
                string input = reader.ReadLine();
                if(input == "Map:"){
                    input = reader.ReadLine();
                    while(input != "Map/"){
                        if(input.Length == 12){
                            asciiMap.Add(input);
                            input = reader.ReadLine();
                        }
                        else{
                            throw new Exception("Invalid length in ASCII map");
                        }
                    }
                    /*foreach(string i in getAsciiMap){
                        Console.WriteLine(i);
                    }*/
                }
                if(input == "Meta:"){
                    input = reader.ReadLine();
                    while(input != "Meta/"){
                        List<string> temp = new List<string>();
                        temp = input.Split(":").ToList();
                        if(temp.Count != 2 || temp[1].Replace(" ","").Length == 0){
                            throw new Exception("Invalid meta data");
                        }
                        metaData.Add(temp[0],temp[1].Remove(0,1));

                        //refactor to go through blockTypes
                        
                        if(temp[0] == "Hardened" && !blockTypes.ContainsKey(temp[1])){
                            blockTypes.Add(temp[1].Remove(0,1),typeof(Hardened));
                        }else if(temp[0] == "Unbreakable" && !blockTypes.ContainsKey(temp[1])){
                            blockTypes.Add(temp[1].Remove(0,1),typeof(Unbreakable));
                        }else if(temp[0] == "Invincible" && !blockTypes.ContainsKey(temp[1])){
                            blockTypes.Add(temp[1].Remove(0,1),typeof(Invinsible));
                        }else if(temp[0] == "Healing" && !blockTypes.ContainsKey(temp[1])){
                            blockTypes.Add(temp[1].Remove(0,1),typeof(Healing));
                        }else if(temp[0] == "Hungry" && !blockTypes.ContainsKey(temp[1])){
                            Console.WriteLine("added hungry");
                            blockTypes.Add(temp[1].Remove(0,1),typeof(Hungry));
                        }else if(temp[0] == "Teleporting" && !blockTypes.ContainsKey(temp[1])){
                            Console.WriteLine("added teleporting");
                            blockTypes.Add(temp[1].Remove(0,1),typeof(Teleporting));
                        }

                        input = reader.ReadLine();
                    }
                }
                if(input == "Legend:"){
                    input = reader.ReadLine();
                    while(input != "Legend/"){
                        List<string> temp = new List<string>();
                        temp = input.Split(")").ToList();
                        if(temp.Count != 2 || temp[1].Replace(" ","").Length == 0){
                            throw new Exception("Invalid legend data");
                        }
                        pictureCategory.Add(temp[0],temp[1].Remove(0,1));
                        input = reader.ReadLine();
                    }
                }
            }
            if(asciiMap.Count != 25){
                throw new Exception("Invalid size of ASCII map");
            }
            
        }
        asciiMap.Reverse();
    }
    public List<string> getAsciiMap{
        get{return asciiMap;}
    }
    public IDictionary<string,string> getMetaData{
        get{return metaData;}
    }
    public IDictionary<string,string> getPictureCategory{
        get{return pictureCategory;}
    }
    public IDictionary<string,Type> getBlockTypes{
        get{return blockTypes;}
    }
}