namespace Starter.Template.AOT.Api.Features.Query.NumberToString;

public interface INumberToStringUseCase
{
    Task<NumberToStringOutput?> ExecuteAsync(int number);
}
