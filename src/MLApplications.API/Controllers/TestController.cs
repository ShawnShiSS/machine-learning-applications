using MediatR;
using Microsoft.AspNetCore.Mvc;
using MLApplications.SentimentAnalysis;
using System;
using System.Threading.Tasks;

namespace MLApplications.API.Controllers
{

    /// <summary>
    ///     Controller
    /// </summary>
    //[Authorize("Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ConsumeModel _consumeModel;

        /// <summary>
        ///     Controller ctor
        /// </summary>
        /// <param name="mediator"></param>
        public TestController(IMediator mediator, 
                              ConsumeModel consumeModel)
        {
            this._mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this._consumeModel = consumeModel ?? throw new ArgumentNullException(nameof(consumeModel));

        }


        // GET: api/Test/5
        /// <summary>
        ///     Get a test by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetTest")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ModelOutput> Get(string id)
        {
            // Create single instance of sample data from first line of dataset for model input
            ModelInput sampleData = new ModelInput()
            {
                Comment = @$"{id}",
            };

            // Make a single prediction on the sample data and print results
            var predictionResult = ConsumeModel.Predict(sampleData);

            return predictionResult;
        }

        

    }
}

