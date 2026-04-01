namespace Starter.Template.AOT.Api.Features.Query.NumberStringGet;

public interface INumberStringGetUseCase
{
    NumberStringGetOutput? Execute(int number);
}
