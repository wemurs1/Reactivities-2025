using Application.Activities.DTO;
using Application.Core;
using AutoMapper;
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

    public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Query, Result<ActivityDTO>>
    {
        public async Task<Result<ActivityDTO>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activity = await context.Activities
                .Include(x => x.Attendees)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => request.Id == x.Id, cancellationToken);

            if (activity == null) return Result<ActivityDTO>.Failure("Activity not found", 404);

            return Result<ActivityDTO>.Success(mapper.Map<ActivityDTO>(activity));
        }
    }
}
