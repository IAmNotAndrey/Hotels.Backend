using Microsoft.EntityFrameworkCore;

namespace Hotels.Application.Interfaces.Services;

public interface IContextDefaultValuesSetterService
{
    void SetDefaultValues(ModelBuilder builder);
}
