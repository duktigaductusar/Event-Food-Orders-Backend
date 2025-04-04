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

        Guid angeliki = Guid.Parse("e0d301f3-2943-423c-95a5-ee02cfa4be29");
        Guid riki = Guid.Parse("f9a25aaf-7081-45c1-ae05-3ac38c2e9073");
        Guid joel = Guid.Parse("c90cbace-5a82-43bf-b8f2-9ea64690a689");
        Guid daniel = Guid.Parse("8ef89f5a-c315-470c-9f52-2e6a06dd1197");

        Guid[] seededUsers = [angeliki, riki, joel, daniel];

        string[] bools = ["true", "false"];

        List<SeedUser> seedUsers = [];

        for (int i = 0; i < userSeed.Users.Count; i++)
        {
            seedUsers.Add(new SeedUser(
                userSeed.Users[i].UserId,
                userSeed.Users[i].Username,
                allergies[faker.Random.Int(0, allergies.Length - 1)],
                preferences[faker.Random.Int(0, preferences.Length - 1)]
            ));
        }

        int numberOfEvents = 0;

        var eventFaker = new Faker<Event>()
            .CustomInstantiator(f =>
            {
                Guid ownerId = seededUsers[numberOfEvents % seededUsers.Length];
                numberOfEvents++;
                    
                var e = new Event(ownerId)
                {
                    Title = f.PickRandom(eventTitles),
                    Description = f.PickRandom(eventDescripotions),
                    Date = f.Date.FutureOffset(2),
                    Deadline = f.Date.FutureOffset(1)
                };

                return e;
            });

        List<Event> events = eventFaker.Generate(12);

        List<Participant> participants = [];

        foreach (var ev in events)
        {
            List<Participant> seedParticipants = [];
            for (int i = 0; i < seededUsers.Length; i++)
            {
                var rand = new Random();
                Guid uId = seededUsers[(numberOfEvents + i) % seededUsers.Length];
                SeedUser user = seedUsers.Find(u => u.UserId == uId)!;
                seedParticipants.Add(new Participant(uId, ev.Id)
                {
                    Name = user.Name,
                    ResponseType = Utility.PossibleResponses[rand.Next(Utility.PossibleResponses.Length)],
                    WantsMeal = bool.Parse(bools[rand.Next(2)]),
                    Allergies = user.Allergies,
                    Preferences = user.Preferences
                });
            }

            participants.AddRange(seedParticipants);

            var participantFaker = new Faker<Participant>()
                .CustomInstantiator(f =>
                {
                    SeedUser user = seedUsers.Find(u => u.UserId == ev.OwnerId)!;
                    while (seedParticipants.Where(p => p.UserId == user.UserId).Count() > 0)
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

            var generatedParticipants = participantFaker.Generate(faker.Random.Int(6, 12));
            participants.AddRange(generatedParticipants);
        }

        context.Events.AddRange(events);
        context.Participants.AddRange(participants);
        context.SaveChanges();
    }

    internal class SeedUser(Guid userId, string name, string allergies, string preferences)
    {
        public Guid UserId { get; } = userId;
        public string Name { get; } = name;
        public string Allergies { get; } = allergies;
        public string Preferences { get; } = preferences;
    }
}
