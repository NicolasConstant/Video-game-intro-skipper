using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace VGIS.Azure
{
    public class TableStorageService
    {
        private const string TimeStamp = "Timestamp";
        private readonly string _storageCs;
        private readonly string _tableName;

        public TableStorageService(string storageCs, string tableName)
        {
            _storageCs = storageCs;
            _tableName = tableName;
        }

        public async Task AddEntryToTable(string userId, string gameId, string settingsFileName)
        {
            //Get table ref
            var table = GetTable();

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

        public async Task<List<SettingsEntity>> GetEntries(DateTime from, DateTime to)
        {
            //Get table ref
            var table = GetTable();

            //Retrieve 
            var query = new TableQuery<SettingsEntity>()
                .Where(
                    TableQuery.CombineFilters(
                        TableQuery.GenerateFilterConditionForDate(TimeStamp, QueryComparisons.GreaterThan, from),
                        TableOperators.And,
                        TableQuery.GenerateFilterConditionForDate(TimeStamp, QueryComparisons.LessThan, to)
                    ));

            var seg = await table.ExecuteQuerySegmentedAsync<SettingsEntity>(query, null);
            return seg.Results; //TODO check contiuation token
        }

        private CloudTable GetTable()
        {
            // Parse the connection string and return a reference to the storage account.
            var storageAccount = CloudStorageAccount.Parse(_storageCs);

            // Create the table client.
            var tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            var table = tableClient.GetTableReference(_tableName);
            return table;
        }
    }

    public class SettingsEntity : TableEntity
    {
        #region Ctor
        public SettingsEntity()
        {

        }

        public SettingsEntity(string userId, string gameId)
        {
            PartitionKey = userId;
            RowKey = gameId;
        }
        #endregion

        public string SettingFileName { get; set; }
    }
}