namespace Starter.Template.AOT.Api.Features.Query.NumberStringGetByValue;

public interface INumberStringGetByValueUseCase
{
    NumberStringGetByValueOutput? Execute(int value);
}
