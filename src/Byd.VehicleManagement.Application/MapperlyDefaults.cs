using Riok.Mapperly.Abstractions;

[assembly: MapperDefaults(
    RequiredMappingStrategy = RequiredMappingStrategy.Target,
    IncludedMembers = MemberVisibility.All,
    IncludedConstructors = MemberVisibility.All)]
