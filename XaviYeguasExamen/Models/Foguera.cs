using System.ComponentModel.DataAnnotations;

namespace Cendres;

public class Foguera
{
    public int FogueraID { get; set; }
    
    [Required]
    [MaxLength(60)]
    public string Nom { get; set; }
    public int Descansos { get; set; }
    
    public virtual Zona Zona { get; set; }

    public override string ToString()
    {
        return $"{nameof(FogueraID)}: {FogueraID}, {nameof(Nom)}: {Nom}, {nameof(Descansos)}: {Descansos}";
    }

    public Foguera()
    {
    }
}
