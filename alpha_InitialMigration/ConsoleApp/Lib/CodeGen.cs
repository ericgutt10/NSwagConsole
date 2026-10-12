using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleApp.Lib;
/*
 * settings
{NSwag.CodeGeneration.CSharp.CSharpClientGeneratorSettings}
    AdditionalContractNamespaceUsages: {string[0]}
    AdditionalNamespaceUsages: {string[0]}
    CSharpGeneratorSettings: {NJsonSchema.CodeGeneration.CSharp.CSharpGeneratorSettings}
    ClassName: "EKanbanApiClient"
    ClientBaseClass: null
    ClientBaseInterface: null
    ClientClassAccessModifier: "public"
    ClientInterfaceAccessModifier: "public"
    CodeGeneratorSettings: {NJsonSchema.CodeGeneration.CSharp.CSharpGeneratorSettings}
    ConfigurationClass: null
    DisposeHttpClient: true
    ExceptionClass: "ApiException"
    ExcludedOperationIds: {string[0]}
    ExcludedParameterNames: {string[0]}
    ExposeJsonSerializerSettings: false
    GenerateBaseUrlProperty: true
    GenerateClientClasses: true
    GenerateClientInterfaces: false
    GenerateDtoTypes: true
    GenerateExceptionClasses: false
    GenerateOptionalParameters: false
    GeneratePrepareRequestAndProcessResponseAsAsyncMethods: false
    GenerateResponseClasses: true
    GenerateSyncMethods: false
    GenerateUpdateJsonSerializerSettingsMethod: true
    HttpClientType: "System.Net.Http.HttpClient"
    IncludedOperationIds: {string[0]}
    InjectHttpClient: true
    OperationNameGenerator: {NSwag.CodeGeneration.OperationNameGenerators.MultipleClientsFromOperationIdOperationNameGenerator}
    ParameterArrayType: "System.Collections.Generic.ICollection"
    ParameterDateFormat: "yyyy-MM-dd"
    ParameterDateTimeFormat: "s"
    ParameterDictionaryType: "System.Collections.Generic.IDictionary"
    ParameterNameGenerator: {NSwag.CodeGeneration.DefaultParameterNameGenerator}
    ProtectedMethods: {string[0]}
    QueryNullValue: ""
    ResponseArrayType: "System.Collections.Generic.ICollection"
    ResponseClass: "SwaggerResponse"
    ResponseDictionaryType: "System.Collections.Generic.IDictionary"
    SerializeTypeInformation: false
    SuppressClientClassesOutput: false
    SuppressClientInterfacesOutput: false
    UseBaseUrl: false
    UseHttpClientCreationMethod: false
    UseHttpRequestMessageCreationMethod: false
    UseRequestAndResponseSerializationSettings: false
    WrapDtoExceptions: true
    WrapResponseMethods: {string[0]}
    WrapResponses: false

 */

public static class CodeGen
{
    private static JsonSerializerOptions? _settings;

    public static JsonSerializerOptions JsonOpt => _settings ??= new JsonSerializerOptions()
    {
        ReferenceHandler = ReferenceHandler.Preserve,
        PropertyNameCaseInsensitive = true,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
        WriteIndented = true,
        RespectNullableAnnotations = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public static async Task<FileInfo?> GenerateCSharpClient(this
        OpenApiDocument? document,
        string? className,
        string? nameSpace)
    {
        ArgumentNullException.ThrowIfNull(document, nameof(document));

        className ??= "ApiClient";
        nameSpace ??= "Common.Client";
        // Client Generation Defaults
        /*
        ClassName = "{controller}Client";
        GenerateExceptionClasses = true;
        ExceptionClass = "ApiException";
        ClientClassAccessModifier = "public";
        ClientInterfaceAccessModifier = "public";
        UseBaseUrl = true;
        HttpClientType = "System.Net.Http.HttpClient";
        WrapDtoExceptions = true;
        DisposeHttpClient = true;
        ParameterDateTimeFormat = "s";
        ParameterDateFormat = "yyyy-MM-dd";
        GenerateUpdateJsonSerializerSettingsMethod = true;
        UseRequestAndResponseSerializationSettings = false;
        QueryNullValue = "";
        GenerateBaseUrlProperty = true;
        ExposeJsonSerializerSettings = false;
        InjectHttpClient = true;
        ProtectedMethods = [];
        */

        // Code generation Defaults
        /*
            GenerateDefaultValues = true;
            ExcludedTypeNames = [];
         */
        var settings = new CSharpClientGeneratorSettings
        {
            ClassName = className,
            GenerateExceptionClasses = false,

            //GenerateClientClasses = true,
            //GenerateResponseClasses = true,
            //ParameterArrayType = nameof(System.Collections.IEnumerable),
            //ResponseArrayType = nameof(System.Collections.IEnumerable),
            //ParameterDictionaryType = nameof(System.Collections.IDictionary),
            //ResponseDictionaryType = nameof(System.Collections.IDictionary),
            //GenerateUpdateJsonSerializerSettingsMethod = true,
            //ExposeJsonSerializerSettings = false,
            //SuppressClientClassesOutput = false,
            ParameterDateFormat = "s",
            GenerateOptionalParameters = false,
            WrapResponses = false,
            GenerateResponseClasses = true,



            CSharpGeneratorSettings =
            {
                GenerateNullableReferenceTypes = true,
                Namespace = nameSpace ,
                JsonLibrary = CSharpJsonLibrary.SystemTextJson,
                JsonPolymorphicSerializationStyle = CSharpJsonPolymorphicSerializationStyle.SystemTextJson,
                JsonLibraryVersion = 10.0M,
                //JsonSerializerSettingsTransformationMethod = "JsonOpt",
                //GenerateNullableReferenceTypes = true,
                //GenerateOptionalPropertiesAsNullable = true,

                //ArrayType = nameof(System.Collections.IEnumerable),
                //ArrayInstanceType = nameof(System.Collections.ObjectModel.Collection<>),
                //DictionaryType = nameof(System.Collections.IDictionary),
                //DictionaryInstanceType = nameof(System.Collections.ObjectModel.ReadOnlyDictionary<,>),
                //ArrayBaseType = nameof(System.Collections.CollectionBase),
                //DictionaryBaseType = nameof(System.Collections.DictionaryBase),
                TimeSpanType = nameof(TimeSpan),
                TimeType = nameof(TimeOnly),
                DateTimeType = nameof(DateTime),
                DateType = nameof(DateOnly),
            },
        };

        //var settings = new CSharpClientGeneratorSettings
        //{
        //    UseBaseUrl = false,
        //    ClassName = className,
        //    GenerateClientInterfaces = true,
        //    GenerateClientClasses = false,
        //    GenerateExceptionClasses = false,
        //    ExposeJsonSerializerSettings = true,
        //    InjectHttpClient = true,
        //    DisposeHttpClient = true,
        //    ExceptionClass = "ApiException",

        //    ParameterArrayType = nameof(System.Collections.ICollection),
        //    ResponseArrayType = nameof(System.Collections.ICollection),
        //    ParameterDictionaryType = nameof(System.Collections.IDictionary),
        //    ResponseDictionaryType = nameof(System.Collections.IDictionary),
        //    AdditionalContractNamespaceUsages = [],
        //    AdditionalNamespaceUsages = [],
        //    ClientBaseClass = null,
        //    ClientBaseInterface = null,
        //    ClientClassAccessModifier = "public",
        //    ClientInterfaceAccessModifier = "public",
        //    ConfigurationClass = null,
        //    ExcludedOperationIds = null,
        //    ExcludedParameterNames = null,
        //    GenerateBaseUrlProperty = true,

        //    GenerateDtoTypes = true,
        //    GenerateOptionalParameters = true,
        //    GeneratePrepareRequestAndProcessResponseAsAsyncMethods = true,
        //    GenerateResponseClasses = true,
        //    GenerateSyncMethods = false,
        //    GenerateUpdateJsonSerializerSettingsMethod = true,
        //    HttpClientType = nameof(HttpClient),
        //    IncludedOperationIds = null,
        //    OperationNameGenerator = null,
        //    ParameterDateFormat = "yyyy-MM-dd",
        //    ParameterDateTimeFormat = "s",
        //    ParameterNameGenerator = null,
        //    ProtectedMethods = [],
        //    QueryNullValue = string.Empty,
        //    ResponseClass = null,
        //    SerializeTypeInformation = false,
        //    SuppressClientClassesOutput = false,
        //    SuppressClientInterfacesOutput = false,
        //    UseHttpClientCreationMethod = false,
        //    UseHttpRequestMessageCreationMethod = false,
        //    UseRequestAndResponseSerializationSettings = false,
        //    WrapDtoExceptions = false,
        //    WrapResponseMethods = [],
        //    WrapResponses = false,

        //    CSharpGeneratorSettings =
        //    {
        //        GenerateNativeRecords = true,
        //        GenerateNullableReferenceTypes = true,
        //        GenerateOptionalPropertiesAsNullable = true,

        //        ClassStyle = CSharpClassStyle.Poco,
        //        JsonLibrary = CSharpJsonLibrary.SystemTextJson,
        //        JsonPolymorphicSerializationStyle = CSharpJsonPolymorphicSerializationStyle.SystemTextJson,
        //        JsonLibraryVersion = 10.0M,
        //        Namespace = "HttpClients",

        //        InlineNamedAny = true,
        //        InlineNamedArrays = true,
        //        InlineNamedDictionaries = true,
        //        InlineNamedTuples = true,

        //        ArrayType = nameof(System.Collections.ICollection),
        //        ArrayInstanceType = nameof(System.Collections.ObjectModel.Collection<>),
        //        DictionaryType = nameof(System.Collections.IDictionary),
        //        DictionaryInstanceType = nameof(System.Collections.ObjectModel.ReadOnlyDictionary<,>),
        //        ArrayBaseType = nameof(System.Collections.CollectionBase),
        //        DictionaryBaseType = nameof(System.Collections.DictionaryBase),

        //        AnyType = nameof(Object),
        //        DateTimeType = nameof(DateTime),
        //        DateType = nameof(DateOnly),
        //        EnforceFlagEnums = false,
        //        EnumNameGenerator = new DefaultEnumNameGenerator(),
        //        ExcludedTypeNames = [],
        //        FieldNamePrefix = "",
        //        GenerateDataAnnotations = true,
        //        GenerateDefaultValues = true,
        //        GenerateImmutableArrayProperties = false,
        //        GenerateImmutableDictionaryProperties = false,
        //        GenerateJsonMethods = false,
        //        HandleReferences = false,
        //        JsonConverters = [],
        //        JsonSerializerSettingsTransformationMethod = null,
        //        NumberDecimalType = nameof(Decimal),
        //        NumberDoubleType = nameof(Double),
        //        NumberFloatType = nameof(Single),
        //        NumberType = nameof(Double),
        //        PropertySetterAccessModifier = string.Empty,
        //        RequiredPropertiesMustBeDefined = true,
        //        SchemaType = SchemaType.JsonSchema,
        //        SortConstructorParameters = true,
        //        TemplateDirectory = null,
        //        TimeSpanType = nameof(TimeSpan),
        //        TimeType = nameof(TimeOnly),
        //        TypeAccessModifier = "public",
        //        WriteAccessor = "set",
        //        UseRequiredKeyword = false,

        //        PropertyNameGenerator = new CSharpPropertyNameGenerator(),
        //        TypeNameGenerator = new DefaultTypeNameGenerator()
        //        //TemplateFactory = null,
        //        //ValueGenerator = null,
        //    },
        //};

        try
        {
            var generator = new CSharpClientGenerator(document, settings);
            var generatedCode = generator.GenerateFile();

            var path = Path.Combine(
                AppContext.BaseDirectory,
                $"../../../../",
                "Libraries/SwaggerClient",
                $"{settings.ClassName}.cs");

            var file = new FileInfo(path);

            if (!file.Directory!.Exists) { file.Directory.Create(); }

            await File.WriteAllTextAsync(file.FullName, generatedCode);

            return new FileInfo(file.FullName);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Console.WriteLine(ex.Message);
        }

        return null;
    }
}