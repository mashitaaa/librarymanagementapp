using System;

namespace LibraryManagementApp
{
    public class Buku
    {
        public string Kode { get; set; }
        public string Judul { get; set; }
        public string Pengarang { get; set; }
        public string Kategori { get; set; }
        public string Status { get; set; }
        public DateTime TanggalMasuk { get; set; }
    }
}
