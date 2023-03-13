namespace CourseLibrary.API.Entities
{
    public class Author{}
}

// DTO
namespace CourseLibrary.API.Models
{
    public class Author{};
}

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authors")]//[Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        IAuthorRepository _repo;
        IMapper _mapper;

        public AuthorsController(IAuthorRepository repository, IMapper mapper)
        {
            _repo = repository ?? throw new System.ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
        }

        [HttpGet()]
        [HttpHead] // Similar to get but with no response payload. Only to retrieve info in the header so lightweight.
        public ActionResult<IEnumerable<Models.Author>> GetAuthors()
        {   
            // Get the requested authors as entities from the data source
            var authors = _repo.GetAuthors();

            // Map them to the corresponding DTos (Models)
            var result = _mapper.Map<IEnumerable<Models.Author>>(authors);

            // Return with code
            return Ok(result);
        }

        // REturns the detaul
        [HttpGet("{authorId:guid}", Name = "GetAuthor")] // :syntax is to disambiguate if you have two ways of querying
        public IActionResult GetAuthor(Guid authorId)
        {
            var authorFound = _repo.GetAuthor(authorId);
            if(authorFound is null)
                return NotFound();
            
            return Ok(_mapper.Map<Models.Author>(authorFound));
        }

        // [HttpGet("{authorId:int")]
        // public IActionResult GetAuthor(int authorId)
        // {

        // }

        [HttpPost]
        public ActionResult<Models.Author> CreateAuthor(Models.AuthorCreation author)
        {
            var authorEntity = _mapper.Map<Entities.Author>(author);
            _repo.AddAuthor(authorEntity); // it is added to dbcontext session not the db itself
            _repo.Commit(); // now it is saved to the database

            var result = _mapper.Map<Models.Author>(authorEntity);
            return CreatedAtRoute("GetAuthor", new { authorId = result.Id }, result);
            // the <authorId> field in the anynomous object is matching with route parameter name in the GetAuthor action
            // Populates the Location header with URI for where to get the created resource
            // And the result is the response body
        }
    }
}