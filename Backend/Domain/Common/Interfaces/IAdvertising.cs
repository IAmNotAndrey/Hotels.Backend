namespace Hotels.Domain.Common.Interfaces;

public interface IAdvertising
{
    /// <summary>
    /// Будет ли в промо-ряде
    /// </summary>
    public bool IsPromoSeries { get; set; }

    /// <summary>
    /// Будет ли высвечиваться, как реклама
    /// </summary>
    public bool IsAdvertised { get; set; }
}
