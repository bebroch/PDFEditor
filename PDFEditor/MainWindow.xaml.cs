using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
            allPages.Content = document.PageCount;
            nowPage.Content = 1;
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

        // Кнопка переключения страницы вперёд
        private void Button_Click_Next(object sender, RoutedEventArgs e)
        {
            KeyDown(Key.Right);
        }

        // Кнопка переключения страницы назад
        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            KeyDown(Key.Left);
        }

        // Переключение PDF по сраницам
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDown(e.Key);
        }

        private new void KeyDown(Key key)
        {
            if (document != null && (key == Key.Right || key == Key.Left))
            {
                DPCPlus = docPageCount + 1 <= document.PageCount - 1 ? docPageCount + 1 : 0;
                DPCMinus = docPageCount - 1 >= 0 ? docPageCount - 1 : document.PageCount - 1;

                if (key == Key.Right && pagePDFInPng[DPCPlus] != null)
                { PictureBox.Source = pagePDFInPng[DPCPlus]; docPageCount++; }
                else if (key == Key.Left && pagePDFInPng[DPCMinus] != null)
                { PictureBox.Source = pagePDFInPng[DPCMinus]; docPageCount--; }

                if (key == Key.Right && pagePDFInPng[DPCPlus] == null)
                { ConvertPDFtoImage(DPCPlus); docPageCount++; }
                else if (key == Key.Left && pagePDFInPng[DPCMinus] == null)
                { ConvertPDFtoImage(DPCMinus); docPageCount--; }

                if (docPageCount < 0)
                    docPageCount = document.PageCount - 1;
                else if (docPageCount > document.PageCount - 1)
                    docPageCount = 0;

                nowPage.Content = (docPageCount + 1).ToString();
            }
        }
    }
}