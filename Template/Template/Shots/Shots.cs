using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Enemys;

namespace Template.Shots
{
    public class Shot
    {
        public float x_position;    //position
        public float y_position;

        public float x_speed;       //hastighet och riktning
        public float y_speed;

        public float units_traveled;        //hur länge ett skott ska åka innan den försvinner
        public float units_to_target;       //hur mycket ett skot ska färdas innan den ska försvinna(kommer till fiende)

        public List<Minion> list_target = new List<Minion>();

        public Shot(float x_p, float y_p, float x_s, float y_s, float traveled, float to_target, List<Minion> target)
        {
            x_position = x_p;
            y_position = y_p;

            x_speed = x_s;
            y_speed = y_s;

            units_traveled = traveled;
            units_to_target = to_target;

            list_target = target;
        }
    }
}
