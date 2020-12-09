namespace ApiProvaSalutem.DTO
{
    //classe Data Transfer Object
    public class VendedorDTO
    {
        public string Id { get; set; }
        public long IdVendedor { get; set; }
        public string Cpf { get; set; }
        public string NomeVendedor { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}