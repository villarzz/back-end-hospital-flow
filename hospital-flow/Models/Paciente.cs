public class Paciente
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string DataNascimento { get; set; }
    public string Cpf { get; set; }
    public string? Convenio { get; set; } // Agora é opcional (nullable)
}
