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
        public String name;

        // Boolean variable for whether the room has a computer lab or not.
        public Boolean hasComputers;

        // Default Constructor.
        public Room()
        {

        }

        // Main Constructor.
        public Room(int id_, String name_, Boolean hasComputers_)
        {
            id = id_;
            name = name_;
            hasComputers = hasComputers_;
        }


    }
}
