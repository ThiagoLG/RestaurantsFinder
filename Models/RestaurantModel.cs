using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantsFinder.Models
{
  internal class RestaurantModel
  {
    public string? Name { get; set; }
    public int? Rating { get; set; }
    public int? Distance { get; set; }
    public int? KitchenId { get; set; }
    public KitchenModel? Kitchen { get; set; }
  }
}
