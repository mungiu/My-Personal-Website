﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.Services
{
    /// <summary>
    /// Inserting Google Maps API for requesting long/lat coordinates
    /// </summary>
    public class GeoCoordsService
    {
        private ILogger<GeoCoordsService> _logger;
        private IConfigurationRoot _config;

        //ctor
        public GeoCoordsService(ILogger<GeoCoordsService> logger, 
            IConfigurationRoot config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task<GeoCoordsResult> GetCoordsAsync(string name)
        {
            var result = new GeoCoordsResult()
            {
                //setting default values for instantiated obj
                Success = false,
                Message = "Failed to get coordinates"
            };

            var apiKey = _config["Keys:GoogleKey"];
            var encodedName = WebUtility.UrlEncode(name);
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={encodedName}&key={apiKey}";

            //creating an http client
            var client = new HttpClient();
            //calling GetStringAsync on the above client
            var json = await client.GetStringAsync(url);

            // Read out the results
            // Fragile, might need to change if the Bing API changes
            var results = JObject.Parse(json);
            var resources = results["resourceSets"][0]["resources"];
            if (!resources.HasValues)
            {
                result.Message = $"Could not find '{name}' as a location";
            }
            else
            {
                var confidence = (string)resources[0]["confidence"];
                if (confidence != "High")
                {
                    result.Message = $"Could not find a confident match for '{name}' as a location";
                }
                else
                {
                    var coords = resources[0]["geocodePoints"][0]["coordinates"];
                    result.Latitude = (double)coords[0];
                    result.Longitude = (double)coords[1];
                    result.Success = true;
                    result.Message = "Success";
                }
            }

        }
    }
}
