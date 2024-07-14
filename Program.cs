using System;
using System.IO;

class DosyaYoneticisi
{
    public static void Main(string[] args)
    {
        // Kullanıcıdan komut almak için bir değişken tanımlıyoruz
        string komut = string.Empty;

        // Kullanıcı 'exit' yazana kadar döngü devam eder
        while (komut != "exit")
        {
            Console.WriteLine("Komut Girin: (listele/yap/sil/)");
            // Kullanıcıdan komut alıyoruz
            komut = Console.ReadLine();

            // Kullanıcının girdiği komuta göre uygun metodu çağırıyoruz
            switch (komut.ToLower())
            {
                case "listele":
                    // Dosyaları listeleme metodu çağırılıyor
                    DosyalariListele();
                    break;
                case "yap":
                    // Dosya oluşturma metodu çağırılıyor
                    DosyaOlustur();
                    break;
                case "sil":
                    // Dosya silme metodu çağırılıyor
                    DosyaSil();
                    break;
                default:
                    // Geçersiz komut girildiğinde uyarı veriyoruz
                    Console.WriteLine("Geçersiz komut.");
                    break;
            }
        }
    }

    // Bu metod mevcut dizindeki dosyaları listeler
    static void DosyalariListele()
    {
        Console.WriteLine("Listelenen Dosyalar:");
        // Mevcut dizindeki tüm dosyaları alıyoruz
        foreach (var dosya in Directory.GetFiles("."))
        {
            // Her dosyayı ekrana yazdırıyoruz
            Console.WriteLine(dosya);
        }
    }

    // Bu metod yeni bir dosya oluşturur
    static void DosyaOlustur()
    {
        // Kullanıcıdan dosya adı alıyoruz
        Console.WriteLine("Dosya Adı Girin:");
        string dosyaAdi = Console.ReadLine();
        // Belirtilen isimle yeni bir dosya oluşturuyoruz ve kapatıyoruz
        File.Create(dosyaAdi).Close();
        // Dosyanın oluşturulduğunu ekrana yazdırıyoruz
        Console.WriteLine($"{dosyaAdi} oluşturuldu.");
    }

    // Bu metod belirtilen dosyayı siler
    static void DosyaSil()
    {
        // Kullanıcıdan silinecek dosyanın adını alıyoruz
        Console.WriteLine("Silinecek Dosya Adı Girin:");
        string dosyaAdi = Console.ReadLine();
        // Belirtilen dosyayı siliyoruz
        File.Delete(dosyaAdi);
        // Dosyanın silindiğini ekrana yazdırıyoruz
        Console.WriteLine($"{dosyaAdi} silindi.");
    }
}
