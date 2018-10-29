using Newtonsoft.Json;

public class SearchModel<T>
    {
        [JsonProperty("results")]
        public T[] Results { get; set; }
        [JsonProperty("areResults")]
        public bool AreResults { get; set; }
    }