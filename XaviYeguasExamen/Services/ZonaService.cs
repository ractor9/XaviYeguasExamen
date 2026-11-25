using Cendres;

namespace XaviYeguasExamen.Services;

public class ZonaService
{
    private CendresContext _context;

    public ZonaService(CendresContext context)
    {
        _context = context;
    }
    
    public List<Zona> GetTotes()
    {
        List<Zona> llista = _context.Zona
            .OrderBy(a => a.ZonaID)
            .ToList();
        return llista;
    }

    public List<Zona> GetPerExplorar(int nivellMaxim)
    {
        List<Zona> llista = _context.Zona
            .Where(a => a.Explorada == false && a.NivellRecomanat <= nivellMaxim)
            .OrderBy(zona => zona.NivellRecomanat)
            .ToList();
        return llista;
    }

    public void MostraZona(int idZona)
    {
        Zona zone = _context.Zona.FirstOrDefault(zona => zona.ZonaID == idZona);
        if (zone == null)
        {
            Console.WriteLine($"No es troba la zona amb id {idZona}");
            return;
        }
        Console.WriteLine($"Zona: {zone.Nom} ({zone.Bioma}) - {zone.Foguera.Nom}");
        Console.WriteLine($"Subzones:");
        foreach (var s in zone.Subzones)
        {
            Console.WriteLine($"-{s.Nom} ({s.Bioma}) - {s.Foguera.Nom}");
        }
        
    }

    public bool AssignarRegio(int idZona, int idRegio)
    {
        Zona? z = _context.Zona.FirstOrDefault(zona => zona.ZonaID == idZona);
        Zona? r = _context.Zona.FirstOrDefault(zona => zona.ZonaID == idRegio);
        if (z == null || r == null)
        {
            return false;
        }
        if (z == r)
        {
            return false;
        }
        if (z.Regio == r)
        {
            return false;
        }

        z.Regio = r;

        _context.SaveChanges(); 

        return true;
    }

    public List<Zona> ZonesPendents()
    {
        return _context.Zona.Where(
            z => z.Explorada == false || z.Subzones.Any(zone => zone.Explorada == false)
        ).ToList();
    }

    public void ResumBiomes(int secretsMinims)
    {
        var biomes = _context.Zona
            .GroupBy(z => z.Bioma)
            .Select(g => new {
                Name = g.Key,
                TotalZones = g.Count(), // counts the number of candidates in the group
                SumaSecrets = g.Sum(z => z.Secrets)
            })
            .Where(g => g.SumaSecrets > secretsMinims) // condition on the accumulated votes
            .OrderByDescending(g => g.SumaSecrets)
            .ToList();

        Console.WriteLine($"Resum biomes: {string.Join(", ", biomes)}");
    }
}