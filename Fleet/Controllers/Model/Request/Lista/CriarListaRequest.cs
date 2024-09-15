namespace Fleet.Controllers.Model.Request.Lista
{
    public class CriarListaRequest
    {
        public string Nome { get; set; } = string.Empty;
        public bool Veiculo { get; set; };
    }
}
