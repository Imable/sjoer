using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Assertions;
using UnityEngine;

namespace Assets.DataManagement
{
    abstract class DataAdapter
    {
        // Takes in a string (JSON?) and spits out DTO object
        public abstract DTO convert(string input);
    }

    class MarineTrafficDataAdapter : DataAdapter
    {
        public override DTO convert(string input)
        {
            throw new NotImplementedException();
        }
    }

    class MockDataAdapter : DataAdapter
    {
        public override DTO convert(string input)
        {
            throw new NotImplementedException();
        }
    }

    class PostgresDataAdapter : DataAdapter
    {
        public override DTO convert(string input)
        {
            throw new NotImplementedException();
        }
    }

    class GPSInfoAdapter : DataAdapter
    {
        // Code courtesy of Carson63000 at https://stackoverflow.com/questions/5089791/nmea-checksum-in-c-sharp-net-cf
        private bool checksum(string input)
        {
            string cs = input.Split('*')[1];

            //Start with first Item
            int checksum = Convert.ToByte(input[input.IndexOf('$') + 1]);
            // Loop through all chars to get a checksum
            for (int i = input.IndexOf('$') + 2; i < input.IndexOf('*'); i++)
            {
                // No. XOR the checksum with this character's value
                checksum ^= Convert.ToByte(input[i]);
            }
            // Return the checksum formatted as a two-character hexadecimal
            return cs == checksum.ToString("X2");
        }

        // Input: $GPRMC,071228.00,A,5402.6015,N,00025.9797,E,0.2,332.1,180921,0.2,W,A,S*50
        public override DTO convert(string input)
        {
            // If the checksum is incorrect, this GPS signal is deemed invalid
            Assert.IsTrue(checksum(input));

            // We'll ignore index 0, because that contains '$GPRMC'
            string[] splitInput = input.Split(',');

            GPSInfoDTO dto = new GPSInfoDTO();

            string t = splitInput[1];
            dto.Valid = splitInput[2] == "A";
            dto.Latitude = double.Parse(splitInput[3]);
            dto.LatNS = splitInput[4];
            dto.Longitude = double.Parse(splitInput[5]);
            dto.LongEW = splitInput[6];
            dto.SOG = double.Parse(splitInput[7]);
            dto.TrueCourse = double.Parse(splitInput[8]);
            string d = splitInput[9];
            dto.DT = this.generateDT(t, d);
            dto.Variation = double.Parse(splitInput[10]);
            dto.VarEW = splitInput[11];

            return dto;
        }

        private DateTime generateDT(string time, string date)
        {
            int ms = time.IndexOf(".");
            string format = ms > 0 ? "ddMMyhhmmss." + new string('f', time.Length - 1 - ms) : "ddMMyhhmmss";
            return DateTime.ParseExact(date+time, format,
                System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
