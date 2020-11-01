using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Disasters
{
    [Serializable]
    public class EpidemicDisaster : Disaster
    {
        public EpidemicDisaster()
        {
            DisasterType = DisasterType.Epidemic;
        }

        public override string AlertIt()
        {
            return "Please, go to any medical provider right away to follow a apropiate health plan";
        }
    }
}
