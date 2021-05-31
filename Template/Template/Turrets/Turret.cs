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
using Template.Enemys;

namespace Template.Turrets
{
    public class Turret
    {
        public float x_position;    //position
        public float y_position;

        public float attack_speed;      //attacken
        public float attack_Wind_UP;
        public float attack_range;

        public List<Minion> minions_in_range = new List<Minion>();      //vem som attackeras
        public List<Minion> list_target = new List<Minion>();

        public Turret(float x_p, float y_p, float AS, float Atk_WUP, float range, List<Minion> in_range, List<Minion> target)
        {
            x_position = x_p;
            y_position = y_p;
            attack_speed = AS;
            attack_Wind_UP = Atk_WUP;
            attack_range = range;
            minions_in_range = in_range;
            list_target = target;
        }
    }
}
