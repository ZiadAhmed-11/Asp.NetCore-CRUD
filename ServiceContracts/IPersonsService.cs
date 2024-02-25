using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IPersonsService
    {
        PersonResponse AddPerson(PersonAddRequest? personAddRequest);
        List<PersonResponse> GetAllPersons();
        PersonResponse? GetPersonByPersonId(Guid? personId);
        List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString);
        List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);
        PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest);
        bool DeletePerson(Guid? personID);
    }
}
