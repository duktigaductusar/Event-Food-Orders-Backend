using Bogus;
using EventFoodOrders.Data;
using EventFoodOrders.Entities;
using EventFoodOrders.Utilities;

namespace EventFoodOrders.Mock;

public static class DBSeed
{
    private static readonly string[] allergies = [
        "Jordnötter",
        "Gluten",
        "Laktos",
        "Skaldjur",
        "Nötter",
        "Ägg",
        "Fisk",
        "Soja",
        "Selleri",
        "Sesamfrön"
    ];

    private static readonly string[] preferences = [
        "Vegan",
        "Vegetarian",
        "Halal",
        "Kosher",
        "Laktosfri",
        "Glutenfri",
        "Nötfri",
        "Fiskfri",
        "Fläskfri",
        "Extra stor portion"
    ];

    private static readonly string[] eventTitles = [
        "Kunskapsseminarie",
        "Kompetensstafett",
        "Sommarfest i parken",
        "Julbord med företaget",
        "Fika och bokcirkel",
        "Filmkväll under stjärnorna",
        "Matlagningstävling för vänner",
        "Höstmarknad i byn",
        "Workshop i digitalt skapande",
        "Quizkväll på puben",
        "Yoga och frukost i soluppgången",
        "Vårstädning och korvgrillning",
        "Föreläsning om klimatförändringar",
        "Musikjam för amatörer",
        "Skridskoåkning på sjön",
        "Brädspelskväll",
        "Utställning av lokala konstnärer",
        "Studentmiddag på nationen",
        "Loppis på torget",
        "Sportdag för hela familjen"
    ];

    private static readonly string[] eventDescripotions = [
        "Distribuerade applikationer med Spring Cloud och OpenStack",
        "Mikrotjänster och molnintegration med Spring Cloud",
        "En härlig kväll med musik, grill och gemenskap i stadsparken.",
        "Traditionellt svenskt julbord tillsammans med kollegorna.",
        "Vi diskuterar månadens bok över en kopp kaffe och kanelbullar.",
        "Utomhusbio med klassiska filmer och popcorn.",
        "Vem lagar den bästa pastan? Kom och tävla eller bara njut!",
        "Lokala hantverk, matstånd och ponnyridning för barnen.",
        "Lär dig grunderna i att skapa digital konst och animation.",
        "Samla ett lag och testa era kunskaper i olika kategorier.",
        "Starta dagen med avslappnande yoga och nyttig frukost.",
        "Hjälp till att städa gården – vi bjuder på korv!",
        "Lär dig mer om hur vi påverkar miljön och vad vi kan göra.",
        "Ta med ditt instrument och jamma med andra musikälskare.",
        "En rolig vinterdag med varm choklad och skridskoåkning.",
        "Testa nya och gamla brädspel med vänner och familj.",
        "Inspirerande bilder från vår stad och natur.",
        "Fira terminens slut med trerätters middag och dans.",
        "Hitta fynd och sälj det du inte längre behöver.",
        "Prova olika sporter tillsammans – kul för alla åldrar!"
    ];


    public static void Run(EventFoodOrdersDbContext context, IUserSeed userSeed)
    {
        if (context.Events.Any() || context.Participants.Any())
            return;

        var faker = new Faker("sv");

        List<SeedUser> seedUsers = new();

        for (int i = 0; i < userSeed.Users.Count; i++)
        {
            seedUsers.Add(new SeedUser(
                userSeed.Users[i].UserId,
                userSeed.Users[i].Username,
                allergies[faker.Random.Int(0, allergies.Length - 1)],
                preferences[faker.Random.Int(0, preferences.Length - 1)]
            ));
        }

        var eventFaker = new Faker<Event>()
            .CustomInstantiator(f =>
            {
                var ownerId = seedUsers.ElementAt(faker.Random.Int(0, seedUsers.Count - 1)).UserId;
                return new Event(ownerId)
                {
                    Title = f.PickRandom(eventTitles),
                    Description = f.PickRandom(eventDescripotions),
                    Date = f.Date.FutureOffset(2),
                    Deadline = f.Date.FutureOffset(1)
                };
            });

        var events = eventFaker.Generate(10);

        var participants = new List<Participant>();

        foreach (var ev in events)
        {
            var ownerUser = seedUsers.Find(u => u.UserId == ev.OwnerId);
            var ownerParticipant = new Participant(ev.OwnerId, ev.Id)
            {
                Name = ownerUser.Name,
                ResponseType = faker.PickRandom(Utility.PossibleResponses),
                WantsMeal = faker.Random.Bool(),
                Allergies = ownerUser.Allergies,
                Preferences = ownerUser.Preferences
            };

            participants.Add(ownerParticipant);

            var participantFaker = new Faker<Participant>()
                .CustomInstantiator(f =>
                {
                    SeedUser user = seedUsers.ElementAt(faker.Random.Int(0, seedUsers.Count - 1));
                    while(user.UserId == ev.OwnerId)
                    {
                        user = seedUsers.ElementAt(faker.Random.Int(0, seedUsers.Count - 1));
                    }

                    return new Participant(user.UserId, ev.Id)
                    {
                        Name = user.Name,
                        ResponseType = f.PickRandom(Utility.PossibleResponses),
                        WantsMeal = f.Random.Bool(),
                        Allergies = user.Allergies,
                        Preferences = user.Preferences
                    };
                });

            var generatedParticipants = participantFaker.Generate(faker.Random.Int(3, 10));
            participants.AddRange(generatedParticipants);
        }

        context.Events.AddRange(events);
        context.Participants.AddRange(participants);
        context.SaveChanges();
    }

    private class SeedUser(Guid userId, string name, string allergies, string preferences)
    {
        public Guid UserId { get; } = userId;
        public string Name { get; } = name;
        public string Allergies { get; } = allergies;
        public string Preferences { get; } = preferences;
    }
}
