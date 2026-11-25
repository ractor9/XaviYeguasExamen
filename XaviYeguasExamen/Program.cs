using Cendres;
using XaviYeguasExamen.Services;

using var db = new CendresContext();
db.Database.EnsureDeleted();
db.Database.EnsureCreated();

// ============ EX2 · Dades ============
// Cada zona porta la seva foguera: no cal fer db.Foguera.Add, EF segueix la navegació.
// Les regions s'assignen DESPRÉS del primer SaveChanges. Si es posa Regio = z1 a l'inicialitzador i es desa tot de cop,
// EF insereix primer les zones sense regió (1, 4, 6) i després les subzones: els ids no coincideixen amb l'enunciat
Zona z1 = new Zona() { Nom = "Vall Cendrosa", Bioma = Bioma.BOSC, NivellRecomanat = 10, Explorada = true, Secrets = 4,
    Foguera = new Foguera() { Nom = "Foguera de l'Entrada", Descansos = 12 } };
Zona z2 = new Zona() { Nom = "Bosc dels Murmuris", Bioma = Bioma.BOSC, NivellRecomanat = 15, Explorada = true, Secrets = 6,
    Foguera = new Foguera() { Nom = "Foguera del Roure", Descansos = 5 } };
Zona z3 = new Zona() { Nom = "Ruïnes de Kael", Bioma = Bioma.CIUTAT, NivellRecomanat = 22, Explorada = false, Secrets = 2,
    Foguera = new Foguera() { Nom = "Foguera de la Plaça", Descansos = 3 } };
Zona z4 = new Zona() { Nom = "Cims Glaçats", Bioma = Bioma.NEU, NivellRecomanat = 40, Explorada = false, Secrets = 1,
    Foguera = new Foguera() { Nom = "Foguera del Cim", Descansos = 1 } };
Zona z5 = new Zona() { Nom = "Cova del Glaç", Bioma = Bioma.NEU, NivellRecomanat = 45, Explorada = true, Secrets = 3,
    Foguera = new Foguera() { Nom = "Foguera de la Cova", Descansos = 2 } };
Zona z6 = new Zona() { Nom = "Cràter Roent", Bioma = Bioma.VOLCA, NivellRecomanat = 60, Explorada = false, Secrets = 0,
    Foguera = new Foguera() { Nom = "Foguera de Cendra", Descansos = 0 } };

db.Zona.AddRange(z1, z2, z3, z4, z5, z6);
db.SaveChanges();

// La regió és l'objecte, no l'id. La columna RegioId l'omple EF en desar.
// També val: z1.Subzones.Add(z2);
z2.Regio = z1;
z3.Regio = z1;
z5.Regio = z4;
db.SaveChanges();

ZonaService service = new ZonaService(db);

Console.WriteLine("--- Totes les zones ---");
foreach (Zona z in service.GetTotes())
{
    Console.WriteLine($"{z.ZonaID}. {z} · nivell {z.NivellRecomanat} · {z.Secrets} secrets");
}

// ============ EX3 ============
// 40 ha de treure Ruïnes de Kael (22) i Cims Glaçats (40): el 40 hi entra. 20 no en treu cap
Console.WriteLine("\n--- EX3 · Per explorar fins al nivell 40 ---");
foreach (Zona z in service.GetPerExplorar(40))
{
    Console.WriteLine($"{z} (nivell {z.NivellRecomanat})");
}
Console.WriteLine("\n--- EX3 · Per explorar fins al nivell 20 (cap) ---");
foreach (Zona z in service.GetPerExplorar(20))
{
    Console.WriteLine($"{z} (nivell {z.NivellRecomanat})");
}

// ============ EX4 ============
Console.WriteLine("\n--- EX4 · Zona 1 ---");
service.MostraZona(1);
Console.WriteLine("\n--- EX4 · Zona 6 (sense subzones) ---");
service.MostraZona(6);
Console.WriteLine("\n--- EX4 · Una que no existeix ---");
service.MostraZona(99);

// ============ EX6 ============
// Abans de l'EX5, que canvia les dades. Ha de treure l'1 (per la 3), la 3, la 4 i la 6
Console.WriteLine("\n--- EX6 · Zones pendents ---");
foreach (Zona z in service.ZonesPendents())
{
    Console.WriteLine($"{z.ZonaID}. {z}");
}

// ============ EX7 ============
// També abans de l'EX5. Amb 3: BOSC 10 i NEU 4. CIUTAT (2) i VOLCA (0) no surten
Console.WriteLine("\n--- EX7 · Resum de biomes (3) amb GroupBy ---");
service.ResumBiomes(3);
Console.WriteLine("\n--- EX7 · Resum de biomes (10): BOSC té 10 justos i no surt ---");
service.ResumBiomes(10);

// ============ EX5 ============
Console.WriteLine("\n--- EX5 · Assignar regió ---");
Console.WriteLine($"6 → 4: {service.AssignarRegio(6, 4)}");     // true: el Cràter Roent passa a ser subzona de Cims Glaçats
Console.WriteLine($"6 → 4: {service.AssignarRegio(6, 4)}");     // false: ja era la seva regió
Console.WriteLine($"2 → 1: {service.AssignarRegio(2, 1)}");     // false: ja ho era a les dades
Console.WriteLine($"99 → 1: {service.AssignarRegio(99, 1)}");   // false: la zona 99 no existeix
Console.WriteLine($"1 → 99: {service.AssignarRegio(1, 99)}");   // false: la regió 99 no existeix
Console.WriteLine($"1 → 1: {service.AssignarRegio(1, 1)}");     // false: la mateixa zona
Console.WriteLine($"4 → 5: {service.AssignarRegio(4, 5)}");     // false: al revés (la 5 és subzona de la 4). Opcional
Console.WriteLine($"3 → 4: {service.AssignarRegio(3, 4)}");     // true: canvia de regió, de la 1 a la 4

// Es comprova llegint de la BD, no de les variables: la 4 ara té tres subzones i la 1 només una
Console.WriteLine("\n--- EX4 · Zona 4, després de l'EX5 ---");
service.MostraZona(4);
Console.WriteLine("\n--- EX4 · Zona 1, després de l'EX5 ---");
service.MostraZona(1);
