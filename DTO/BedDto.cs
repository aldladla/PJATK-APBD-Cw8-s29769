namespace PJATK_APBD_Cw8_s29769.DTO;

public class BedDto
{
    public int Id { get; set; }
    public BedTypeDto BedType { get; set; } = null!;
    public RoomDto Room { get; set; } = null!;
}