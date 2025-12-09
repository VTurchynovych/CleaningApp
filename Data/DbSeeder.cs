using CleaningApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CleaningApp.Data
{
    public class DbSeeder
    {
        private readonly ApplicationDbContext _context;

        public DbSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Zabezpieczenie: jeśli są już usługi, nie dodajemy duplikatów
            if (await _context.Services.AnyAsync()) return;

            // Domyślne zdjęcie dla wszystkich (do późniejszej podmiany w panelu Admina)
            string defaultImage = "/img/hero-bg.jpg";

            var services = new List<Service>
            {
                // --- KATEGORIA: MIESZKANIA STANDARD ---
                new Service {
                    Name = "Pakiet Start: Kawalerka",
                    DefaultPrice = 149.00m,
                    EstimatedTime = "ok. 2h",
                    AreaRange = "do 35 m²",
                    Includes = "Odkurzanie, mycie podłóg, kurze, dezynfekcja łazienki",
                    Description = "Idealne rozwiązanie dla singla. Szybkie i skuteczne odświeżenie małego mieszkania.",
                    ImageUrl = defaultImage
                },
                new Service {
                    Name = "Standard: Mieszkanie 2-pokojowe",
                    DefaultPrice = 199.00m,
                    EstimatedTime = "ok. 3h",
                    AreaRange = "36 - 50 m²",
                    Includes = "Kompleksowe sprzątanie, mycie frontów kuchennych, armatura",
                    Description = "Najpopularniejszy wybór. Przywracamy blask Twojemu mieszkaniu, gdy Ty odpoczywasz.",
                    ImageUrl = defaultImage
                },
                new Service {
                    Name = "Standard: Apartament Rodzinny",
                    DefaultPrice = 289.00m,
                    EstimatedTime = "ok. 4-5h",
                    AreaRange = "51 - 80 m²",
                    Includes = "Wszystkie pokoje, kuchnia, łazienka + toaleta, listwy przypodłogowe",
                    Description = "Dokładne sprzątanie dla wymagających rodzin. Dbamy o każdy kąt, w którym bawią się dzieci.",
                    ImageUrl = defaultImage
                },
                new Service {
                    Name = "Standard: Duży Apartament",
                    DefaultPrice = 349.00m,
                    EstimatedTime = "ok. 5-6h",
                    AreaRange = "81 - 120 m²",
                    Includes = "Pełen zakres standard, odkurzanie mebli tapicerowanych, drzwi",
                    Description = "Przestrzeń wymaga czasu. Nasi specjaliści zadbają o każdy metr Twojego apartamentu.",
                    ImageUrl = defaultImage
                },

                // --- KATEGORIA: DOMY I REZYDENCJE ---
                new Service {
                    Name = "Dom: Pakiet Podstawowy",
                    DefaultPrice = 450.00m,
                    EstimatedTime = "ok. 6h",
                    AreaRange = "do 150 m²",
                    Includes = "Parter + Piętro: odkurzanie, mycie, łazienki, kuchnia z zewnątrz",
                    Description = "Regularne utrzymanie czystości w Twoim domu. Poczuj świeżość od progu.",
                    ImageUrl = defaultImage
                },
                new Service {
                    Name = "Dom: Sprzątanie Generalne",
                    DefaultPrice = 890.00m,
                    EstimatedTime = "cały dzień (8h+)",
                    AreaRange = "do 200 m²",
                    Includes = "Mycie okien, wnętrza szafek, fugi, oświetlenie, sprzęt AGD",
                    Description = "Głębokie czyszczenie każdego zakamarka. Usługa premium przywracająca stan 'jak nowy'.",
                    ImageUrl = defaultImage
                },
                new Service {
                    Name = "Rezydencja: Premium Care",
                    DefaultPrice = 1499.00m,
                    EstimatedTime = "2 dni lub zespół 2-os",
                    AreaRange = "powyżej 250 m²",
                    Includes = "Pełna obsługa VIP, polerowanie podłóg, pranie dywanów, mycie okien",
                    Description = "Ekskluzywna usługa dla największych posiadłości. Dyskrecja i najwyższa jakość gwarantowana.",
                    ImageUrl = defaultImage
                },

                // --- KATEGORIA: BIURA I FIRMY ---
                new Service {
                    Name = "Biuro: Mały Zespół",
                    DefaultPrice = 120.00m,
                    EstimatedTime = "ok. 1.5h",
                    AreaRange = "do 40 m²",
                    Includes = "Opróżnianie koszy, biurka, aneks kuchenny, toaleta",
                    Description = "Szybki serwis przed otwarciem biura. Czyste biurka sprzyjają efektywności.",
                    ImageUrl = defaultImage
                },
                new Service {
                    Name = "Biuro: Open Space",
                    DefaultPrice = 299.00m,
                    EstimatedTime = "ok. 3h",
                    AreaRange = "do 100 m²",
                    Includes = "Odkurzanie wykładzin, dezynfekcja klamek i włączników, kuchnia",
                    Description = "Zadbaj o zdrowie pracowników. Profesjonalna dezynfekcja i czystość w miejscu pracy.",
                    ImageUrl = defaultImage
                },
                new Service {
                    Name = "Sala Konferencyjna: Ekspres",
                    DefaultPrice = 89.00m,
                    EstimatedTime = "45 min",
                    AreaRange = "1 pomieszczenie",
                    Includes = "Przecieranie stołów, odkurzanie, wietrzenie, przygotowanie do spotkania",
                    Description = "Błyskawiczne przygotowanie sali przed ważnymi negocjacjami.",
                    ImageUrl = defaultImage
                },

                // --- KATEGORIA: PRANIE TAPICERKI (PAGO) ---
                new Service {
                    Name = "Pranie: Sofa 2-osobowa",
                    DefaultPrice = 140.00m,
                    EstimatedTime = "ok. 1h",
                    AreaRange = "1 mebel",
                    Includes = "Odkurzanie, odplamianie, pranie ekstrakcyjne Karcher",
                    Description = "Usunięcie plam, roztoczy i nieprzyjemnych zapachów. Twoja sofa odzyska dawny kolor.",
                    ImageUrl = defaultImage
                },
                new Service {
                    Name = "Pranie: Narożnik L",
                    DefaultPrice = 220.00m,
                    EstimatedTime = "ok. 1.5h",
                    AreaRange = "Standard L",
                    Includes = "Pranie siedzisk, oparć, boczków i funkcji spania",
                    Description = "Dogłębne czyszczenie ulubionego miejsca wypoczynku całej rodziny.",
                    ImageUrl = defaultImage
                },
                new Service {
                    Name = "Pranie: Narożnik U (XXL)",
                    DefaultPrice = 290.00m,
                    EstimatedTime = "ok. 2h",
                    AreaRange = "Duży narożnik",
                    Includes = "Kompleksowe pranie dużych mebli wypoczynkowych",
                    Description = "Dla dużych mebli wymagających dużej uwagi. Efekt świeżości gwarantowany.",
                    ImageUrl = defaultImage
                },
                new Service {
                    Name = "Pranie: Fotel / Krzesło",
                    DefaultPrice = 60.00m,
                    EstimatedTime = "ok. 30 min",
                    AreaRange = "1 sztuka",
                    Includes = "Pranie tapicerki meblowej",
                    Description = "Odświeżenie ulubionego fotela. Usuwamy ślady po kawie i codzinnym użytkowaniu.",
                    ImageUrl = defaultImage
                },
                new Service {
                    Name = "Pranie: Wykładzina Biurowa",
                    DefaultPrice = 15.00m,
                    EstimatedTime = "zależnie od m²",
                    AreaRange = "cena za m²",
                    Includes = "Pranie maszynowe dużych powierzchni",
                    Description = "Cena za m2. Przemysłowe czyszczenie wykładzin w biurach i korytarzach.",
                    ImageUrl = defaultImage
                },

                // --- KATEGORIA: MYCIE OKIEN ---
                new Service {
                    Name = "Okna: Pakiet Standard",
                    DefaultPrice = 99.00m,
                    EstimatedTime = "ok. 1.5h",
                    AreaRange = "do 5 okien",
                    Includes = "Mycie szyb, ram wewnątrz i parapetów",
                    Description = "Wpuść więcej słońca do domu! Krystalicznie czyste szyby bez smug.",
                    ImageUrl = defaultImage
                },
                new Service {
                    Name = "Okna: Pakiet Dom",
                    DefaultPrice = 199.00m,
                    EstimatedTime = "ok. 3h",
                    AreaRange = "do 10 okien",
                    Includes = "Mycie szyb obustronne, ramy, parapety",
                    Description = "Kompleksowe mycie stolarki okiennej w całym domu jednorodzinnym.",
                    ImageUrl = defaultImage
                },
                new Service {
                    Name = "Okna: Po Remoncie",
                    DefaultPrice = 299.00m,
                    EstimatedTime = "ok. 4h",
                    AreaRange = "do 8 okien",
                    Includes = "Usuwanie farby, kleju, taśm, gruntowne czyszczenie",
                    Description = "Specjalistyczne usuwanie trudnych zabrudzeń budowlanych. Bezpieczne dla szyb.",
                    ImageUrl = defaultImage
                },
                new Service {
                    Name = "Witryna Sklepowa",
                    DefaultPrice = 80.00m,
                    EstimatedTime = "ok. 1h",
                    AreaRange = "do 10 m²",
                    Includes = "Mycie przeszkleń zewnętrznych i wewnętrznych",
                    Description = "Czysta witryna to wizytówka Twojego biznesu. Zadbaj o pierwsze wrażenie.",
                    ImageUrl = defaultImage
                },

                // --- KATEGORIA: PO REMONCIE ---
                new Service {
                    Name = "Sprzątanie Poremontowe: Małe",
                    DefaultPrice = 399.00m,
                    EstimatedTime = "ok. 5h",
                    AreaRange = "do 40 m²",
                    Includes = "Odpylanie ścian, mycie podłóg, usuwanie resztek zaprawy",
                    Description = "Pozbędziemy się wszechobecnego pyłu. Przygotujemy mieszkanie do wprowadzenia się.",
                    ImageUrl = defaultImage
                },
                new Service {
                    Name = "Sprzątanie Poremontowe: Duże",
                    DefaultPrice = 799.00m,
                    EstimatedTime = "8h+",
                    AreaRange = "do 80 m²",
                    Includes = "Odpylanie sufitów, ścian, mycie okien, doczyszczanie posadzek",
                    Description = "Generalne porządki po ekipie budowlanej. Zamień plac budowy w przytulny dom.",
                    ImageUrl = defaultImage
                },

                // --- KATEGORIA: ECO I SPECJALNE ---
                new Service {
                    Name = "ECO Sprzątanie (Alergik)",
                    DefaultPrice = 249.00m,
                    EstimatedTime = "ok. 3h",
                    AreaRange = "do 50 m²",
                    Includes = "Użycie wyłącznie pary wodnej i środków 100% naturalnych",
                    Description = "Bez chemii. Idealne dla alergików, małych dzieci i miłośników ekologii.",
                    ImageUrl = defaultImage
                },
                new Service {
                    Name = "Ozonowanie Pomieszczeń",
                    DefaultPrice = 150.00m,
                    EstimatedTime = "1h + wietrzenie",
                    AreaRange = "do 60 m²",
                    Includes = "Generator ozonu przemysłowy",
                    Description = "Usuwa wirusy, bakterie i nieprzyjemne zapachy (np. dymu papierosowego).",
                    ImageUrl = defaultImage
                },
                new Service {
                    Name = "Mycie Piekarnika / Lodówki",
                    DefaultPrice = 70.00m,
                    EstimatedTime = "ok. 45 min",
                    AreaRange = "1 urządzenie",
                    Includes = "Rozkręcenie, namaczanie, usuwanie tłuszczu",
                    Description = "Dodatek do sprzątania. Przywracamy blask Twojemu sprzętowi AGD.",
                    ImageUrl = defaultImage
                },
                new Service {
                    Name = "Mycie Tarasu / Balkonu",
                    DefaultPrice = 120.00m,
                    EstimatedTime = "ok. 1h",
                    AreaRange = "do 15 m²",
                    Includes = "Mycie ręczne lub myjką ciśnieniową (zależnie od posadzki)",
                    Description = "Przygotuj swój balkon na sezon letni. Usuwamy mech i osad miejski.",
                    ImageUrl = defaultImage
                },
                new Service {
                    Name = "Prasowanie (Pakiet)",
                    DefaultPrice = 100.00m,
                    EstimatedTime = "ok. 2h",
                    AreaRange = "Kosz ubrań",
                    Includes = "Prasowanie koszul, spodni, pościeli u klienta",
                    Description = "Nie lubisz prasować? Zrobimy to za Ciebie profesjonalną stacją parową.",
                    ImageUrl = defaultImage
                },
                new Service {
                    Name = "Sprzątanie Nagrobków",
                    DefaultPrice = 90.00m,
                    EstimatedTime = "ok. 1h",
                    AreaRange = "1 nagrobek",
                    Includes = "Mycie, polerowanie, uprzątnięcie starych zniczy",
                    Description = "Zadbamy o miejsce pamięci Twoich bliskich z należytym szacunkiem.",
                    ImageUrl = defaultImage
                },
                new Service {
                    Name = "Mycie Elewacji (Parter)",
                    DefaultPrice = 350.00m,
                    EstimatedTime = "ok. 3h",
                    AreaRange = "jedna ściana",
                    Includes = "Mycie ciśnieniowe Karcher",
                    Description = "Usuwanie glonów i zabrudzeń atmosferycznych z tynku.",
                    ImageUrl = defaultImage
                },
                new Service {
                    Name = "Sprzątanie Przed Sprzedażą",
                    DefaultPrice = 599.00m,
                    EstimatedTime = "ok. 5h",
                    AreaRange = "całe mieszkanie",
                    Includes = "Home Staging (porządek), mycie okien, zapach",
                    Description = "Zwiększ wartość nieruchomości. Czyste mieszkanie sprzedaje się szybciej i drożej.",
                    ImageUrl = defaultImage
                },
                new Service {
                    Name = "Abonament: 4x w miesiącu",
                    DefaultPrice = 700.00m,
                    EstimatedTime = "4 wizyty po 3h",
                    AreaRange = "do 50 m²",
                    Includes = "Regularne wizyty co tydzień w niższej cenie",
                    Description = "Zapomnij o sprzątaniu na stałe. Stała Pani sprzątająca i gwarancja terminu.",
                    ImageUrl = defaultImage
                }
            };

            await _context.Services.AddRangeAsync(services);
            await _context.SaveChangesAsync();
        }
    }
}