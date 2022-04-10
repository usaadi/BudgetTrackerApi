using FluentValidation.Results;

namespace Application.Common.Exceptions;

public class RelatedDataExistException : Exception
{
    public RelatedDataExistException()
        : base("There exist related data for this entity")
    {
    }
}