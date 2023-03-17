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
    string inputName = getInputData("text", "Informe o termo relacionado ao nome do restaurante: ", false);
    int inputRating = getInputData("number", "Informe a avaliação mínima do restaurante: ", false);
    int inputDistance = getInputData("number", "Informe a distância máxima do restaurante: ", false);
    int inputPrice = getInputData("number", "Informe o preço máximo do restaurante: ", false);
    string inputKitchen = getInputData("text", "Informe o tipo de cozinha do restaurante: ", false);

    Console.WriteLine("input vazio: " + inputRating);

    /*- Realiza o filtro do conteúdo -*/
    //IEnumerable<RestaurantModel> finalResults = restaurantsItems.Where(item =>
    //{
    //  if (inputName.Length > 0 && !item.Name.Contains(inputName)) return false;
    //  if (inputRating > 0)

    //    return true;
    //});

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

  private static dynamic getInputData(string type, string inputAskmessage, Boolean? requested)
  {
    Console.Write(inputAskmessage);
    var inputText = Console.ReadLine();

    if (
      inputText != null &&
      (
        (requested == true && inputText?.Trim().Length > 0) ||
        requested == false)
      )
    {
      if (type == "number")
        try
        {
          var parsedInput = int.Parse(inputText as dynamic);

          if (parsedInput > 9999) Console.WriteLine("Valor numérico não pode ser maior que 9999.");
          else return parsedInput;

        }
        catch (Exception e)
        {
          if (requested == true) Console.WriteLine("Valor numérico inválido.");
          else return null;
        }
      else if (type == "text")
        return inputText ?? "";
    }
    else
    {
      Console.WriteLine("Valor inserido inválido.");
    }

    return getInputData(type, inputAskmessage, requested);
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