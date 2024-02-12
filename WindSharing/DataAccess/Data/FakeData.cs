using Core.Domain;
using Microsoft.VisualBasic;

namespace DataAccess.Data
{
    public static class FakeData
    {
        public static IEnumerable<Activity> Activities => new List<Activity>()
        {
            new Activity()
            {
                Id = Guid.Parse("d75ca686-2e5f-4dfd-b4ad-3a5528b7fefe"), //3
                Name = "Serfing",
                IconName = "Serf",
                Spots = new List<Spot>()
                {
                    new Spot()
                    {
                        Id = Guid.Parse("4BCC4F3B-DB71-4442-A65B-556E2513700D"),
                        //PlaceId = Guid.Parse("2bca4fc2-359b-4150-9ebe-c19cfcc8a9b6"),
                        Name = "Sun place",
                        Note = @"Сёрф-спот на любой уровень катания от новичка до эксперта.

Типы волн: пойнтбрейки, бичбрейки, рифбрейки.

Сезон: ноябрь - апрель.

Логистика: общественный транспорт ограничен — вам понадобится автомобиль (полный привод) или мотоцикл.

Дополнительные мероприятия: исследуйте приливные бассейны, посетите природные горячие источники в Лас-Салинас, прогуляйтесь по природному заповеднику Чакосенте, прокатитесь на лошадях по пляжу или попробуйте свои силы в подводной охоте.",
                        CreateDataTime = new DateTime(2022, 12, 20),
                        Latitude = 43.55896934593335,
                        Longitude = 39.70559056533847,
                        IsActive = true,
                        SpotPhotos = new List<SpotPhoto>()
                        {
                            new SpotPhoto()
                            {
                                Id = Guid.Parse("30BE0198-7A29-4EE6-8BAB-B702F7A357E2"),
                                FileName = "Lisa_Andersen.jpg",
                                Comment = "девушка с доской",
                                CreateDataTime = new DateTime(2023, 02, 14)
                            },
                            new SpotPhoto()
                            {
                                Id = Guid.Parse("78791d84-8822-40a6-ae71-025db1ce12e9"),
                                FileName = "sea-ocean-people-man.jpg",
                                Comment = "мужчина на доске",
                                CreateDataTime = new DateTime(2023, 02, 08)
                            },

                        }
                    }
                }
            },

            new Activity()
            {
                Id = Guid.Parse("db0208a4-2609-4f07-8c50-8fd9e064d9a4"), //4
                Name = "Swimming",
                IconName = "Swim",
                Spots = new List<Spot>()
                {
                    new Spot()
                    {
                        Id = Guid.Parse("F91DBEBE-04A8-46BA-877E-62EF624A77F4"),
                        //PlaceId = Guid.Parse("9b882404-8db2-4a5e-afcf-075fb9ac3195"),
                        Name = "Middle place",
                        Note = "Middle place note",
                        CreateDataTime = new DateTime(2022, 12, 21),
                        Latitude = 44.07603843509156,
                        Longitude = 39.002465565338476,
                        IsActive = true
                    }
                }
            }
        };

        public static IEnumerable<UserWind> UserWinds => new List<UserWind>()
        {
            new UserWind()
            {
                Id = Guid.Parse("41e6095e-4efb-4f0e-b06e-c8bfa54e1ab0"),
                UserName = "Ivanov",
                NormalizedUserName = Strings.UCase("Ivanov"),
                Fio = "Ivanov Ivan Ivanich",
                Email = "ivanov@mail.ru",
                NormalizedEmail = Strings.UCase("ivanov@mail.ru"),
                Age = 31,
                About = "sportsmen",
                FotoFileName = "ivanov.jpg",
                IsActive = true,
                UserSpots = new List<UserSpot>()
                {
                    new UserSpot()
                    {
                        Id = Guid.Parse("3BBC2DE1-1EE0-4D50-AE4C-EA4444AE28C6"),
                        SpotId = Guid.Parse("4BCC4F3B-DB71-4442-A65B-556E2513700D"),
                        CreateDataTime = new DateTime(2022, 12, 22),
                        Comment = "Было здорово"
                    }
                }
            },
            new UserWind()
            {
                Id = Guid.Parse("493fbfb1-2608-4c39-be19-906dfa4edde7"), //2
                UserName = "Petrov",
                NormalizedUserName = Strings.UCase("Petrov"),
                Fio = "Petrov Petr Petrovich",
                Email = "petrov@mail.ru",
                NormalizedEmail = Strings.UCase("petrov@mail.ru"),
                Age = 55,
                About = "men",
                FotoFileName = null,
                IsActive = true,
                UserSpots = new List<UserSpot>()
                {
                    new UserSpot()
                    {
                        Id = Guid.Parse("D5EC202C-75E2-4506-96A7-33D35396F0E5"),
                        SpotId = Guid.Parse("F91DBEBE-04A8-46BA-877E-62EF624A77F4"),
                        CreateDataTime = new DateTime(2022, 12, 23),
                        Comment = "Поеду еще раз"
                    }
                }

            },
            new UserWind()
            {
                Id = Guid.Parse("48CD68E0-475C-44FB-BC3C-B40504B7CDA2"), //2
                UserName = "Sidorov",
                NormalizedUserName = Strings.UCase("Sidorov"),
                Fio = "Sidorov sidor Sidorovich",
                Email = "sidorov@mail.ru",
                NormalizedEmail = Strings.UCase("sidorov@mail.ru"),
                Age = 15,
                About = "men",
                FotoFileName = null,
                IsActive = true,
            }
        };
    }
}