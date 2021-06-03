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
        public float x_position;    //position
        public float y_position;

        public float x_speed;       //hastighet + riktning
        public float y_speed;

        public int health;      //hälsa

        public float units_traveled;        //antal units som färdats          så att torn ska attackera den som färdats längst

        public Minion(float x_p, float y_p, float x_s, float y_s, int hp, float traveled)
        {
            x_position = x_p;
            y_position = y_p;

            x_speed = x_s;
            y_speed = y_s;

            health = hp;

            units_traveled = traveled;
        }
    }
}
