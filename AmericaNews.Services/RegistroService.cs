using AmericaNews.Data;
using AmericaNews.Data.Interfaces;
using AmericaNews.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmericaNews.Services
{
    public class RegistroService : IRegistroService
    {
        IRegistroRepository _registroRepository;

        public RegistroService(IRegistroRepository registroRepository)
        {
            _registroRepository = registroRepository;
        }

        public async Task<List<RegistroModel>> GetAll()
        {
            var registros = await _registroRepository.GetAll();

            if (registros == null || !registros.Any())
                throw new KeyNotFoundException("Nenhuma registro foi encontrada!");

            return registros;

        }

        public async Task<RegistroModel> GetById(int id)
        {
            try { 
                var registro = await _registroRepository.GetById(id);

                if (registro == null)
                    throw new KeyNotFoundException(string.Format("O registro de ID {0} não foi encontrado!", id));

                return registro;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro buscar o registro de ID {0}. Detalhes: {1}", id, ex.Message));
                throw;
            }
        }

        public void Insert(RegistroModel registro)
        {
            _registroRepository.Insert(registro);
        }

        public void InsertBatch(List<RegistroModel> registros)
        {
            _registroRepository.InsertBatch(registros);
        }
    }
}
