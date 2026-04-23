using Starter.Template.AOT.Api.Shared.ColorManipulation;
using Starter.Template.AOT.Api.Shared.DiskItem;

namespace Starter.Template.AOT.Api.Features.Query.DiskItemsGetAll;

public class DiskItemsGetAllUseCase(
    IDiskItemsGetAllRepository repository,
    ILogger<DiskItemsGetAllUseCase> logger) : IDiskItemsGetAllUseCase
{
    public async Task<DiskItemsGetAllOutput> ExecuteAsync(int driveIndex)
    {
        logger.LogInformation("[DiskItemsGetAllUseCase][ExecuteAsync] Obter estrutura de disco para índice {DriveIndex}", driveIndex);

        var entity = await repository.GetByDriveIndexAsync(driveIndex);

        logger.LogInformation("[DiskItemsGetAllUseCase][ExecuteAsync] Aplicar cores a {Count} itens de nível raiz", entity.Children.Count);

        var colorIndex = 0;

        foreach (var child in entity.Children)
        {
            child.Color = ColorUtils.GetColorForIndex(colorIndex);
            ApplyColorsToChildren(child.Children, ColorUtils.GenerateBaseColor(child.Color), 1);
            colorIndex++;
        }

        logger.LogInformation("[DiskItemsGetAllUseCase][ExecuteAsync] Mapear entidade para output");

        var output = MapToOutput(entity);

        logger.LogInformation("[DiskItemsGetAllUseCase][ExecuteAsync] Retornar output com estrutura de disco completa");

        return output;
    }

    private static void ApplyColorsToChildren(List<DiskItemEntity> items, string baseColor, int level)
    {
        var count = 0;

        foreach (var item in items)
        {
            var interpolated = ColorUtils.InterpolateToGrey(baseColor, Math.Min(1.0, level * 0.2));
            item.Color = count % 2 == 0
                ? ColorUtils.Saturate(interpolated, 0.2)
                : ColorUtils.Desaturate(interpolated, 0.2);

            ApplyColorsToChildren(item.Children, ColorUtils.GenerateBaseColor(item.Color), level + 1);
            count++;
        }
    }

    private static DiskItemsGetAllOutput MapToOutput(DiskItemEntity entity) =>
        new()
        {
            Name = entity.Name,
            Value = entity.Size,
            Color = entity.Color,
            FormattedSize = entity.FormattedSize,
            Children = entity.Children.Count > 0
                ? entity.Children.Select(MapToOutput).ToList()
                : null
        };
}
