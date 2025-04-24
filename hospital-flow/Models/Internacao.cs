namespace hospital_flow.Models
{
    public class Internacao
    {
        public int Id { get; set; }
        public string DataInicio { get; set; } = null!;
        public string? DataFim { get; set; }
        public int PacienteId { get; set; }
        public int AcomodacaoId { get; set; }
        public int StatusInternacaoId { get; set; }
    }

}
