using System.Reflection;
using DataModel_Mapper.Configuration;

namespace DataModel_Mapper;

public record DataModelInfo(
    MemberInfo Property, 
    string ColumnName, 
    TableConfiguration TableConfiguration);