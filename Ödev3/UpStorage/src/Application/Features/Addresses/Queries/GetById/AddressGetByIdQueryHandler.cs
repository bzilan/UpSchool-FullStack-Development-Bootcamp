using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Addresses.Queries.GetById
{
    public class AddressGetByIdQueryHandler : IRequestHandler<AddressGetByIdQuery, List<AddressGetByIdDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        public AddressGetByIdQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<List<AddressGetByIdDto>> Handle(AddressGetByIdQuery request, CancellationToken cancellationToken)
        {
            var dbQuery = _applicationDbContext.Addresses.AsQueryable();

            dbQuery = dbQuery.Where(x => x.Id == request.Id);

            if (request.IsDeleted.HasValue) dbQuery = dbQuery.Where(x => x.IsDeleted == request.IsDeleted.Value);

            dbQuery = dbQuery.Include(x => x.City);

            var addresses = await dbQuery.ToListAsync(cancellationToken);

            var addressDtos = MapAddressesToGetByIdDtos(addresses);

            return addressDtos.ToList();
        }
        private IEnumerable<AddressGetByIdDto> MapAddressesToGetByIdDtos(List<Address> addresses)
        {
            foreach (var address in addresses)
            {
                yield return new AddressGetByIdDto()
                {
                    Id = address.Id,
                    Name = address.Name,
                    UserId = address.UserId,
                    CountryId = address.CountryId,
                    CityId = address.CityId,
                    District = address.District,
                    PostCode = address.PostCode,
                    AddressLine1 = address.AddressLine1,
                    AddressLine2 = address.AddressLine2,
                };
            }
        }
    } 
}
