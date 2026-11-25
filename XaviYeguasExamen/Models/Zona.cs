using System.ComponentModel.DataAnnotations;

namespace Cendres;

public class Zona
{
    public int ZonaID { get; set; }
    
    [Required]
    [MaxLength(60)]
    public string Nom { get; set; }
    
    public Bioma Bioma { get; set; }
    
    [Range(1, 150)]
    public int NivellRecomanat { get; set; }
    
    public bool Explorada { get; set; }
    
    public int Secrets { get; set; }
    
    public virtual Zona? Regio { get; set; }
    
    public virtual ICollection<Zona> Subzones { get; set; }
    
    [Required]
    public virtual Foguera Foguera { get; set; }

    public override string ToString()
    {
        return
            $"{nameof(ZonaID)}: {ZonaID}, {nameof(Nom)}: {Nom}, {nameof(Bioma)}: {Bioma}, {nameof(NivellRecomanat)}: {NivellRecomanat}, {nameof(Explorada)}: {Explorada}, {nameof(Secrets)}: {Secrets}";
    }

    public Zona()
    {
        Subzones = new HashSet<Zona>();
    }
}