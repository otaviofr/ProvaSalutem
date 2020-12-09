using ApiProvaSalutem.Model;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiProvaSalutem.Infraestructure.Repositories
{
    // Aqui é onde ficam todos métodos que tem ligação com o banco de dados, seja para busca, inserção ou remoção
    public class VendedorRepository : IVendedorRepository // define herança
    {
        private readonly MongoContext _mongoContext; // declara váriavel mongo

        public VendedorRepository() // construtor VendedorRepository
        {
            _mongoContext = new MongoContext(); // instancia MongoContext
        }

        //método responsável por salvar dados no banco
        public async Task<bool> Save(Vendedor obj) // recebe um objeto do tipo Vendedor
        {
            await _mongoContext.DSalutem_Vendedor.InsertOneAsync(obj); // aguarda inserção no banco 
            return true;
        }

        //método para busca de um unico vendedor no banco, este é usado somente para realizar a busca para atualização, pos isso não aparece no controller
        public Vendedor GetUnique(long id) // recebe um id
        {
            return _mongoContext.DSalutem_Vendedor.Find(x => x.IdVendedor.Equals(id)).FirstOrDefault(); // retorna o vendedor cujo id seja igual ao informado pelo usuário
        }

        //método que atualiza o vendedor
        public async Task<bool> Update(Vendedor obj) // recebe um objeto do tipo Vendedor
        {
            //garante que o vendedor a ser atualizado seja o mesmo informado pelo usuário
            var filter = Builders<Vendedor>.Filter.Eq(x => x.IdVendedor, obj.IdVendedor);
            //recebe o vendedor antigo, e atribui os dados novos a ele, caso não seja alterado algum campo, é mantido o último valor
            var update = Builders<Vendedor>.Update.
                Set(x => x.IdVendedor, obj.IdVendedor).
                Set(x => x.Cpf, obj.Cpf).
                Set(x => x.NomeVendedor, obj.NomeVendedor).
                Set(x => x.Latitude, obj.Latitude).
                Set(x => x.Longitude, obj.Longitude);

            var result = await _mongoContext.DSalutem_Vendedor.UpdateOneAsync(filter, update); // realiza a atualização no banco

            //condição para verificar se teve algum vendedor alterado
            if (result.ModifiedCount > 0)
            {
                return true;
            }
            return false;
        }

        //método que busca um unico vendedor
        public IEnumerable<Vendedor> GetById(long id) // recebe um id
        {
            var vendedor = _mongoContext.DSalutem_Vendedor.Find(x => x.IdVendedor.Equals(id)).ToList(); // retorna o vendedor cujo id seja igual ao informado pelo usuário

            return vendedor; // retorna dados do vendedor ao usuário
        }

        //método que deleta um vendedor
        public void Delete(long idVendedor) // recebe um id
        {
            _mongoContext.DSalutem_Vendedor.DeleteOne(x => x.IdVendedor == idVendedor); // encontra no banco o vendedor cujo id informado seja igual e o deleta
        }

        //método que busca todos vendedores do banco
        public IEnumerable<Vendedor> GetAll(int skip = 0, int limit = 50) // recebe valores para paginação
        {
            return _mongoContext.DSalutem_Vendedor.Find(x => true).Skip(skip).Limit(limit).ToList(); // busca e retorna todos vendedores no banco de dados
        }
    }
}