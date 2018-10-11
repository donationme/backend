using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using SADJZ.Consts;
using SADJZ.Models;

namespace SADJZ.Database
{

    public sealed class DatabaseInterfacer<Model> where Model: DatabaseEntry
    {
        private IMongoCollection<Model> collection;
        public DatabaseInterfacer(DatabaseEndpoints endpoint)
        {
            var connectionString = AppInformtion.DatabseAdress;

            var client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase(AppInformtion.AppName);

            this.collection = db.GetCollection<Model>(endpoint.ToString());
            
        }

        public async Task<bool> AddModel(Model model)
        {
            Model userModel = await this.GetModel(model.Id);
            if (userModel == null){
                collection.InsertOne(model);   
                return true;
            }else{
                return false;
            }
        }

        public async Task<Model> GetModel(string id){
            List<Model> Models = await collection.Find(model => model.Id == id).ToListAsync();
            if (Models.Count >= 1){
                return Models[0];
            }
            return null;
        }

        public bool ModelExists(string username, string password){
            bool modelExists = collection.Find(model => model.Id == username).Any();
            return modelExists;
        }
    }
}


