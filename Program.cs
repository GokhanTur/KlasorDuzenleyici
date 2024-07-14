using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class DosyaYoneticisi
{
    // 32 bayt (256 bit) uzunluğunda bir anahtar kullanın
    private static readonly byte[] Key = Encoding.UTF8.GetBytes("your-32-byte-long-key-your-32-byte-long-");
    // 16 bayt (128 bit) uzunluğunda bir IV kullanın
    private static readonly byte[] IV = Encoding.UTF8.GetBytes("your-16-byte-iv--");

    public static void Main(string[] args)
    {
        string komut = string.Empty;

        while (komut != "exit")
        {
            Console.WriteLine("Komut Girin: (listele/yap/sil/sifrele/coz)");
            komut = Console.ReadLine();

            switch (komut.ToLower())
            {
                case "listele":
                    DosyalariListele();
                    break;
                case "yap":
                    DosyaOlustur();
                    break;
                case "sil":
                    DosyaSil();
                    break;
                case "sifrele":
                    DosyaSifrele();
                    break;
                case "coz":
                    DosyaCoz();
                    break;
                default:
                    Console.WriteLine("Geçersiz komut.");
                    break;
            }
        }
    }

    static void DosyalariListele()
    {
        Console.WriteLine("Listelenen Dosyalar:");
        foreach (var dosya in Directory.GetFiles("."))
        {
            Console.WriteLine(dosya);
        }
    }

    static void DosyaOlustur()
    {
        Console.WriteLine("Dosya Adı Girin:");
        string dosyaAdi = Console.ReadLine();
        File.Create(dosyaAdi).Close();
        Console.WriteLine($"{dosyaAdi} oluşturuldu.");
    }

    static void DosyaSil()
    {
        Console.WriteLine("Silinecek Dosya Adı Girin:");
        string dosyaAdi = Console.ReadLine();
        File.Delete(dosyaAdi);
        Console.WriteLine($"{dosyaAdi} silindi.");
    }

    static void DosyaSifrele()
    {
        Console.WriteLine("Şifrelenecek Dosya Adı Girin:");
        string dosyaAdi = Console.ReadLine();
        byte[] dosyaIcerigi = File.ReadAllBytes(dosyaAdi);
        byte[] sifrelenmisIcerik = Sifrele(dosyaIcerigi);
        File.WriteAllBytes(dosyaAdi, sifrelenmisIcerik);
        Console.WriteLine($"{dosyaAdi} şifrelendi.");
    }

    static void DosyaCoz()
    {
        Console.WriteLine("Çözülecek Dosya Adı Girin:");
        string dosyaAdi = Console.ReadLine();
        byte[] sifrelenmisIcerik = File.ReadAllBytes(dosyaAdi);
        byte[] cozulmusIcerik = Coz(sifrelenmisIcerik);
        File.WriteAllBytes(dosyaAdi, cozulmusIcerik);
        Console.WriteLine($"{dosyaAdi} çözüldü.");
    }

    static byte[] Sifrele(byte[] veri)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key.Take(32).ToArray(); // Anahtarı 32 bayt uzunluğunda yapıyoruz
            aesAlg.IV = IV.Take(16).ToArray(); // IV'yi 16 bayt uzunluğunda yapıyoruz

            ICryptoTransform sifreleyici = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msSifrele = new MemoryStream())
            {
                using (CryptoStream csSifrele = new CryptoStream(msSifrele, sifreleyici, CryptoStreamMode.Write))
                {
                    csSifrele.Write(veri, 0, veri.Length);
                    csSifrele.FlushFinalBlock();
                }
                return msSifrele.ToArray();
            }
        }
    }

    static byte[] Coz(byte[] sifreliVeri)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key.Take(32).ToArray(); // Anahtarı 32 bayt uzunluğunda yapıyoruz
            aesAlg.IV = IV.Take(16).ToArray(); // IV'yi 16 bayt uzunluğunda yapıyoruz

            ICryptoTransform cozuMek = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msCoz = new MemoryStream(sifreliVeri))
            {
                using (CryptoStream csCoz = new CryptoStream(msCoz, cozuMek, CryptoStreamMode.Read))
                {
                    byte[] cozulmusVeri = new byte[sifreliVeri.Length];
                    int desifrelenmisByteSayisi = csCoz.Read(cozulmusVeri, 0, cozulmusVeri.Length);
                    Array.Resize(ref cozulmusVeri, desifrelenmisByteSayisi);
                    return cozulmusVeri;
                }
            }
        }
    }
}
