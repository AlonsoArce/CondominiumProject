using Infraestructure.Models;
using System.Collections.Generic;

namespace Infraestructure.Repository
{
    public interface IRepositoryRubroCobro
    {
        IEnumerable<RubroCobro> GetRubroCobros();
        RubroCobro Save(RubroCobro rubroCobro);

        RubroCobro GetRubroByID(int id);
    }
}