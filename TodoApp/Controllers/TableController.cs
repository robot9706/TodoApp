using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.Bson;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TodoApp.Data.Collections;
using TodoApp.Data.Model;

namespace TodoApp.Controllers
{
    public class CreateTableData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class TableReference
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        public TableReference(Table table)
        {
            Id = table.ID.ToString();
            Name = table.Name;
        }
    }

    [ApiController]
    public class TableController : ControllerBase
    {
        private UserManager<User> userManager;

        public TableController(UserManager<User> usermanager)
        {
            userManager = usermanager;
        }

        [HttpPost("/table/create")]
        public async Task<IActionResult> Create(CreateTableData data)
        {
            if (string.IsNullOrEmpty(data.Name))
            {
                return BadRequest();
            }

            User activeUser = await userManager.GetUserAsync(User);

            Table newTable = new Table();
            newTable.Name = data.Name;
            newTable.Owner = activeUser.ID;
            TableCollection.InsertOne(newTable);

            return Ok(newTable);
        }

        [HttpGet("/table/{tableID}")]
        public async Task<IActionResult> Get([FromRoute] string tableID)
        {
            if (string.IsNullOrEmpty(tableID))
            {
                return BadRequest();
            }

            Table table = TableCollection.FindOneById(new ObjectId(tableID));
            if (table == null)
            {
                return NotFound();
            }

            return Ok(table);
        }

        [HttpGet("/table/all")]
        public async Task<TableReference[]> GetAll()
        {
            User activeUser = await userManager.GetUserAsync(User);

            return TableCollection.FindByUser(activeUser.ID).Select(x => new TableReference(x)).ToArray();
        }

        [HttpGet("/table/{tableID}/delete")]
        public async Task<IActionResult> DeleteTable([FromRoute] string tableID)
        {
            if (string.IsNullOrEmpty(tableID))
            {
                return BadRequest();
            }

            ObjectId table = new ObjectId(tableID);
            if (!TableCollection.DeleteOneById(table))
            {
                return BadRequest();
            }

            CardListCollection.DeleteByTable(table);

            return Ok();
        }

    }
}
