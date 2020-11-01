using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Disasters
{
    [Serializable]
    public class EarthquakeDisaster : Disaster
    {
        public EarthquakeDisaster()
        {
            DisasterType = DisasterType.Earthquake;
        }

        public override string AlertIt()
        {
            return "Please do not be panic, wait until the earthquake ends, put yourself down on any element that can protect you";
        }
    }
}
