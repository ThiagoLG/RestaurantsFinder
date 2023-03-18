using RestaurantsFinder.Models;
using System.Runtime.CompilerServices;

class RestaurantFinder
{
  private static readonly string kitchensPath = "./Files/kitchens.csv";
  private static readonly string restaurantsPath = "./Files/restaurants.csv";

  public static void Main(string[] args)
  {
    /*- cria os file readers -*/
    var kitchensReader = new StreamReader(File.OpenRead(kitchensPath));
    var restaurantsReader = new StreamReader(File.OpenRead(restaurantsPath));

    /*- envia os file readers e solicita a leitura dos dados das planilhas -*/
    var kitchenItems = getKitchens(kitchensReader);
    var restaurantsItems = getRestaurants(restaurantsReader);

    /*- indexa os dados de cozinhas, para associar dentro do restaurante -*/
    var kitchensById = new Dictionary<int, KitchenModel>();
    foreach (var kitchenItem in kitchenItems)
      kitchensById.Add(kitchenItem.Id, kitchenItem);

    /*- Associa a cozinha dentro do item de restaurante -*/
    foreach (var restaurantItem in restaurantsItems)
      restaurantItem.Kitchen = kitchensById[restaurantItem.KitchenId ?? 0];

    /*- Recebe o input do console -*/
    string? inputName = getInputData("text", "Informe o termo relacionado ao nome do restaurante: ", false, 200);
    int? inputRating = getInputData("number", "Informe a avaliação mínima do restaurante: ", false, 5);
    int? inputDistance = getInputData("number", "Informe a distância máxima do restaurante: ", false, 10);
    int? inputPrice = getInputData("number", "Informe o preço máximo do restaurante: ", false, 1000);
    string? inputKitchen = getInputData("text", "Informe o tipo de cozinha do restaurante: ", false, 200);

    /*- Realiza o filtro do conteúdo -*/
    IEnumerable<RestaurantModel> finalResults = restaurantsItems.Where(item =>
    {
      if (inputName?.Length > 0 && (item.Name == null || !item.Name.Contains(inputName))) return false;
      if (inputRating != null && (item.Rating == null || item.Rating < inputRating)) return false;
      if (inputDistance != null && (item.Distance == null || item.Distance > inputDistance)) return false;
      if (inputPrice != null && (item.Price == null || item.Price > inputPrice)) return false;
      if (inputKitchen?.Length > 0 && (item.Kitchen == null || item.Kitchen.Name == null || item.Kitchen.Name.Contains(inputKitchen))) return false;

      return true;
    });

    /*- Ordena os resultados -*/
    IEnumerable<RestaurantModel> orderedResults = finalResults
                                                    .OrderBy(r => r.Distance)
                                                    .ThenByDescending(r => r.Rating)
                                                    .ThenBy(r => r.Price);

    /*- Exibe os resultados em tela -*/
    Console.WriteLine("Resultados da busca: ");
    foreach (RestaurantModel restaurant in orderedResults.Take(5))
    {
      Console.WriteLine(restaurant.ToString());
    }

  }

  private static List<KitchenModel> getKitchens(StreamReader fileReader)
  {
    List<KitchenModel> kitchenList = new List<KitchenModel>();

    if (fileReader == null) return kitchenList;

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

    if (fileReader == null) return restaurantsList;

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
        Console.WriteLine("Ocorreu um erro na execução. O item de 'restaurante' recebido não corresponde ao esperado");
        Console.Error.WriteLine(e);
      }
    }

    return restaurantsList;
  }

  private static dynamic? getInputData(string type, string inputAskmessage, Boolean requested, int max)
  {
    Console.Write(inputAskmessage);
    var inputText = Console.ReadLine();

    if (inputText != null && ((requested == true && inputText?.Trim().Length > 0) || requested == false))
    {
      if (type == "number")
      {
        try
        {
          var parsedInput = int.Parse(inputText as dynamic);

          if (parsedInput > max) Console.WriteLine("Valor numérico não pode ser maior que " + max + ".");
          else return parsedInput;

        }
        catch (Exception)
        {
          if (requested == true) Console.WriteLine("Valor numérico inválido.");
          else return null;
        }
      }
      else if (type == "text")
      {
        if (inputText?.Length > max) Console.WriteLine("O tamanho do termo pesquisado não pode exceder " + max + " caracteres.");
        else return inputText ?? "";
      }
    }
    else
    {
      Console.WriteLine("Valor inserido inválido.");
    }

    return getInputData(type, inputAskmessage, requested, max);
  }

}


/** Dúvidas **
 
 • Quando usar variáveis como var e quando usar variáveis como prop dentro dos métodos
 • Uso do static na função (quando usar ou não)
 • Quando usar modelo e quando usar interface
 • No modelo, eu devo usar propriedades com letra maiúscula?
 • Quando usar array e quando usar List (qual a diferença)
 • É possível omitir parâmetro na chamada da função quando for opcional?
 • Como definir o cabeçalho de uma função para retornar somente int ou string, sem usar dynamic

 */