using Bogus;
using EventFoodOrders.Data;
using EventFoodOrders.Entities;
using EventFoodOrders.Utilities;

namespace EventFoodOrders.Seed;

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
        "Sesamfrön",
        "KunskapsSem",
        "KompetensStaffet"
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


    public static void Run(EventFoodOrdersDbContext context)
    {
        if (context.Events.Any() || context.Participants.Any())
            return;

        var faker = new Faker("sv");

        List<Guid> userIds = [];

        for (int i = 0; i < 50; i++)
        {
            userIds.Add(Guid.NewGuid());
        }

        var eventFaker = new Faker<Event>()
            .CustomInstantiator(f =>
            {
                var ownerId = userIds.ElementAt(faker.Random.Int(0, userIds.Count - 1));
                return new Event(ownerId)
                {
                    Title = f.PickRandom(eventTitles),
                    Description = f.PickRandom(eventDescripotions),
                    Date = f.Date.FutureOffset(2),
                    Deadline = f.Date.FutureOffset(1)
                };
            });

        var events = eventFaker.Generate(100);

        var participants = new List<Participant>();

        foreach (var ev in events)
        {
            var ownerParticipant = new Participant(ev.OwnerId, ev.Id)
            {
                Name = faker.Name.FullName(),
                ResponseType = faker.PickRandom(Utility.PossibleResponses),
                WantsMeal = faker.Random.Bool(),
                Allergies = allergies[faker.Random.Int(0, allergies.Length - 1)],
                Preferences = preferences[faker.Random.Int(0, preferences.Length - 1)]
            };

            participants.Add(ownerParticipant);

            var participantFaker = new Faker<Participant>()
                .CustomInstantiator(f =>
                {
                    var userId = userIds.ElementAt(faker.Random.Int(0, userIds.Count - 1));
                    return new Participant(userId, ev.Id)
                    {
                        Name = f.Name.FullName(),
                        ResponseType = f.PickRandom(Utility.PossibleResponses),
                        WantsMeal = f.Random.Bool(),
                        Allergies = allergies[faker.Random.Int(0, allergies.Length - 1)],
                        Preferences = preferences[faker.Random.Int(0, preferences.Length - 1)]
                    };
                });

            var generatedParticipants = participantFaker.Generate(faker.Random.Int(3, 40));
            participants.AddRange(generatedParticipants);
        }

        context.Events.AddRange(events);
        context.Participants.AddRange(participants);
        context.SaveChanges();
    }
}
