using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Disasters
{
    [Serializable]
    public abstract class Disaster : IComparable
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DisasterType DisasterType { get; set; }

        private DateTime reportedOn;
        public DateTime ReportedOn 
        {
            get
            {
                return reportedOn;
            }
            set
            {
                if (value != DateTime.MinValue && value != DateTime.MaxValue)
                    reportedOn = value;
            }
        }

        public abstract string AlertIt();

        public int CompareTo(object otherDisastrerObj)
        {
            Disaster otherDisastrer = otherDisastrerObj as Disaster;
            return ReportedOn.CompareTo(otherDisastrer.ReportedOn);
        }
    }
}
