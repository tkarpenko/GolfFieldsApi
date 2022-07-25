using System.ComponentModel.DataAnnotations;

namespace Golf.Fields.Api
{
    public class AuthModel
    {
        [Required]
        public string? Phone { get; set; }
    }
}

