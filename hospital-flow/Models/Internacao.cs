using System.Text.Json.Serialization;

namespace hospital_flow.Models
{
    public class Internacao
    {
        public int Id { get; set; }
        public string DataInicio { get; set; } = default!;
        public string? DataFim { get; set; }
        [JsonIgnore]
        public int Atendimento { get; set; }
        public int PacienteId { get; set; }
        public int AcomodacaoId { get; set; }
        public int StatusInternacaoId { get; set; }
    }
    
    public class InternacaoFiltro
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string DataInicio { get; set; } = default!;
        public string? DataFim { get; set; }
        public string? NomePaciente { get; set; }
        public string? Convenio { get; set; }
        public int Atendimento { get; set; }
        public int PacienteId { get; set; }
        public int AcomodacaoId { get; set; }
        public int StatusInternacaoId { get; set; }
        public string? StatusInternacaoDescricao { get; set; }
        public string? AcomodacaoDescricao { get; set; }
    }
}
