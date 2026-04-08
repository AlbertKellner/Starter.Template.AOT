namespace Starter.Template.AOT.Api.Features.Query.NumberGetByValue;

public interface INumberGetByValueUseCase
{
    NumberGetByValueOutput Execute(int number);
}
