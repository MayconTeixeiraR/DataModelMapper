using DataModel_Mapper.Builder;

namespace DataModel_Mapper;

public record SqlBuilderContextOptions
{
    public ISqlTranslator Translator { internal get; init; }
}