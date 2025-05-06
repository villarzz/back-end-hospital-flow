using ClosedXML.Excel;

namespace hospital_flow.Services
{
    public class RelatoriosService
    {
        private readonly InternacaoService _internacaoService;
        public RelatoriosService(InternacaoService internacaoService)
        {
            _internacaoService = internacaoService;
        }

        public byte[] GerarRelatorioInternacoes(string? atendimento, string? nomePaciente, string? convenio, string? statusInternacao)
        {
            var internacoes = _internacaoService.ObterInternacoes(atendimento, nomePaciente, convenio, statusInternacao);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Relatório de Internações");

                // Título
                worksheet.Cell("A1").Value = "Relatório de Internações";
                worksheet.Range("A1:F1").Merge().Style
                    .Font.SetBold()
                    .Font.FontSize = 16;
                worksheet.Row(1).Height = 25;
                worksheet.Row(1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                // Cabeçalhos
                worksheet.Cell(2, 1).Value = "Data Início";
                worksheet.Cell(2, 2).Value = "Data Fim";
                worksheet.Cell(2, 3).Value = "Atendimento";
                worksheet.Cell(2, 4).Value = "Nome do Paciente";
                worksheet.Cell(2, 5).Value = "Descrição da Acomodação";
                worksheet.Cell(2, 6).Value = "Status da Internação";

                // Estilo dos cabeçalhos
                var headerRange = worksheet.Range("A2:F2");
                headerRange.Style
                    .Font.SetBold()
                    .Fill.SetBackgroundColor(XLColor.LightGray)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Border.OutsideBorder = XLBorderStyleValues.Thin;
                headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                // Dados
                for (int i = 0; i < internacoes.Count; i++)
                {
                    var row = i + 3;
                    var item = internacoes[i];
                    worksheet.Cell(row, 1).Value = item.DataInicio;
                    worksheet.Cell(row, 2).Value = item.DataFim;
                    worksheet.Cell(row, 3).Value = item.Atendimento;
                    worksheet.Cell(row, 4).Value = item.NomePaciente;
                    worksheet.Cell(row, 5).Value = item.AcomodacaoDescricao;
                    worksheet.Cell(row, 6).Value = item.StatusInternacaoDescricao;

                    // Estilo básico das células
                    var dataRange = worksheet.Range(row, 1, row, 6);
                    dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                }

                // Auto ajustar colunas
                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }


    }
}
