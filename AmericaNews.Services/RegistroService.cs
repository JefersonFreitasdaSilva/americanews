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

        public List<RegistroModel> GetAll()
        {
            return _registroRepository.GetAll();
        }

        public RegistroModel? GetById(int id)
        {
            return _registroRepository.GetById(id);
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
