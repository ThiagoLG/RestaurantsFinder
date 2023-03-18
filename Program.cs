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
    var kitchenItems = getListData<KitchenModel>(kitchensReader);
    var restaurantsItems = getListData<RestaurantModel>(restaurantsReader);

    /*- indexa os dados de cozinhas, para associar dentro do restaurante -*/
    var kitchensById = new Dictionary<int, KitchenModel>();
    foreach (var kitchenItem in kitchenItems)
      kitchensById.Add(kitchenItem.Id, kitchenItem);

    /*- Associa a cozinha dentro do item de restaurante -*/
    foreach (var restaurantItem in restaurantsItems)
      restaurantItem.Kitchen = kitchensById[restaurantItem.KitchenId ?? 0];

    /*- Recebe o input do console -*/
    printColoredMessage("***** Busca de restaurantes ****", ConsoleColor.DarkGreen);
    string? inputName = getInputData("text", "Informe o termo relacionado ao nome do restaurante: ", false, 200);
    int? inputRating = getInputData("number", "Informe a avaliação mínima do restaurante: ", false, 5);
    int? inputDistance = getInputData("number", "Informe a distância máxima do restaurante: ", false, 10);
    int? inputPrice = getInputData("number", "Informe o preço máximo do restaurante: ", false, 1000);
    string? inputKitchen = getInputData("text", "Informe o tipo de cozinha do restaurante: ", false, 200);

    /*- Realiza o filtro do conteúdo -*/
    IEnumerable<RestaurantModel> finalResults = restaurantsItems.Where(item =>
    {
      if (inputName?.Length > 0 && (item.Name == null || !item.Name.ToLower().Contains(inputName))) return false;
      if (inputDistance != null && (item.Distance == null || item.Distance > inputDistance)) return false;
      if (inputRating != null && (item.Rating == null || item.Rating < inputRating)) return false;
      if (inputPrice != null && (item.Price == null || item.Price > inputPrice)) return false;
      if (inputKitchen?.Length > 0 && (item.Kitchen == null || item.Kitchen.Name == null || !item.Kitchen.Name.ToLower().Contains(inputKitchen))) return false;

      return true;
    });

    /*- Ordena os resultados -*/
    IEnumerable<RestaurantModel> orderedResults = finalResults
                                                    .OrderBy(r => r.Distance)
                                                    .ThenByDescending(r => r.Rating)
                                                    .ThenBy(r => r.Price);

    /*- Exibe os resultados em tela -*/
    printColoredMessage("\n------------------------- RESULTADOS DA BUSCA -------------------------\n", ConsoleColor.DarkGreen);
    if (orderedResults.Count() > 0)
      printResultsAsTable(orderedResults);
    else
      printColoredMessage("Nenhum restaurante foi localizado com os filtros informados.", ConsoleColor.DarkYellow);

  }

  private static dynamic? getInputData(string type, string inputAskmessage, Boolean requested, int max)
  {

    printColoredMessage(inputAskmessage, ConsoleColor.DarkYellow, true);
    var inputText = Console.ReadLine();

    if (inputText != null && ((requested == true && inputText?.Trim().Length > 0) || requested == false))
    {
      if (type == "number")
      {
        try
        {
          var parsedInput = int.Parse(inputText as dynamic);

          if (parsedInput > max) printColoredMessage($"Valor numérico não pode ser maior que {max}.", ConsoleColor.DarkRed);
          else return parsedInput;

        }
        catch (Exception)
        {
          if (requested == true) printColoredMessage("Valor numérico inválido.", ConsoleColor.DarkRed);
          else return null;
        }
      }
      else if (type == "text")
      {
        if (inputText?.Length > max) printColoredMessage($"O tamanho do termo pesquisado não pode exceder {max} caracteres.", ConsoleColor.DarkRed);
        else return inputText is string ? inputText?.ToLower() : "";
      }
    }
    else
    {
      printColoredMessage("Valor inserido inválido.", ConsoleColor.DarkRed);
    }

    return getInputData(type, inputAskmessage, requested, max);
  }

  private static void printColoredMessage(string message, ConsoleColor color, bool inline = false)
  {
    Console.ForegroundColor = color;
    if (inline == true) Console.Write(message);
    else Console.WriteLine(message);
    Console.ResetColor();
  }

  private static void printResultsAsTable(IEnumerable<RestaurantModel> kitchensToPrint)
  {
    int index = 1;
    printColoredMessage("-------------------------------------------------------------------------------------------------------------------", ConsoleColor.DarkCyan);
    printColoredMessage("| #   | Nome do restaurante                      | Distância (km) | Avaliação (*) | Preço (R$) | Tipo de Cozinha  |", ConsoleColor.DarkCyan);
    printColoredMessage("-------------------------------------------------------------------------------------------------------------------", ConsoleColor.DarkCyan);

    foreach (RestaurantModel restaurant in kitchensToPrint.Take(5))
    {
      Console.Write($"| {index}".PadRight(5, ' '), ConsoleColor.DarkBlue, true);
      Console.WriteLine($" | {restaurant.ToTableString()}");
      index++;
    }
    printColoredMessage("-------------------------------------------------------------------------------------------------------------------", ConsoleColor.DarkCyan);
    printColoredMessage("\n-----------------------------------------------------------------------\n", ConsoleColor.DarkGreen);

  }

  private static List<T> getListData<T>(StreamReader fileReader)
  {
    List<T> dataList = new List<T>();

    if (fileReader == null) return dataList;

    var line = fileReader?.ReadLine(); //Atribui já na criação da variável para já tirar o cabeçalho

    while (!string.IsNullOrEmpty(line = fileReader?.ReadLine()))
    {
      var dataProps = line.Split(',');

      try
      {
        dynamic? dataObj = null;

        if (typeof(T) == typeof(KitchenModel))
          dataObj = new KitchenModel(dataProps);
        else if (typeof(T) == typeof(RestaurantModel))
          dataObj = new RestaurantModel(dataProps);

        if (dataObj != null) dataList.Add(dataObj);

      }
      catch (Exception e)
      {
        Console.WriteLine("Ocorreu um erro na execução. O item de 'cozinha' recebido não corresponde ao esperado");
        Console.Error.WriteLine(e);
      }
    }

    return dataList;
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