using Hotels.Domain.Enums;
using Hotels.Presentation.DtoBs.Common;
using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.DtoBs.Subobjects;

public class SubobjectDtoB : ApplicationBaseEntityDtoB
{
    [Required, Length(3, 128)]
    public override required string Name { get; set; }

    [Required, MaxLength(5000)]
    public required string Description { get; set; }

    /// <summary>
    /// Площадь, м2
    /// </summary>
    [Required, Range(0, float.MaxValue)]
    public float Square { get; set; }

    [Required, Range(0, int.MaxValue)]
    public int MinDaysForOrder { get; set; }

    /// <summary>
    /// Цена за дополнительное место
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal? ExtraPlacePrice { get; set; }

    /// <summary>
    /// Количество спальных мест у подобъекта
    /// </summary>
    [Required, Range(1, int.MaxValue)]
    public int BedCount { get; set; }

    [Required, Range(1, int.MaxValue)]
    public int BedroomCount { get; set; }

    /// <summary>
    /// Ориентировочные цены для разных сезонов { 0: summer; 1: summer winter } (см. Season)
    /// </summary>
    [Length(2, 2)]
    public decimal?[] SeasonPrices { get; set; } = new decimal?[2];

    /// <summary>
    /// Сезон, в который подобъект доступен
    /// </summary>
    [Required] public SubobjectSeason Season { get; set; } = SubobjectSeason.Summer;

    // ===

    [Required] public required string PartnerId { get; set; }
    public Guid? TypeId { get; set; }
    public Guid? WeekRateId { get; set; }
}
