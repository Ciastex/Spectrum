using Newtonsoft.Json;
using System.Collections.Generic;

namespace Spectrum.Resonator.Models
{
    public class Validator
    {
        [JsonProperty("validatorName")]
        public string Name { get; set; }

        [JsonProperty("requiredFiles")]
        public List<string> RequiredFiles { get; set; }
    }
}
