using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clicker.Engine.Public
{
    public abstract class AJsonSerializable
    {
        /// <summary>
        ///  Initialize <see cref="JsonSerializerSettings"/> properly
        /// </summary>
        protected static JsonSerializerSettings InitJsonSerializerSettings()
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            // If a member is not present during deserialization, throw an Exception
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Error;
            // Always serialize and deserialize null values
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Include;
            // Add the default value during serialization
            jsonSerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
            // Add indentation for readability
            jsonSerializerSettings.Formatting = Formatting.Indented;

            return jsonSerializerSettings;
        }

    }
}
