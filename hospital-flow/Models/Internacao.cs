namespace hospital_flow.Models
{
    public class Internacao
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int AcomodacaoId { get; set; }
        public int StatusInternacaoId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFinal { get; set; }
    }
}
