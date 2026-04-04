using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Starter.Template.AOT.Api.Infra.ModelBinding;

/// <summary>
/// Workaround para .NET 10 com PublishAot=true: ModelMetadata.IsEnhancedModelMetadataSupported
/// é um static readonly bool iniciado como false. Com MVC rodando em modo JIT, os providers
/// (SimpleTypeModelBinderProvider, TryParseModelBinderProvider) verificam esse flag antes de
/// acessar IsConvertibleType e IsParseableType, lançando NotSupportedException.
/// Este activator usa DynamicMethod com skipVisibility=true (modo JIT) para emitir Stsfld
/// diretamente no backing field readonly. Em modo Native AOT, DynamicMethod não é suportado;
/// neste caso o activator é ignorado porque FallbackSimpleTypeModelBinderProvider e
/// NullModelBinderProvider já substituem todos os providers que dependem desse flag.
/// </summary>
internal static class EnhancedModelMetadataActivator
{
    internal static void Activate(ILogger logger)
    {
        // In Native AOT, DynamicMethod and FieldInfo.SetValue on initonly fields do not work.
        // AOT-compatible providers (FallbackSimpleTypeModelBinder, NullModelBinder) already
        // cover all cases where IsEnhancedModelMetadataSupported would be required.
        if (!RuntimeFeature.IsDynamicCodeSupported)
        {

            logger.LogDebug(
                "[EnhancedModelMetadataActivator][Activate] Modo Native AOT — activator ignorado. Providers AOT-compatíveis garantem o model binding.");

            return;
        }

        var modelMetadataType = typeof(Microsoft.AspNetCore.Mvc.ModelBinding.ModelMetadata);

        var backingField = modelMetadataType.GetField(
            "<IsEnhancedModelMetadataSupported>k__BackingField",
            BindingFlags.NonPublic | BindingFlags.Static);

        if (backingField is not null)
        {
            try
            {
                var dm = new DynamicMethod(
                    "SetEnhancedModelMetadataSupported",
                    typeof(void),
                    Type.EmptyTypes,
                    modelMetadataType,
                    skipVisibility: true);

                var il = dm.GetILGenerator();
                il.Emit(OpCodes.Ldc_I4_1);
                il.Emit(OpCodes.Stsfld, backingField);
                il.Emit(OpCodes.Ret);

                dm.Invoke(null, null);

                logger.LogInformation(
                    "[EnhancedModelMetadataActivator][Activate] IsEnhancedModelMetadataSupported definido via DynamicMethod em {Type}",
                    modelMetadataType.FullName);

                return;
            }
            catch (Exception ex)
            {

                logger.LogWarning(
                    "[EnhancedModelMetadataActivator][Activate] DynamicMethod falhou: {Message}",
                    ex.Message);

            }

            try
            {
                backingField.SetValue(null, true);

                logger.LogInformation(
                    "[EnhancedModelMetadataActivator][Activate] IsEnhancedModelMetadataSupported definido via FieldInfo.SetValue em {Type}",
                    modelMetadataType.FullName);

                return;
            }
            catch (Exception ex)
            {

                logger.LogWarning(
                    "[EnhancedModelMetadataActivator][Activate] FieldInfo.SetValue falhou: {Message}",
                    ex.Message);

            }
        }

        logger.LogWarning(
            "[EnhancedModelMetadataActivator][Activate] IsEnhancedModelMetadataSupported não pôde ser ativado — model binding pode falhar em modo JIT");

    }
}
