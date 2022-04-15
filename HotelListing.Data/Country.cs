using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListing.Data
{
    public class Country
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        [ForeignKey(nameof(Hotels))]
        // по ходу треба зробити поле-посилання для зовнішнього ключа у відповідь як в Hotel
        public virtual IList<Hotel> Hotels { get; set; }
    }
}