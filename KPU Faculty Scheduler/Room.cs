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
        public int roomNum;
        public String building; 

        // Boolean variable for whether the room has a computer lab or not.
        public Boolean hasComputers;

        // Default Constructor.
        public Room()
        {

        }

        // Main Constructor.
        public Room(int id_, int roomNum_, String building_, Boolean hasComputers_)
        {
            id = id_;
            roomNum = roomNum_;
            building = building_;
            hasComputers = hasComputers_;
        }


    }
}
