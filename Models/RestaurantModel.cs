﻿using System;
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
    public int? Price { get; set; }
    public int? KitchenId { get; set; }
    public KitchenModel? Kitchen { get; set; }


    public RestaurantModel(string name, int rating, int distance, int price, int kitchenId)
    {
      this.Name = name;
      this.Rating = rating;
      this.Distance = distance;
      this.Price = price;
      this.KitchenId = kitchenId;
    }

    //Override do construtor para aceitar um array de propriedades como parâmetro
    public RestaurantModel(params object[] props)
    {
      if (props.Length > 0 && props[1] is string)
        this.Name = (string)props[0];

      if (props.Length > 0)
        this.Rating = int.Parse(props[1] as dynamic);

      if (props.Length > 0)
        this.Distance = int.Parse(props[2] as dynamic);

      if (props.Length > 0)
        this.Price = int.Parse(props[3] as dynamic);

      if (props.Length > 0)
        this.KitchenId = int.Parse(props[4] as dynamic);
    }

    public string ToTableString()
    {                                                                                      
      string stringToPrint = "";

      stringToPrint += $"{Name?.Substring(0, Math.Min(40, Name.Length)).PadRight(40,' ')} | ";
      stringToPrint += $"{Distance?.ToString().PadLeft(14, ' ')} | ";
      stringToPrint += $"{Rating?.ToString().PadLeft(13, ' ')} | ";
      stringToPrint += $"{Price?.ToString().PadLeft(10, ' ')} | ";
      stringToPrint += $"{Kitchen?.Name?.Substring(0, Math.Min(16, Kitchen.Name.Length)).PadRight(16, ' ')} |";

      return  stringToPrint;
    }

    public override string ToString()
    {
      return $"Nome: {Name} | Avaliação: {Rating} | Distância: {Distance} | Preço: {Price} | Cozinha: {Kitchen?.Name}";
    }


    internal void Deconstruct(out RestaurantModel restaurant, out int index)
    {
      throw new NotImplementedException();
    }
  }
}
