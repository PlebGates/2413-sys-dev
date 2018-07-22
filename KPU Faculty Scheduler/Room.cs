using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPU_Faculty_Scheduler
{
    public class Room
    {

        // Variables to hold the room name and id.
        public int id;
        public string name; 

        // Boolean variable for whether the room has a computer lab or not.
        public bool hasComputers;

        // Default Constructor.
        public Room()
        {

        }

        // Main Constructor.
        public Room(int id_, string name_, bool hasComputers_)
        {
            id = id_;
            name = name_;
            hasComputers = hasComputers_;
        }


    }
}
