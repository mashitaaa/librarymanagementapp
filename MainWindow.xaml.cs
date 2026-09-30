using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace LibraryManagementApp
{
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<Buku> daftarBuku = new ObservableCollection<Buku>();
        private readonly ICollectionView tampilan;

        public MainWindow()
        {
            InitializeComponent();

            tampilan = CollectionViewSource.GetDefaultView(daftarBuku);
            dgBuku.ItemsSource = tampilan;
            dpTanggal.SelectedDate = DateTime.Today;
            UpdateJumlah();
        }

        private void BtnTambah_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKode.Text))
            {
                MessageBox.Show("Kode buku harus diisi!");
                txtKode.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtJudul.Text))
            {
                MessageBox.Show("Judul buku harus diisi!");
                txtJudul.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPengarang.Text))
            {
                MessageBox.Show("Pengarang harus diisi!");
                txtPengarang.Focus();
                return;
            }

            if (cmbKategori.SelectedItem == null)
            {
                MessageBox.Show("Pilih kategori buku!");
                return;
            }

            if (dpTanggal.SelectedDate == null)
            {
                MessageBox.Show("Pilih tanggal masuk!");
                return;
            }

            if (rbTersedia.IsChecked != true && rbDipinjam.IsChecked != true)
            {
                MessageBox.Show("Pilih status buku!");
                return;
            }

            string kode = txtKode.Text.Trim();

            bool kodeSudahAda = daftarBuku.Any(b =>
                string.Equals(b.Kode, kode, StringComparison.OrdinalIgnoreCase));

            if (kodeSudahAda)
            {
                MessageBox.Show("Kode buku sudah terdaftar!");
                txtKode.Focus();
                return;
            }

            string kategori = "";
            if (cmbKategori.SelectedItem is ComboBoxItem item)
            {
                kategori = item.Content.ToString();
            }

            Buku bukuBaru = new Buku
            {
                Kode = kode,
                Judul = txtJudul.Text.Trim(),
                Pengarang = txtPengarang.Text.Trim(),
                Kategori = kategori,
                Status = rbTersedia.IsChecked == true ? "Tersedia" : "Dipinjam",
                TanggalMasuk = dpTanggal.SelectedDate.Value
            };

            daftarBuku.Add(bukuBaru);
            UpdateJumlah();

            MessageBox.Show(
                "Data buku berhasil ditambahkan!",
                "Informasi",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            BtnReset_Click(sender, e);
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            txtKode.Clear();
            txtJudul.Clear();
            txtPengarang.Clear();
            cmbKategori.SelectedIndex = -1;
            dpTanggal.SelectedDate = DateTime.Today;
            rbTersedia.IsChecked = false;
            rbDipinjam.IsChecked = false;
            txtKode.Focus();
        }

        private void BtnHapus_Click(object sender, RoutedEventArgs e)
        {
            Buku terpilih = dgBuku.SelectedItem as Buku;

            if (terpilih == null)
            {
                MessageBox.Show("Pilih data buku yang ingin dihapus!");
                return;
            }

            MessageBoxResult hasil = MessageBox.Show(
                "Yakin ingin menghapus buku \"" + terpilih.Judul + "\"?",
                "Konfirmasi",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (hasil == MessageBoxResult.Yes)
            {
                daftarBuku.Remove(terpilih);
                UpdateJumlah();
            }
        }

        private void TxtCari_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tampilan == null)
            {
                return;
            }

            string kata = txtCari.Text.Trim();

            if (kata.Length == 0)
            {
                tampilan.Filter = null;
            }
            else
            {
                tampilan.Filter = obj =>
                {
                    Buku b = (Buku)obj;
                    return b.Kode.IndexOf(kata, StringComparison.OrdinalIgnoreCase) >= 0
                        || b.Judul.IndexOf(kata, StringComparison.OrdinalIgnoreCase) >= 0
                        || b.Pengarang.IndexOf(kata, StringComparison.OrdinalIgnoreCase) >= 0;
                };
            }

            UpdateJumlah();
        }

        private void UpdateJumlah()
        {
            if (txtJumlah == null)
            {
                return;
            }

            int tampil = tampilan.Cast<object>().Count();
            txtJumlah.Text = $"Jumlah Buku: {daftarBuku.Count} (ditampilkan {tampil})";
        }
    }
}
