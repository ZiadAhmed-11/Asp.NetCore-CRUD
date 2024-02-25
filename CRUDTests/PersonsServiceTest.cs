using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        public PersonsServiceTest()
        {
            _personsService = new PersonsService();
            _countriesService = new CountriesService(false);
        }

        #region AddPerson()
        //when PersonAddRequest is null =>ArgumentNullException
        [Fact]
        public void AddPerson_NullRequest()
        {
            //arrange
            PersonAddRequest? request = null;
            //assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personsService.AddPerson(request);
            });
        }
        //When PersonName is null
        [Fact]
        public void AddPerson_NullPersonName()
        {
            //Arrange
            PersonAddRequest? request = new PersonAddRequest() { PersonName = null };
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personsService.AddPerson(request);
            });
        }
        //When All is valid
        [Fact]
        public void AddPerson_PersonInsert()
        {
            //Arrange
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Ziad",
                Email = "Ziad@gmail.com",
                Address = "25 Fayoum",
                CountryId = Guid.NewGuid(),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2002-08-11"),
                ReceiveNewsLetters = true
            };
            //Act
            PersonResponse personResponse=_personsService.AddPerson(personAddRequest);
            List<PersonResponse> person_list = _personsService.GetAllPersons();
            //Assert
            Assert.True(personResponse.PersonId != Guid.Empty);
            Assert.Contains(personResponse, person_list );
        }
        #endregion

        #region GetPersonById
        [Fact]
        public void GetPersonByPersonId_NullId()
        {
            //Arrange
            Guid? personId = null;
            //Act
            PersonResponse? Person_Response_from_get_method = _personsService.GetPersonByPersonId(personId);
            //Assert
            Assert.Null(Person_Response_from_get_method);

        }
        [Fact]
        public void GetPersonByPersonId_All_Valid()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest() { CountryName = "Canada" };
            CountryResponse? countryResponse = _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest? person_Add_Request=new PersonAddRequest() { PersonName="Ziad",Email="Ziad@gmail.com",Address="21 Fayoum",CountryId=countryResponse.CountryID,DateOfBirth=DateTime.Parse("2002-08-11"),ReceiveNewsLetters=true,Gender=ServiceContracts.Enums.GenderOptions.Male};
            PersonResponse? PersonResponse_From_add=_personsService.AddPerson( person_Add_Request);

            //Act
            PersonResponse? PersonResponse_From_get = _personsService.GetPersonByPersonId(PersonResponse_From_add.PersonId);

            //Assert
            Assert.Equal(PersonResponse_From_add, PersonResponse_From_get);
        }
        #endregion

        #region GetAllPersons
        [Fact]
        public void GetAllPersons_Empty_List()
        {
            List<PersonResponse>actualPersons=_personsService.GetAllPersons();
            Assert.Empty(actualPersons);
        }
        [Fact]
        public void GetAllPersons_FewPersons()
        {
            //Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

            CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@example.com", Gender = GenderOptions.Male, Address = "address of smith", CountryId = country_response_1.CountryID, DateOfBirth = DateTime.Parse("2002-05-06"), ReceiveNewsLetters = true };

            PersonAddRequest person_request_2 = new PersonAddRequest() { PersonName = "Mary", Email = "mary@example.com", Gender = GenderOptions.Female, Address = "address of mary", CountryId = country_response_2.CountryID, DateOfBirth = DateTime.Parse("2000-02-02"), ReceiveNewsLetters = false };

            PersonAddRequest person_request_3 = new PersonAddRequest() { PersonName = "Rahman", Email = "rahman@example.com", Gender = GenderOptions.Male, Address = "address of rahman", CountryId = country_response_2.CountryID, DateOfBirth = DateTime.Parse("1999-03-03"), ReceiveNewsLetters = true };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest> { person_request_1, person_request_2, person_request_3 };
            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = _personsService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }
            //Act
            List<PersonResponse> persons_list_from_get = _personsService.GetAllPersons();

            //Assert
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                Assert.Contains(person_response_from_add, persons_list_from_get);
            }
        }
        #endregion
    }
}
