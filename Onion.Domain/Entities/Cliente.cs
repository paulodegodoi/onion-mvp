namespace Onion.Domain.Entities;

public sealed class Cliente : Entity
{
    public string Documento { get; private set; } = string.Empty;
    // public string Documento { get; private set; } = string.Empty;
    public string RazaoSocial { get; private set; } = string.Empty;
    public List<Pedido> Pedidos { get; set; } = new();
}