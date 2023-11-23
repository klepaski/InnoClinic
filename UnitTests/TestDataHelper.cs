using Lw.Data.Entity;
using Microsoft.EntityFrameworkCore;
using OfficesAPI.Models;

namespace UnitTests
{
    public class TestDataHelper
    {
        public static List<Office> GetFakeOfficesList()
        {
            return new List<Office>()
            {
                new Office {
                    Id = 1,
                    City = "Brest",
                    Street = "Kolesnika",
                    HouseNumber = "4",
                    OfficeNumber = "54",
                    PhotoUrl = "https://avatars.mds.yandex.net/get-altay/4464784/2a0000017840924485aa42a8ef3d614ace76/L_height",
                    RegistryPhoneNumber = "+375333186223",
                    Status = Status.Inactive
                },
                new Office {
                    Id = 2,
                    City = "Minsk",
                    Street = "Sverdlova",
                    HouseNumber = "13a",
                    OfficeNumber = "226",
                    PhotoUrl = "https://avatars.mds.yandex.net/get-altay/933207/2a00000161bf61714998e315745eea065577/orig",
                    RegistryPhoneNumber = "+375333733839",
                    Status = Status.Active
                }
            };
        }
    }
}
