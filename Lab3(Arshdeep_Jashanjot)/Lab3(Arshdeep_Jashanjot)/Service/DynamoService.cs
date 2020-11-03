using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Lab3_Arshdeep_Jashanjot_.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Lab3_Arshdeep_Jashanjot_.Service
{
    public class DynamoService : ITableDataService
    {
        public readonly DynamoDBContext DbContext;
        private AmazonDynamoDBClient client;
        private BasicAWSCredentials credentials;
        
        public DynamoService()
        {
            
            credentials = new BasicAWSCredentials(ConfigurationManager.AppSettings["accessId"], ConfigurationManager.AppSettings["secretKey"]);
            client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast2);
            //SingleTableBatchWrite(DbContext);

            DbContext = new DynamoDBContext(client, new DynamoDBContextConfig
            {
                //Setting the Consistent property to true ensures that you'll always get the latest 
                ConsistentRead = true,
                SkipVersionCheck = true
            });
        }
        public void BatchStore<T>(IEnumerable<T> items) where T : class
        {
            var itemBatch = DbContext.CreateBatchWrite<T>();

            foreach (var item in items)
            {
                itemBatch.AddPutItem(item);
            }

            itemBatch.ExecuteAsync();
        }

        public IEnumerable<T> GetAll<T>(string name) where T : class
        {
            var scanConditions = new List<ScanCondition>()
            {new ScanCondition("name", ScanOperator.Equal, name) };
            // ScanFilter scanFilter = new ScanFilter();
            //   scanFilter.AddCondition("Email", ScanOperator.Equal, userMail);
            var items = DbContext.ScanAsync<T>(scanConditions, null);
            return (IEnumerable<T>)items;
        }

        public T GetItem<T>(string key) where T : class
        {
            var item = DbContext.LoadAsync<T>(key);
            return item.Result;
        }

        public void Store<T>(T item) where T : new()
        {
            DbContext.SaveAsync(item);
        }

        public void UpdateItem<T>(T item) where T : class
        {
            var savedItem = DbContext.LoadAsync(item);

            if (savedItem == null)
            {
                throw new AmazonDynamoDBException("The item does not exist in the Table");
            }

            DbContext.SaveAsync(item);
        }
        public void DeleteItem<T>(T item)
        {
            var savedItem = DbContext.LoadAsync(item);

            if (savedItem == null)
            {
                throw new AmazonDynamoDBException("The item does not exist in the Table");
            }

            DbContext.DeleteAsync(item);
        }
        private static void SingleTableBatchWrite(DynamoDBContext context)
        {
            Movie movie1 = new Movie
            {
                id = 902,
                name = "Cinematic Background HD",
                url = "https://moviesnet.s3.us-east-2.amazonaws.com/Cinematic+Background+HD"

            };

            var movieBatch = context.CreateBatchWrite<Movie>();
            movieBatch.AddPutItems(new List<Movie> { movie1 });

            Console.WriteLine("Performing batch write in SingleTableBatchWrite().");
            movieBatch.ExecuteAsync();
        }
        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await DbContext.ScanAsync<Movie>(new List<ScanCondition>()).GetRemainingAsync();
        }

    }
}
