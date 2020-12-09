using ApiProvaSalutem.Model;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiProvaSalutem.Infraestructure.Repositories
{
    // Aqui é onde ficam todos métodos que tem ligação com o banco de dados, seja para busca, inserção ou remoção
    public class ClienteRepository : IClienteRepository // define herança
    {
        private readonly MongoContext _mongoContext; // declara váriavel mongo

        public ClienteRepository() // construtor ClienteRepository
        {
            _mongoContext = new MongoContext(); // instancia MongoContext
        }

        //método responsável por salvar dados no banco
        public async Task<bool> Save(Cliente obj) // recebe um objeto do tipo Cliente
        {
            await _mongoContext.DSalutem_Cliente.InsertOneAsync(obj); // aguarda inserção no banco 
            return true;
        }

        //método para busca de um unico cliente no banco, este é usado somente para realizar a busca para atualização, pos isso não aparece no controller
        public Cliente GetUnique(long id) // recebe um id
        {
            return _mongoContext.DSalutem_Cliente.Find(x => x.IdCliente.Equals(id)).FirstOrDefault(); // retorna o cliente cujo id seja igual ao informado pelo usuário
        }

        //método que atualiza o cliente
        public async Task<bool> Update(Cliente obj) // recebe um objeto do tipo Cliente
        {
            //garante que o cliente a ser atualizado seja o mesmo informado pelo usuário
            var filter = Builders<Cliente>.Filter.Eq(x => x.IdCliente, obj.IdCliente);
            //recebe o cliente antigo, e atribui os dados novos a ele, caso não seja alterado algum campo, é mantido o último valor
            var update = Builders<Cliente>.Update.
                Set(x => x.IdCliente, obj.IdCliente).
                Set(x => x.Cnpj, obj.Cnpj).
                Set(x => x.RazaoSocial, obj.RazaoSocial).
                Set(x => x.Latitude, obj.Latitude).
                Set(x => x.Longitude, obj.Longitude);

            var result = await _mongoContext.DSalutem_Cliente.UpdateOneAsync(filter, update); // realiza a atualização no banco

            //condição para verificar se teve algum cliente alterado
            if (result.ModifiedCount > 0)
            {
                return true;
            }
            return false;
        }

        //método que busca um unico cliente
        public IEnumerable<Cliente> GetById(long id) // recebe um id
        {
            var cliente = _mongoContext.DSalutem_Cliente.Find(x => x.IdCliente.Equals(id)).ToList(); // retorna o cliente cujo id seja igual ao informado pelo usuário

            return cliente; // retorna dados do cliente ao usuário
        }

        //método que deleta um cliente
        public void Delete(long idCliente) // recebe um id
        {
            _mongoContext.DSalutem_Cliente.DeleteOne(x => x.IdCliente == idCliente); // encontra no banco o cliente cujo id informado seja igual e o deleta
        }

        //método que busca todos clientes do banco
        public IEnumerable<Cliente> GetAll(int skip = 0, int limit = 50) // recebe valores para paginação
        {
            return _mongoContext.DSalutem_Cliente.Find(x => true).Skip(skip).Limit(limit).ToList(); // busca e retorna todos clientes no banco de dados
        }
    }
}