namespace PJATK_APBD_Cw8_s29769.DTO;

public class RoomDto
{
    public string Id { get; set; } = null!;
    public bool HasTv { get; set; }
    public WardDto Ward { get; set; } = null!;
}