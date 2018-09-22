using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using SADJZ.Models;

namespace SADJZ.Database
{

    public sealed class AccountDatabase
    {
        private IMongoCollection<AccountModel> collection;
        public AccountDatabase()
        {
            var connectionString = "mongodb://localhost:27017";

            var client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase("SADJZ");

            this.collection = db.GetCollection<AccountModel>("user");
            
        }

        public async Task<bool> AddAccount(AccountModel model)
        {
            AccountModel userModel = await this.GetAccount(model.Auth.Username);
            if (userModel == null){
                collection.InsertOne(model);   
                return true;
            }else{
                return false;
            }
        }

        public async Task<AccountModel> GetAccount(string username){
            List<AccountModel> accountModels = await collection.Find(account => account.Auth.Username == username).ToListAsync();
            if (accountModels.Count >= 1){
                return accountModels[0];
            }else{
                return null;
            }
        }

        public async Task<bool> ValidateAccount(string username, string password){
            List<AccountModel> accountModels = await collection.Find(account => account.Auth.Username == username).ToListAsync();
            if (accountModels[0].Auth.Password == password){
                return true;
            }else{
                return false;
            }
        }
    }
}


