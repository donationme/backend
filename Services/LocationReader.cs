using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SADJZ.Models;

namespace SADJZ.Services{

    public class RegionReader

    {

        public  static List<LocationModel> ReadCSV(string csv){
            string[] splitted = csv.Split(new string[] {System.Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

            List<LocationModel> values = splitted
                                           .Skip(1)
                                           .Select(v => LocationModel.FromCsv(v))
                                           .ToList();
            return values;
        }



    }


}








