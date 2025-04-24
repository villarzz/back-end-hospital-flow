using System.Text.Json.Serialization;

namespace hospital_flow.Models
{
    public class Internacao
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string DataInicio { get; set; } = default!;
        public string? DataFim { get; set; }
        [JsonIgnore]
        public int Atendimento { get; set; }
        public int PacienteId { get; set; }
        public int AcomodacaoId { get; set; }
        public int StatusInternacaoId { get; set; }
    }
}
