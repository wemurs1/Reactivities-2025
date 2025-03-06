using Application.Activities.DTO;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Queries;

public class GetActivityDetails
{
    public class Query : IRequest<Result<ActivityDTO>>
    {
        public required string Id { get; set; }
    }

    public class Handler(AppDbContext context, IMapper mapper, IUserAccessor userAccessor) : IRequestHandler<Query, Result<ActivityDTO>>
    {
        public async Task<Result<ActivityDTO>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activity = await context.Activities
                .ProjectTo<ActivityDTO>(mapper.ConfigurationProvider, new { currentUserId = userAccessor.GetUserId() })
                .FirstOrDefaultAsync(x => request.Id == x.Id, cancellationToken);

            if (activity == null) return Result<ActivityDTO>.Failure("Activity not found", 404);

            return Result<ActivityDTO>.Success(activity);
        }
    }
}
