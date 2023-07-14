using Application.Common.Interfaces;
using FluentValidation;

namespace Application.Features.Orders.Commands.Add
{
    public class OrderAddCommandValidator : AbstractValidator<OrderAddCommand>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        public OrderAddCommandValidator(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.ProductCrawlType).NotNull()
                .WithMessage("You must select the product type");

            RuleFor(x => x.Id).MustAsync(CheckIfOrderExists)
                .WithMessage("This order number is already exists in the database");
        }

        private Task<bool> CheckIfOrderExists(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(!_applicationDbContext.Orders.Any(x => x.Id == id));
        }
    }
}
