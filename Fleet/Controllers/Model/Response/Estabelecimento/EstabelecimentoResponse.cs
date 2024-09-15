using Fleet.Enums;

namespace Fleet.Controllers.Model.Response.Estabelecimento
{
    public class EstabelecimentoResponse
    {
        public TipoEstabelecimentoEnum Tipo { get; set; }  //Tipos Posto , clientes ou oficinas
        public string Cnpj { get; set; } = string.Empty; //requisito
        public string Razao { get; set; } = string.Empty;
        public string Fantasia { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public string Rua { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
