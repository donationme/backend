using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SADJZ.Models;

namespace SADJZ.Services{

    public class LocationReader

    {

        public  static List<LocationCollectionObject> ReadCSV(string csv){
            string[] splitted = csv.Split(new string[] {System.Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

            List<LocationCollectionObject> values = splitted
                                           .Skip(1)
                                           .Select(v => LocationCollectionObject.FromCsv(v))
                                           .ToList();
            return values;
        }



    }


}








