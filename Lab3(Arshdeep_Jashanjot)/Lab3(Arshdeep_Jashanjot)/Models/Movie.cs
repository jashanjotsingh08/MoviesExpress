using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace Lab3_Arshdeep_Jashanjot_.Models
{
    [DynamoDBTable("Movies")]
    public class Movie
    {
        [DynamoDBHashKey]
        public int id { get; set; }
        [DynamoDBRangeKey]
        public string name { get; set; }
        [DynamoDBProperty("url")]
        public string url { get; set; }
        

    }
}
