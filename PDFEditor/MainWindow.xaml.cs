using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IronPdf;

namespace PDFEditor
{
    public partial class MainWindow : Window
    {
        //private List<Bitmap> pagePDFInPng;// Массив в котором содержатся изображения всех листов документа
        private ImageSource[] pagePDFInPng;
        private PdfDocument document;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Кидаем в программу файл и выводим его.
        private void MainPanel_Drop(object sender, DragEventArgs e)
        {
            string[] path = (string[])e.Data.GetData(DataFormats.FileDrop);
            document = new PdfDocument(path[0]);
            pagePDFInPng = new ImageSource[document.PageCount];
            ConvertPDFtoImage(0);
        }

        // Конвертируем PDF в изображение
        private void ConvertPDFtoImage(int page)
        {
            Bitmap image = new Bitmap(document.PageToBitmap(page, 96));
            BitmapSource imgsourc = Imaging.CreateBitmapSourceFromHBitmap(image.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            PictureBox.Source = imgsourc;
            pagePDFInPng[page] = PictureBox.Source;
        }

        private int DPCPlus = 0;
        private int DPCMinus = 0;
        private int docPageCount = 0;

        // Переключение PDF по сраницам
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right || e.Key == Key.Left)
            {
                DPCPlus = docPageCount + 1 <= document.PageCount - 1 ? docPageCount + 1 : 0;
                DPCMinus = docPageCount - 1 >= 0 ? docPageCount - 1 : document.PageCount - 1;

                if (e.Key == Key.Right && pagePDFInPng[DPCPlus] != null)
                { PictureBox.Source = pagePDFInPng[DPCPlus]; docPageCount++; }
                else if (e.Key == Key.Left && pagePDFInPng[DPCMinus] != null)
                { PictureBox.Source = pagePDFInPng[DPCMinus]; docPageCount--; }


                if (e.Key == Key.Right && pagePDFInPng[DPCPlus] == null)
                { ConvertPDFtoImage(DPCPlus); docPageCount++; }
                else if (e.Key == Key.Left && pagePDFInPng[DPCMinus] == null)
                { ConvertPDFtoImage(DPCMinus); docPageCount--; }


                if (docPageCount < 0)
                    docPageCount = document.PageCount - 1;
                else if (docPageCount > document.PageCount - 1)
                    docPageCount = 0;
            }
        }
    }
}