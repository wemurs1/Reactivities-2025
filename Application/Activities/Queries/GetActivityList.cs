using Application.Activities.DTO;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Queries;

public class GetActivityList
{
    private const int MaxPageSize = 50;

    public class Query : IRequest<Result<PagedList<ActivityDTO, DateTime?>>>
    {
        public DateTime? Cursor { get; set; }
        private int _pageSize = 3; // for an example
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

    }

    public class Handler(AppDbContext context, IMapper mapper, IUserAccessor userAccessor) : IRequestHandler<Query, Result<PagedList<ActivityDTO, DateTime?>>>
    {
        public async Task<Result<PagedList<ActivityDTO, DateTime?>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = context.Activities.OrderBy(x => x.Date).AsQueryable();

            if (request.Cursor.HasValue)
            {
                query = query.Where(x => x.Date >= request.Cursor.Value);
            }

            var activities = await query
                .Take(request.PageSize + 1)
                .ProjectTo<ActivityDTO>(mapper.ConfigurationProvider, new { currentUserId = userAccessor.GetUserId() })
                .ToListAsync(cancellationToken);

            DateTime? nextCursor = null;
            if (activities.Count > request.PageSize)
            {
                nextCursor = activities.Last().Date;
                activities.RemoveAt(activities.Count - 1);
            }

            return Result<PagedList<ActivityDTO, DateTime?>>.Success(
                new PagedList<ActivityDTO, DateTime?>
                {
                    Items = activities,
                    NextCursor = nextCursor
                }
            );
        }
    }
}
