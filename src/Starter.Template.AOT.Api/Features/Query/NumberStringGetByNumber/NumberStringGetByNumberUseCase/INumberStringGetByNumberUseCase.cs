namespace Starter.Template.AOT.Api.Features.Query.NumberStringGetByNumber;

public interface INumberStringGetByNumberUseCase
{
    NumberStringGetByNumberOutput? Execute(int number);
}
