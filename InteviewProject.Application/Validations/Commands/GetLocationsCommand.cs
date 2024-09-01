using InteviewProject.Application.Pagination;

namespace InteviewProject.Application.Validations.Commands
{
    public class GetLocationsCommand:PaginatedQuery
    {
        public string Location { get; set; }
    }
}
