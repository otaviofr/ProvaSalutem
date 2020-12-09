namespace ApiProvaSalutem.ViewModel
{
    //Classe ViewModel para busca do cliente
    public class ClienteViewModel
    {
        public string Id { get; set; }
        public long IdCliente { get; set; }
        public string Cnpj { get; set; }
        public string RazaoSocial { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}