using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.Field;
using Models;

namespace api.Mappers
{
    public static class FieldMapper
    {
        public static FieldDto ToFieldDto(this Field fieldModel)
        {
            return new FieldDto
            {
                FieldId = fieldModel.FieldId,
                FieldName = fieldModel.FieldName,
                FieldImageUrl = fieldModel.FieldImageUrl,
                FieldDescription = fieldModel.FieldDescription,
                IsEnabled = fieldModel.IsEnabled,
                Categories = fieldModel.Categories.Select(c => c.ToCategorydDto()).ToList()

            };
        }

        public static Field ToFieldFromFieldDto(this CreateFieldDto fieldDto)
        {
            return new Field
            {
                FieldName = fieldDto.FieldName,
                FieldImageUrl = fieldDto.FieldImageUrl,
                FieldDescription = fieldDto.FieldDescription,
            };
        }

    }
}