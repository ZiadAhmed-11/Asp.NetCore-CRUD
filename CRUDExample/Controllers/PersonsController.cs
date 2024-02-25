using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers
{
    public class PersonsController : Controller
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        public PersonsController(IPersonsService personsService,ICountriesService countriesService)
        {
            _personsService = personsService;

            _countriesService = countriesService;

        }
        [Route("/persons/index")]
        [Route("/")]
        public IActionResult Index(string searchBy,string? searchString,string sortBy=nameof(PersonResponse.PersonName),SortOrderOptions sortOrder=SortOrderOptions.ASC)
        {
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                {nameof(PersonResponse.PersonName),"Person Name" },
                {nameof(PersonResponse.Email),"Email" },
                {nameof(PersonResponse.DateOfBirth),"Date of Birth" },
                {nameof(PersonResponse.Gender),"Gender" },
                {nameof(PersonResponse.CountryId),"Country" },
                {nameof(PersonResponse.Address),"Address" }
            };
            List<PersonResponse> persons= _personsService.GetFilteredPersons(searchBy,searchString);

            //sorting
            List<PersonResponse> sortedPersons= _personsService.GetSortedPersons(persons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder=sortOrder.ToString();
            return View(sortedPersons);
        }


        //Executes when the user clicks on "Create Person" hyperlink (while opening the create view)
        [Route("persons/create")]
        [HttpGet]
        public IActionResult Create()
        {
            List<CountryResponse> countries = _countriesService.GetAllCountries();
            ViewBag.Countries = countries;

            return View();
        }

        [HttpPost]
        [Route("persons/create")]
        public IActionResult Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries;

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }

            //call the service method
            PersonResponse personResponse = _personsService.AddPerson(personAddRequest);

            //navigate to Index() action method (it makes another get request to "persons/index"
            return RedirectToAction("Index", "Persons");
        }
        [HttpGet]
        [Route("[action]/{PersonId}")]
        public IActionResult Edit(Guid personId)
        {
            PersonResponse? personResponse = _personsService.GetPersonByPersonId(personId);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

            List<CountryResponse> countries = _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(temp =>
            new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });

            return View(personUpdateRequest);
        }

        [HttpPost]
        [Route("[action]/{personID}")]
        public IActionResult Edit(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? personResponse = _personsService.GetPersonByPersonId(personUpdateRequest.PersonId);

            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                PersonResponse updatedPerson = _personsService.UpdatePerson(personUpdateRequest);
                return RedirectToAction("Index");
            }
            else
            {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries.Select(temp =>
                new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }
        }


        [HttpGet]
        [Route("[action]/{personID}")]
        public IActionResult Delete(Guid? personID)
        {
            PersonResponse? personResponse = _personsService.GetPersonByPersonId(personID);
            if (personResponse == null)
                return RedirectToAction("Index");

            return View(personResponse);
        }

        [HttpPost]
        [Route("[action]/{personID}")]
        public IActionResult Delete(PersonUpdateRequest personUpdateResult)
        {
            PersonResponse? personResponse = _personsService.GetPersonByPersonId(personUpdateResult.PersonId);
            if (personResponse == null)
                return RedirectToAction("Index");

            _personsService.DeletePerson(personUpdateResult.PersonId);
            return RedirectToAction("Index");
        }
    }
}
