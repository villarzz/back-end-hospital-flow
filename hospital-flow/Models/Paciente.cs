using System.Text.Json.Serialization;

namespace hospital_flow.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string DataNascimento { get; set; }
        public string Cpf { get; set; }
        public string Convenio { get; set; }
    }

    public class PacienteFiltro
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string DataNascimento { get; set; }
        public string Cpf { get; set; }
        public string? Convenio { get; set; }
        public string? ConvenioDescricao { get; set; }
    }
}
