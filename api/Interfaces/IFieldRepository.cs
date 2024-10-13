using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.Field;
using Models;

namespace Interfaces
{
    public interface IFieldRepository
    {
        Task<List<Field>> GetAllFieldsAsync();
        Task<Field?> GetByIdFieldAsync(int id);
        Task<Field> CreateFieldAsync(Field fieldModel);
        Task<Field?> UpdateFieldAsync(int id, UpdateFieldDto fieldDto);
        Task<Field?> DeleteFieldAsync(int id);
    }
}