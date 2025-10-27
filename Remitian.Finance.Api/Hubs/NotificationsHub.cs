using Microsoft.AspNetCore.SignalR;

namespace Remitian.Finance.Api.Hubs;

public class NotificationsHub : Hub
{
    public async Task SendMessageBankAccountUpdated(int bankAccountId, int newBalance)
    {
        await Clients.All.SendAsync("bank-account-updated", bankAccountId, newBalance);
    }

    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    // Método que o CLIENTE pode chamar no SERVIDOR
    // Ex: connection.invoke("EnviarMensagem", "usuario", "olá")
    public async Task EnviarMensagem(string usuario, string mensagem)
    {
        // Método que o SERVIDOR chama no CLIENTE
        // Isso envia a mensagem para TODOS os clientes conectados
        // Ex: connection.on("ReceberMensagem", (usuario, mensagem) => { ... })
        await Clients.All.SendAsync("ReceberMensagem", usuario, mensagem);
    }

    // Você pode adicionar outros métodos
    // Exemplo: Enviar apenas para quem chamou
    public async Task EnviarMensagemPrivada(string mensagem)
    {
        await Clients.Caller.SendAsync("ReceberMensagem", "Servidor", $"Sua mensagem privada foi: {mensagem}");
    }

}