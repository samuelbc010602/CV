using System;
using Game;
using DIKUArcade.GUI;
using ASCIILoader;
using States.GameRunning;

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Program
{
    public class StartingPoint{
        public static void Main(){
            var windowArgs = new WindowArgs() { Title = "Breakout" };
            game startPoint = new game(windowArgs); 
            startPoint.Run();
        }
    }


}
