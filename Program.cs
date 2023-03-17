// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


class KitchenFinder
{
  private static readonly string kitchensPath = "./Files/kitchens.csv";
  private static readonly string restaurantsPath = "./Files/restaurants.csv";

  public static void Main(string[] args)
  {

    var kitchensReader = new StreamReader(File.OpenRead(kitchensPath));
    var restaurantsReader = new StreamReader(File.OpenRead(restaurantsPath));

     

  }

}
