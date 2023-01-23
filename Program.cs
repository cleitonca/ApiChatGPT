using Newtonsoft.Json;
using System.Text;


if (args.Length > 0)
{
    HttpClient httpClient = new HttpClient();

    httpClient.DefaultRequestHeaders.Add("authorization", "Bearer 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");

    var content = new StringContent("{\"model\": \"text-davinci-001\", \"prompt\": \"" + args[0] + "\",\"temperature\": 1,\"max_tokens\": 100}",
          Encoding.UTF8, "application/json");

    HttpResponseMessage response = await httpClient.PostAsync("https://api.openai.com/v1/completions", content);

    string responseString = await response.Content.ReadAsStringAsync();

    try
    {
        var dyData = JsonConvert.DeserializeObject<dynamic>(responseString);

        string guess = GuessCommand(dyData!.choices[0].text);

        Console.ForegroundColor = ConsoleColor.Green;
        
        Console.WriteLine($"---> Meu palpite no prompt de comando é: {guess}");
        
        Console.ResetColor();

    }
    catch (Exception ex)
    {
        Console.WriteLine($"---> Não foi possível deserializar o JSON: {ex.Message}");
}

    //Console.WriteLine(responseString);

}

else
{
    Console.WriteLine("--> Você precisa fornecer alguma entrada");
}

static string GuessCommand(string raw)
{
    Console.WriteLine("---> Texto de retorno da API GPT-3:");

    
    Console.ForegroundColor = ConsoleColor.Yellow;
    
    Console.WriteLine(raw);

    var lastIndex = raw.LastIndexOf('\n');

    string guess = raw.Substring(lastIndex + 1);

    Console.ResetColor();

    TextCopy.ClipboardService.SetText(guess);

    return guess;
}