using RestaurantsFinder.Models;
using System.Runtime.CompilerServices;

class RestaurantFinder
{
  private static readonly string kitchensPath = "./Files/kitchens.csv";
  private static readonly string restaurantsPath = "./Files/restaurants.csv";

  public static void Main(string[] args)
  {
    Console.WriteLine("App running");

    var kitchensReader = new StreamReader(File.OpenRead(kitchensPath));
    var restaurantsReader = new StreamReader(File.OpenRead(restaurantsPath));

    var kitchenItems = getKitchens(kitchensReader);
    Console.WriteLine(kitchenItems);

    foreach (var item in kitchenItems)
    {
      Console.WriteLine(item);
    }

    Console.WriteLine("pós execução da função");

  }

  private static List<KitchenModel> getKitchens(StreamReader fileReader)
  {
    List<KitchenModel> kitchenList = new List<KitchenModel>();

    if (fileReader == null)
    {
      return kitchenList;
    }

    var line = fileReader?.ReadLine(); //Atribui já na criação da variável para já tirar o cabeçalho

    while (!string.IsNullOrEmpty(line = fileReader?.ReadLine()))
    {
      var kitchenProps = line.Split(',');

      try
      {
        var kitchen = new KitchenModel(kitchenProps);
        kitchenList.Add(kitchen);
      }
      catch (Exception e)
      {
        Console.WriteLine("Ocorreu um erro na execução. O item de 'cozinha' recebido não corresponde ao esperado");
        Console.Error.WriteLine(e);
      }
    }

    return kitchenList;
  }


  private static List<RestaurantModel> getRestaurants(StreamReader fileReader)
  {
    List<RestaurantModel> restaurantsList = new List<RestaurantModel>();

    if (fileReader == null)
    {
      return restaurantsList;
    }

    var line = fileReader?.ReadLine(); //Atribui já na criação da variável para já tirar o cabeçalho

    while (!string.IsNullOrEmpty(line = fileReader?.ReadLine()))
    {
      var restaurantProps = line.Split(',');

      try
      {
        var restaurant = new RestaurantModel(restaurantProps);
        restaurantsList.Add(restaurant);
      }
      catch (Exception e)
      {
        Console.WriteLine("Ocorreu um erro na execução. O item de 'cozinha' recebido não corresponde ao esperado");
        Console.Error.WriteLine(e);
      }
    }

    return restaurantsList;
  }


}


/** Dúvidas **
 
 • Quando usar variáveis como var e quando usar variáveis como prop dentro dos métodos
 • Uso do static na função (quando usar ou não)
 • Quando usar modelo e quando usar interface
 • No modelo, eu devo usar propriedades com letra maiúscula?
 • Quando usar array e quando usar List (qual a diferença)
 • 

 */