using MLApplications.Core.Enumerations;

namespace MLApplications.Core.Specifications.Interfaces
{
    public interface ISearchQuery
    {
        int Start { get; set; }
        int PageSize { get; set; }
        string SortColumn { get; set; }
        SortDirection? SortDirection { get; set; }
    }
    
}
