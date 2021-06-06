using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Template.Enemys;

namespace Template.Shots
{
    public class Shot
    {
        //public float x_position;    //position
        //public float y_position;
        public Vector2 Position;

        //public float x_speed;       //hastighet och riktning
        //public float y_speed;
        public Vector2 Speed;

        public float units_traveled;        //hur länge ett skott ska åka innan den försvinner
        public float units_to_target;       //hur mycket ett skot ska färdas innan den ska försvinna(kommer till fiende)

        public List<Minion> list_target = new List<Minion>();

        public Shot(Vector2 position, Vector2 speed, float traveled, float to_target, List<Minion> target)
        {
            Position = position;

            Speed = speed; 

            units_traveled = traveled;
            units_to_target = to_target;

            list_target = target;
        }
    }
}
