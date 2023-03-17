using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantsFinder.Models
{
  internal class KitchenModel
  {
    public int Id { get; set; }
    public string? Name { get; set; }

    public KitchenModel(int Id, string? Name)
    {
      this.Id = Id;
      this.Name = Name;
    }

    //Override do construtor para aceitar um array de propriedades como parâmetro
    public KitchenModel(params object[] props)
    {
      if (props.Length > 0)
        try
        {
          this.Id = int.Parse(props[0] as dynamic);
        }
        catch (Exception)
        {
          Console.WriteLine("não parseou :(");
        }

      if (props.Length > 1 && props[1] is string)
        this.Name = (string)props[1];
    }

    public override string ToString()
    {
      return $"ID: {Id}, Nome: {Name}";
    }

  }
}
