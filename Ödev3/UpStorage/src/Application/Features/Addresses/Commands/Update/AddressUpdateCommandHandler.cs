using Application.Common.Interfaces;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Addresses.Commands.Update
{
    public class AddressUpdateCommandHandler : IRequestHandler<AddressUpdateCommand, Response<int>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public AddressUpdateCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Response<int>> Handle(AddressUpdateCommand request, CancellationToken cancellationToken)
        {
            var address = _applicationDbContext.Addresses.FirstOrDefault(x => x.Id == request.Id);

            address.Name = request.Name;
            address.UserId = request.UserId;
            address.CountryId = request.CountryId;
            address.CityId = request.CityId;
            address.District = request.District;
            address.PostCode = request.PostCode;
            address.AddressLine1 = request.AddressLine1;
            address.AddressLine2 = request.AddressLine2;
            address.CreatedOn = DateTimeOffset.Now;
            address.CreatedByUserId = request.UserId;
            address.IsDeleted = false;

            _applicationDbContext.Addresses.Update(address);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return new Response<int>($"The address named was successfully updated.");

        }
    }
}
