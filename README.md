# 🚀 VFSBoot Task Monitor

> Um projeto educativo para demonstrar o poder das `Task`s assíncronas em C#, com foco em requisições paralelas — úteis para monitoramento de serviços online, mas que também mostram como a má utilização dessa técnica pode ser perigosa.

---

## 📌 Visão geral

Este projeto realiza **requisições HTTP em massa e de forma paralela** usando `Task` em C#. Ele simula um sistema de monitoramento contínuo a uma API (neste caso, de agendamento de vistos), verificando se o status retornado indica **"Aberto"** ou **"Fechado"**.

---

## 🎯 Intenção do projeto

Este projeto foi criado com **duas finalidades principais**:

1. **Demonstrar o poder e eficiência das `Task`s assíncronas em C#.**
2. **Alertar sobre os riscos de uso indevido, que podem simular um ataque DDoS.**

---

## 🧠 Conceitos utilizados

### ✅ `Task` e paralelismo

Usamos `Task.Run` para criar múltiplas tarefas que rodam em paralelo, permitindo executar várias requisições simultaneamente sem bloquear a aplicação.

### ✅ `await Task.WhenAll(...)`

Garante que todas as requisições paralelas sejam aguardadas antes de iniciar um novo ciclo.

### ✅ Monitoramento contínuo

Com um loop infinito e atraso de 2 segundos entre ciclos, o sistema nunca para de monitorar a API.

---

## 📍 Estrutura resumida do código

```csharp
while (true)
{
    var tasks = new Task[100];
    for (int i = 0; i < tasks.Length; i++)
    {
        int taskIndex = i;
        tasks[i] = Task.Run(async () =>
        {
            using var response = await client.GetAsync("");
            string data = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                if (!data.Contains("closed"))
                    Console.WriteLine($"[T{taskIndex}] 🟢 ABERTO!");
                else
                    Console.WriteLine($"[T{taskIndex}] 🔴 Fechado.");
            }
        });
    }

    await Task.WhenAll(tasks);
    await Task.Delay(2000);
}
