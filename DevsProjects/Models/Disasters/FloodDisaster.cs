using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Disasters
{
    [Serializable]
    public class FloodDisaster : Disaster
    {
        public FloodDisaster()
        {
            DisasterType = DisasterType.Flood;
        }

        public override string AlertIt()
        {
            return "Run!!!, put you and your family away from the river and the most high of the ground";
        }
    }
}
