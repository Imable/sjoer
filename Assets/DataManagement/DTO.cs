using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.DataManagement
{
    public abstract class DTO
    {
        
    }

    [Serializable]
    public class MarineTrafficAISDTO : DTO
    {
        public string Identifier;
        public double Latitude;
        public double Longitude;
    }

    public class GPSInfoDTO : DTO
    {
        public DateTime DT { get; set; }
        public bool Valid { get; set; }
        public double Latitude { get; set; }
        public string LatNS { get; set; }
        public double Longitude { get; set; }
        public string LongEW { get; set; }
        public double SOG { get; set; }
        public double TrueCourse { get; set; }
        public double Variation { get; set; }
        public string VarEW { get; set; }
        public string Checksum { get; set; }
    }
}
