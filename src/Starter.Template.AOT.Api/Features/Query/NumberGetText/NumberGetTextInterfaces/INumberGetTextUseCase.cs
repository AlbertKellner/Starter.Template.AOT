namespace Starter.Template.AOT.Api.Features.Query.NumberGetText;

public interface INumberGetTextUseCase
{
    NumberGetTextOutput? Execute(int number);
}
