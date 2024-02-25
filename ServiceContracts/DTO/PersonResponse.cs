﻿using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class PersonResponse
    {
        public Guid PersonId { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public double? Age { get; set; }
        public string? Country { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != typeof(PersonResponse))
                return false;
            PersonResponse person = (PersonResponse)obj;
            return PersonId == person.PersonId && PersonName == person.PersonName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest() { PersonId = PersonId, PersonName = PersonName, Email = Email, Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true), Address = Address, DateOfBirth = DateOfBirth, CountryId = CountryId, ReceiveNewsLetters = ReceiveNewsLetters };
    }
    }

    public static class PersonExtenstions
    {
        public static PersonResponse ToPersonResponse(this Person person )
        {
            return new PersonResponse()
            {
                PersonId = person.PersonId,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                CountryId = person.CountryId,
                Address = person.Address,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25):null,
            };
        }

    }
   
}
