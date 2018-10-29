using Newtonsoft.Json;

public class SearchModel<T>
    {
        [JsonProperty("results")]
        public T[] Results { get; set; }

    }