using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Golf.Fields.Shared
{
    [Table("City")]
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string? Name { get; set; }

        public int CountryId { get; set; }


        public virtual Country? Country { get; set; }

        public virtual ICollection<Field>? Fields { get; set; }
    }
}

