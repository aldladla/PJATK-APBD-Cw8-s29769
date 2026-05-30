namespace PJATK_APBD_Cw8_s29769.DTO;

public class AdmissionDto
{
    public int Id { get; set; }
    public DateTime AdmissionDate { get; set; }
    public DateTime? DischargeDate { get; set; }
    public WardDto Ward { get; set; } = null!;
}