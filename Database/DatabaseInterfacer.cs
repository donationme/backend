using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
            Model fetchModel = await this.GetModel(model.Id);
            if (fetchModel == null){
                collection.InsertOne(model);   
                return true;
            }else{
                return false;
            }
        }


        public bool UpdateModel<S>(string id, Expression<Func<Model,S>> fieldDef, S value)
        {

            var filter = Builders<Model>.Filter.Where(c => c.Id == id);
            var update = Builders<Model>.Update.Set(fieldDef, value);
            var updated = collection.UpdateMany(filter, update);
            return updated.IsAcknowledged;
        }


        public async Task<Model> GetModel(string id){
            List<Model> Models = await collection.Find(model => model.Id == id).ToListAsync();
            if (Models.Count >= 1){
                return Models[0];
            }
            return null;
        }

        public bool ModelExists(string id){
            bool modelExists = collection.Find(model => model.Id == id).Any();
            return modelExists;
        }
    }
}


