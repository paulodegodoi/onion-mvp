namespace Onion.Domain.Entities;

public sealed class Cliente
{
    public string Documento { get; private set; } = string.Empty;
    public string RazaoSocial { get; private set; } = string.Empty;
}