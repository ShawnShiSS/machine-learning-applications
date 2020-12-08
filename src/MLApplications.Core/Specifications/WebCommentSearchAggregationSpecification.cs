using MLApplications.Core.Constants;
using MLApplications.Core.Enumerations;
using Ardalis.Specification;

namespace MLApplications.Core.Specifications
{
    /// <summary>
    ///     Specification for searching and returning aggregated value. E.g. Count, Sum, etc..
    ///     This is similar to a search specification, minus the sorting.
    /// </summary>
    public class WebCommentSearchAggregationSpecification : Specification<Entities.WebComment>
    {
        public WebCommentSearchAggregationSpecification(string feedback="")
        {
            Query.Where(x =>
                // Must include FeedbackType, because it is part of the Partition Key
                x.FeedbackType == EntityConstants.FEEDBACK_TYPE_WEBCOMMENT
                &&
                x.EntityStatus == EntityStatus.Active);

            if (!string.IsNullOrWhiteSpace(feedback))
            {
                Query.Where(x => x.Feedback.ToLower().Contains(feedback.ToLower()));
            }

        }

    }
}
