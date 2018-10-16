using System.Collections.Generic;
using System.IO;
using System.Linq;
using SADJZ.Models;

namespace SADJZ.Services{

    public class LocationReader

    {

        public List<LocationListObject> ReadCSV(string fPath){

            List<LocationListObject> values = File.ReadAllLines(fPath)
                                           .Skip(1)
                                           .Select(v => LocationListObject.FromCsv(v))
                                           .ToList();
            return values;
        }



    }


}