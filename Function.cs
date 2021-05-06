using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LastAssignmentGet
{
    public class Item
    {
        public string song;
        public string lyrics;
    }
    public class Function
    {
        private static AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        private string tableName = "assignment11";
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<Item> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            string song = "";
            Dictionary<string, string> dict = (Dictionary<string, string>)input.QueryStringParameters;
            dict.TryGetValue(dict.First().Key, out song);
            GetItemResponse res = await client.GetItemAsync(tableName, new Dictionary<string, AttributeValue>
                {
                {dict.First().Key, new AttributeValue { S = song} }
                }
            );
            Document myDoc = Document.FromAttributeMap(res.Item);
            Item myItem = JsonConvert.DeserializeObject<Item>(myDoc.ToJson());
            return myItem;
        }
    }
}