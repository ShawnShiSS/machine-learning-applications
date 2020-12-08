using MLApplications.Core.Constants;
using MLApplications.Core.Entities.Base;
using MLApplications.Core.Enumerations;
using System.Collections.Generic;

namespace MLApplications.Core.Entities
{
    /// <summary>
    ///     Comment
    /// </summary>
    public class WebComment : BaseEntity
    {
        /// <summary>
        ///     Feedback Type, which will be used as the partition key
        /// </summary>
        public string FeedbackType => EntityConstants.FEEDBACK_TYPE_WEBCOMMENT;

        /// <summary>
        ///     Feedback
        /// </summary>
        public string Feedback { get; set; }

        /// <summary>
        ///     Sentiment Type 
        /// </summary>
        public SentimentType? SentimentType { get; set; }
    }
}
