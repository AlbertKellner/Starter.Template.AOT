namespace Starter.Template.AOT.Api.Features.Query.NumberGetLabel;

public interface INumberGetLabelUseCase
{
    NumberGetLabelOutput? Execute(int number);
}
