using System.Text;

namespace Rememory.Persistance.Models;

public class AddressSettings
{
    public string? Region { get; set; }
    public string? District { get; set; }
    public string? Town { get; set; }
    public string? Street { get; set; }
    public string? House { get; set; }
    public string? Building { get; set; }
    public string? Flat { get; set; }

    public string ToHtml()
    {
        return $"<p>Регион: {Region}</p>" +
               $"<p>Район: {District}</p>" +
               $"<p>Город: {Town}</p>" +
               $"<p>Улица: {Street}</p>" +
               $"<p>Дом: {House}</p>" +
               $"<p>Корпус: {Building}</p>" +
               $"<p>Квартира: {Flat}</p>";
    }
}