using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using Template;

namespace Template.Enemys
{
    public class Minion
    {
        //public float x_position;    //position
        //public float y_position;
        public Vector2 Position;

        //public float x_speed;       //hastighet + riktning
        //public float y_speed;
        public Vector2 Speed;

        public int health;      //hälsa

        public float units_traveled;        //antal units som färdats          så att torn ska attackera den som färdats längst

        public Minion(Vector2 position, Vector2 speed, int hp, float traveled)
        {
            Position = position;

            Speed = speed;

            health = hp;

            units_traveled = traveled;
        }
    }
}
