using Infraestructure.Models;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public interface IServiceRubroCobro
    {
        IEnumerable<RubroCobro> GetRubroCobros();
        RubroCobro Save(RubroCobro rubroCobro);

        RubroCobro GetRubroByID(int id);
    }
}