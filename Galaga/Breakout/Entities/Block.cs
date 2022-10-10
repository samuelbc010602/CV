namespace Entities.Block;

using DIKUArcade.Events;

using ASCIILoader;
using Facade;
using Entities.Ball;


using DIKUArcade.Entities;
using DIKUArcade.Graphics;
///<summary>
///Class reponsible for all blocks
///</summary>
public abstract class block : Entity{
    private int startHealth;
    private int currentHealth;
    private int val = 1;
    public abstract void hit(uint damage, ball caller = null);
    public int StartHealth{
        get{return startHealth;}
        protected set{startHealth = value;}
    }
    public int CurrentHealth{
        get{return currentHealth;}
        protected set{currentHealth = value;}
    }
    public int Val{
        get{return val;}
        protected set{val = value;}
    }

    public Shape GetShape{
        get{return base.Shape;}
    }

    public Image getSetImage{
        get{return (Image)base.Image;}
        protected set{base.Image = value;}
    }

    public block(Shape shape, IBaseImage image):base(shape.AsStationaryShape(),image){
    }
}