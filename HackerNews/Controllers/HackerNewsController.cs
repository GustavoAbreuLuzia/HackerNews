using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data;
using HackerNews.Settings;
using System.Net.Http;
using HackerNews.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace HackerNews_Gustavo.Controllers
{
    [Route("api/[controller]")]
    //[Authorize] JWT Authentication to be finished on further releases
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    [ApiController]
    public class HackerNewsController : ControllerBase
    {
        private readonly IOptions<HackerNewsAPI> _settings;

        public HackerNewsController(IOptions<HackerNewsAPI> settings)
        {
            _settings = settings;
        }

        // GET: api/HackerNews
        // Search for best stories 
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<int> storiesIds;
                List<Story> stories = new List<Story>();
                List<StoryHackerNews> storiesHackerNews = new List<StoryHackerNews>();

                // Get the id list of the best stories 
                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage res = await client.GetAsync(_settings.Value.bestStories))
                using (HttpContent content = res.Content)
                {
                    string storiesJson = await content.ReadAsStringAsync();

                    // Take the defined quantity of the ebst stories, the api already return it ordened by score
                    storiesIds = JsonConvert.DeserializeObject<List<int>>(storiesJson).Take(_settings.Value.storiesQuantity).ToList();
                }

                // Get details for each selected best story
                for(int i = 0; i < storiesIds.Count(); i++){
                    using (HttpClient client = new HttpClient())
                    using (HttpResponseMessage res = await client.GetAsync(string.Format(_settings.Value.storyDetails, storiesIds[i])))
                    using (HttpContent content = res.Content)
                    {
                        string storyDetailJson = await content.ReadAsStringAsync();
                        storiesHackerNews.Add(JsonConvert.DeserializeObject<StoryHackerNews>(storyDetailJson));
                    }
                }

                // Convert the selected best stories to the desired format
                stories = storiesHackerNews.Select(s => new Story {
                    title = s.title,
                    uri = s.url,
                    postedBy = s.by,
                    // The time given by the api is a Unix Time, so it can be calculated from 01/01/1970 00:00:00 UTC
                    // DateTimeKind is Local because the wanted format must have the offset from UTC for the current local time
                    time = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local).AddSeconds(s.time), 
                    score = s.score,
                    commentCount = s.descendants
                }).ToList();

                // Return the list serialized as a json object with a status 200.
                return Ok(JsonConvert.SerializeObject(stories));
            }
            catch (Exception ex)
            {
                // Returns the exception message with the status code 400
                return BadRequest(ex.Message);
            }
        }
    }
}
