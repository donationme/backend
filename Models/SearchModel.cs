using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

public class SearchModel<T>
    {
        [Required]
        public T[] Results { get; set; }

    }