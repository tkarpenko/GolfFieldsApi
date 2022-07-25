using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Golf.Fields.Shared
{
    [Table("Field")]
    public class Field
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int SizeAcre { get; set; }

        public int CityId { get; set; }


        public virtual City? City { get; set; }

        public virtual ICollection<Reservation>? Reservations { get; set; }
    }
}

