namespace Tufanlar.UI.Dtos;

public class KategoriDto
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;

    // Picker'da (Seçim Kutusu) sadece isim görünsün diye bu override şart:
    public override string ToString() => Ad;
}