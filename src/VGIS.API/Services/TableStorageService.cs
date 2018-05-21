using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Vgis_crowdsourcing_api.Services
{
    public class TableStorageService
    {
        private readonly string _storageCs;
        private readonly string _tableName;

        public TableStorageService(string storageCs, string tableName)
        {
            _storageCs = storageCs;
            _tableName = tableName;
        }

        public async Task AddEntryToTable(string userId, string gameId, string settingsFileName)
        {

            // Parse the connection string and return a reference to the storage account.
            var storageAccount = CloudStorageAccount.Parse(_storageCs);

            // Create the table client.
            var tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            var table = tableClient.GetTableReference(_tableName);

            // Create the table if it doesn't exist.
            await table.CreateIfNotExistsAsync();

            //Create entity 
            var settingsEntity = new SettingsEntity(userId, gameId)
            {
                SettingFileName = settingsFileName
            };

            // Create the TableOperation object that inserts the customer entity.
            var insertOperation = TableOperation.InsertOrReplace(settingsEntity);

            // Execute the insert operation.
            await table.ExecuteAsync(insertOperation);
        }

    }

    public class SettingsEntity : TableEntity
    {
        public SettingsEntity(string userId, string gameId)
        {
            PartitionKey = userId;
            RowKey = gameId;
        }
        
        public string SettingFileName { get; set; }
    }
}