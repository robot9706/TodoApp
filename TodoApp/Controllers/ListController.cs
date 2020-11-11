using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TodoApp.Data.Collections;
using TodoApp.Data.Model;

namespace TodoApp.Controllers
{
    public class CreateListData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class CreateCardData
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    [ApiController]
    public class ListController : ControllerBase
    {
        private UserManager<User> userManager;

        public ListController(UserManager<User> usermanager)
        {
            userManager = usermanager;
        }

        [HttpPost("/table/{tableID}/createList")]
        public async Task<IActionResult> CreateList([FromRoute] string tableID, [FromBody] CreateListData data)
        {
            if (string.IsNullOrEmpty(data.Name))
            {
                return BadRequest();
            }

            User activeUser = await userManager.GetUserAsync(User);
            Table table = TableCollection.FindByUserAndId(activeUser.ID, new ObjectId(tableID));
            if (table == null)
            {
                return NotFound();
            }

            CardList newList = new CardList();
            newList.Name = data.Name;
            newList.Table = table.ID;
            newList.Content = new List<Card>();
            CardListCollection.InsertOne(newList);

            return Ok(newList);
        }

        [HttpGet("/table/{tableID}/all")]
        public List<CardList> GetLists([FromRoute] string tableID)
        {
            return CardListCollection.FindAllInTable(new ObjectId(tableID));
        }

        [HttpPost("/table/{tableID}/{listID}/create")]
        public async Task<IActionResult> CreateCard([FromRoute] string tableID, [FromRoute] string listID, [FromBody] CreateCardData data)
        {
            if (string.IsNullOrEmpty(data.Title))
            {
                return BadRequest();
            }

            Card newCard = new Card()
            {
                Title = data.Title,
                Description = data.Description
            };
            if (!CardListCollection.AddCardToTable(new ObjectId(tableID), new ObjectId(listID), newCard))
            {
                return BadRequest();
            }

            return Ok(CardListCollection.FindOneList(new ObjectId(tableID), new ObjectId(listID)));
        }

        [HttpGet("/table/{tableID}/{listID}/delete")]
        public async Task<IActionResult> DeleteList([FromRoute] string tableID, [FromRoute] string listID)
        {
            if (!CardListCollection.DeleteByTableAndId(new ObjectId(tableID), new ObjectId(listID)))
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet("/table/{tableID}/{listID}/{cardIndex}/delete")]
        public async Task<IActionResult> DeleteCard([FromRoute] string tableID, [FromRoute] string listID, [FromRoute] int cardIndex)
        {
            if (cardIndex < 0 || cardIndex < 0)
            {
                return BadRequest();
            }

            CardList fromList = CardListCollection.FindListByTableAndId(new ObjectId(tableID), new ObjectId(listID));
            if (fromList == null || fromList.Content.Count <= cardIndex)
            {
                return BadRequest();
            }

            fromList.Content.RemoveAt(cardIndex);

            if (!CardListCollection.UpdateContent(fromList))
            {
                return BadRequest();
            }

            return Ok(CardListCollection.FindOneList(new ObjectId(tableID), new ObjectId(listID)));
        }

        [HttpPost("/table/{tableID}/{listID}/{cardIndex}/edit")]
        public async Task<IActionResult> CreateCard([FromRoute] string tableID, [FromRoute] string listID, [FromRoute] int cardIndex, [FromBody] CreateCardData data)
        {
            if (string.IsNullOrEmpty(data.Title))
            {
                return BadRequest();
            }

            CardList list = CardListCollection.FindListByTableAndId(new ObjectId(tableID), new ObjectId(listID));
            list.Content[cardIndex] = new Card()
            {
                Title = data.Title,
                Description = data.Description
            };

            if (!CardListCollection.UpdateContent(list))
            {
                return BadRequest();
            }

            return Ok(list);
        }

        [HttpGet("/table/{tableID}/{listID}/move")] //?toList=&cardIndex=&newCardIndex=
        public async Task<IActionResult> MoveCard([FromRoute] string tableID, [FromRoute] string listID, [FromQuery(Name = "toList")] string toListID, [FromQuery] int cardIndex, [FromQuery] int newCardIndex)
        {
            ObjectId table = new ObjectId(tableID);

            if (cardIndex < 0 || newCardIndex < 0)
            {
                return BadRequest();
            }

            CardList fromList = CardListCollection.FindListByTableAndId(table, new ObjectId(listID));
            if (fromList == null || fromList.Content.Count <= cardIndex)
            {
                return BadRequest();
            }

            CardList toList = CardListCollection.FindListByTableAndId(table, new ObjectId(toListID));
            if (toList == null)
            {
                return BadRequest();
            }

            Card card = fromList.Content[cardIndex];
            fromList.Content.RemoveAt(cardIndex);

            toList.Content.Insert(newCardIndex, card);

            if (!CardListCollection.UpdateContent(fromList))
            {
                return BadRequest();
            }
            if (!CardListCollection.UpdateContent(toList))
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
