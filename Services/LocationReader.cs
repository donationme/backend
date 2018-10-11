using System.Collections.Generic;
using System.IO;
using System.Linq;
using SADJZ.Models;

namespace SADJZ.Services{

    public class LocationReader

    {

        public List<LocationModel> ReadCSV(string fPath){

            List<LocationModel> values = File.ReadAllLines(fPath)
                                           .Skip(1)
                                           .Select(v => LocationModel.FromCsv(v))
                                           .ToList();
            return values;
        }



    }


}