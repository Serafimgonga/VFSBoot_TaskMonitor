
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
namespace vfsboot;

class Program
{
    static async Task Main(string[] args)
    {

        Console.WriteLine("Iniciando envio de requisições paralelas...");

        HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://api.visasbot.com/api/visa/86")
        };
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        while (true)
        {
            var tasks = new Task[100];

            for (int i = 0; i < tasks.Length; i++)
            {
                int taskIndex = i; // Captura correta do índice
                tasks[i] = Task.Run(async () =>
                {
                    try
                    {
                        using (HttpResponseMessage response = await client.GetAsync(""))
                        {
                            string data = await response.Content.ReadAsStringAsync();

                            if (response.IsSuccessStatusCode)
                            {
                                if (!data.Contains("closed"))
                                {
                                    Console.Beep(); // ou alguma outra ação
                                    Console.WriteLine($"[T{taskIndex}] 🟢 ABERTO!");
                                }
                                else
                                {
                                    Console.WriteLine($"[T{taskIndex}] 🔴 Fechado.");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"[T{taskIndex}] ⚠ Erro: {response.StatusCode}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[T{taskIndex}] ❌ Exceção: {ex.Message}");
                    }
                });
            }

            await Task.WhenAll(tasks); // Espera todas as requisições completarem
            await Task.Delay(2000); // Espera 2 segundos antes de repetir
        }

    }
}
