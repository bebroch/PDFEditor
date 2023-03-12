using System;
using System.Collections.Generic;
using Bitmap = System.Drawing.Bitmap;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IronPdf;
using System.Linq;
using Image = System.Windows.Controls.Image;
using System.Windows.Controls;
using System.Windows.Data;

namespace PDFEditor
{
    public partial class MainWindow : Window
    {
        //private List<Bitmap> pagePDFInPng;// Массив в котором содержатся изображения всех листов документа
        private ImageSource[] pagesPDFInImage;
        private Bitmap[] pagesPDFInBitmap;
        private PdfDocument document;
        string[] paths;
        private const int DPI = 96;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Кидаем в программу файл и выводим его.
        private void MainPanel_Drop(object sender, DragEventArgs e)
        {
            paths = (string[])e.Data.GetData(DataFormats.FileDrop);
            document = new PdfDocument(paths[0]);
            pagesPDFInImage = new ImageSource[document.PageCount];
            pagesPDFInBitmap = new Bitmap[document.PageCount];
            allPages.Content = document.PageCount;
            nowPage.Content = 1;
            ActivateMenu();
            ConvertPDFtoImage(0);
        }

        private void ImageSourceUpdatedLL(object sender, DataTransferEventArgs e)
        {
            canvas.Width = PictureBox.ActualWidth;
            canvas.Height = PictureBox.ActualHeight;
        }

        // Функция активирует кнопки сверху, после загрузки файла
        private void ActivateMenu()
        {
            saveMenuItem.IsEnabled        = true;
            saveHowMenuItem.IsEnabled     = true;
            saveAllMenuItem.IsEnabled     = true;
            cancelMenuItem.IsEnabled      = true;
            deleteMenuItem.IsEnabled      = true;
            convertJpglMenuItem.IsEnabled = true;
            convertPnglMenuItem.IsEnabled = true;
            convertMenuItem.IsEnabled     = true;
            saveButton.IsEnabled          = true;
            addLineButton.IsEnabled       = true;
            addImageButton.IsEnabled      = true;
            paintButt.IsEnabled           = true;
        }

        // Конвертируем PDF в изображение
        private void ConvertPDFtoImage(int page)
        {
            Bitmap imageBmp = new Bitmap(document.PageToBitmap(page, DPI)); // Выгружаем картинку страницы из файла
            pagesPDFInBitmap[page] = imageBmp; // Сохраняем лист в массив
            BitmapSource imgsource = Imaging.CreateBitmapSourceFromHBitmap(imageBmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()); // Конвертируем из Bitmap в BitmapSource
            PictureBox.Source = imgsource; // Выводим картинку
            pagesPDFInImage[page] = imgsource; // Сохраняем лист в массив
        }

        // Кнопка переключения страницы вперёд
        private void Button_Click_Next(object sender, RoutedEventArgs e) => KeyDown(Key.Right);

        // Кнопка переключения страницы назад
        private void Button_Click_Back(object sender, RoutedEventArgs e) => KeyDown(Key.Left);

        // Переключение PDF по сраницам
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDown(e.Key);
        }

        private int DPCPlus = 0; // Следующая страница, если она выходит за рамки то это 1 страница
        private int DPCMinus = 0; // Следующая страница, всё так же, как и с DPCPLus, только назад
        private int docPageCount = 0; // Текующая страница

        // Функция, которая отлавливает нажатие клавиш и перелистывает в зависимости от нажатой кнопки
        private new void KeyDown(Key key)
        {
            if (document != null && (key == Key.Right || key == Key.Left))
            {
                /*
                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)PictureBox.ActualWidth, (int)PictureBox.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                renderTargetBitmap.Render(PictureBox);

                BitmapSource bitmapSource = renderTargetBitmap;
                bitmapSource.BeginInit();
                bitmapSource.StreamSource.Write(PictureBox.Source);
                bitmapSource.EndInit();

                // Создать DrawingVisual и отрисовать линию
                DrawingVisual drawingVisual = new DrawingVisual();
                using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                {
                    drawingContext.DrawLine(lines[0]);
                    drawingContext.
                }

                // Создать ImageSource из DrawingVisual
                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
                    bitmapSource.PixelWidth, bitmapSource.PixelHeight,
                    bitmapSource.DpiX, bitmapSource.DpiY, PixelFormats.Pbgra32);
                renderTargetBitmap.Render(drawingVisual);
                ImageSource imageSource = renderTargetBitmap;

                // Присвоить ImageSource для Image
                PictureBox.Source = imageSource;*/


                DPCPlus = docPageCount + 1 <= document.PageCount - 1 ? docPageCount + 1 : 0;
                DPCMinus = docPageCount - 1 >= 0 ? docPageCount - 1 : document.PageCount - 1;

                if (key == Key.Right && pagesPDFInImage[DPCPlus] != null)
                { PictureBox.Source = pagesPDFInImage[DPCPlus]; docPageCount++; }
                else if (key == Key.Left && pagesPDFInImage[DPCMinus] != null)
                { PictureBox.Source = pagesPDFInImage[DPCMinus]; docPageCount--; }

                if (key == Key.Right && pagesPDFInImage[DPCPlus] == null)
                { ConvertPDFtoImage(DPCPlus); docPageCount++; }
                else if (key == Key.Left && pagesPDFInImage[DPCMinus] == null)
                { ConvertPDFtoImage(DPCMinus); docPageCount--; }

                if (docPageCount < 0)
                    docPageCount = document.PageCount - 1;
                else if (docPageCount > document.PageCount - 1)
                    docPageCount = 0;



                nowPage.Content = (docPageCount + 1).ToString();
            }

            if (key == Key.Z && Keyboard.Modifiers == ModifierKeys.Control && linesStack.Count != 0)
            {
                foreach (Line item in linesStack.Peek())
                {
                    canvas.Children.Remove(item);
                }
                linesStack.Pop();
            }
        }
    }

    public partial class MainWindow
    {
        // Сохраняем файл
        private void MenuSave(object sender, RoutedEventArgs e) => SaveFile();

        // Сохраняем под новым именем 
        private void MenuSaveHow(object sender, RoutedEventArgs e)
        {

        }

        // Сохраняем все файлы
        private void MenuSaveAll(object sender, RoutedEventArgs e)
        {

        }

        // Открываем новый файл
        private void MenuOpen(object sender, RoutedEventArgs e)
        {

        }

        // Добавляем файл в новое окно
        private void MenuAddFile(object sender, RoutedEventArgs e)
        {

        }

        // Отменяем действие
        private void MenuCancel(object sender, RoutedEventArgs e)
        {

        }

        // Добавляем пустую страницу
        private void MenuAddPage(object sender, RoutedEventArgs e)
        {

        }

        // Удалить страницу
        private void MenuDeletePage(object sender, RoutedEventArgs e)
        {

        }

        // Конвертировать PDF в JPG
        private void MenuConvertToJPG(object sender, RoutedEventArgs e)
        {

        }

        // Конвертировать PDF в PNG
        private void MenuConvertToPNG(object sender, RoutedEventArgs e)
        {

        }
    }

    public partial class MainWindow
    {
        // Функция дл сохранения файла
        private void SaveFile()
        {
            CountLineSigmentChangedPages();
            SaveFileByLineSigment();

            document = ImageToPdfConverter.ImageToPdf(pagesPDFInBitmap, IronPdf.Imaging.ImageBehavior.FitToPage);
            document.SaveAs(paths[0]);
            MessageBox.Show("Файл успешно сохранён!");
        }

        List<Tuple<int, bool>> lineSigment;

        // Находим отрезки где есть и нету изменёных листов, пример [(0, true), (11, false)] от 0 до 10 лист изменён, а дальше чистые
        private void CountLineSigmentChangedPages()
        {
            lineSigment = new List<Tuple<int, bool>>(); // Создаём массив с этими отрезками
            lineSigment.Add(new Tuple<int, bool>(0, true)); // Добавляем 1 страницу, так как программа по стандарту открывает 1 страницу
            for (int i = 1; i < document.PageCount; i++)
            {
                if (pagesPDFInBitmap[i] != null && pagesPDFInBitmap[i - 1] == null)
                    lineSigment.Add(new Tuple<int, bool>(i, true)); // Если до этого страница не была изменена, а цикл находится на изменёной странице, то записываем, как начало отрезка изменёных листов
                else if (pagesPDFInBitmap[i] == null && pagesPDFInBitmap[i - 1] != null)
                    lineSigment.Add(new Tuple<int, bool>(i, false)); // Тут записываем начало не изменёных листов
            }
        }

        // Сохраняем файл по изменёным отрезкам и скрепляем со старыми листами
        private void SaveFileByLineSigment()
        {
            PdfDocument newDocument = new PdfDocument(document);

            foreach (Tuple<int, bool> sigment in lineSigment)
            {
                for (int i = sigment.Item1; i < document.PageCount; i++)
                {
                    /*
                     * короче прикол в том что есть первый отрезок (0, true) (я пометил красным шариком)
                     * а чтобы записать его нужен конец, типо (0, 10, true) 0 начало 10 конец true типо это изменёная страница
                     * а конца у него нету по этому хз чё делать, 
                     * можно структуру сделать, как у int, но сейчас я это делать уже не буду
                     * а ещё нужно переписать CountLineSigmentChangedPages 
                     * потому что чел может просто пролистать страницы
                     * короче когда рисуешь или добавляешь страницы или ещё чё то нужно тогда их сейвить
                     * а не просто пролистыванием страницы
                     * пометка на будущее так сказать
                     */
                }
            }
        }
    }

    public partial class MainWindow
    {
        // Кнопка, которая сохраняет pdf документ
        private void SaveButt(object sender, RoutedEventArgs e) => SaveFile();

        // Кнопка, которая добавляет строку 
        private void AddLineButt(object sender, RoutedEventArgs e)
        {

        }

        // Кнопка, которая добавляет картинку 
        private void AddImageButt(object sender, RoutedEventArgs e)
        {

        }

        // Кнопка, которая выбирает кисть, что бы рисовать.
        private void PaintButt(object sender, RoutedEventArgs e)
        {
            Line line = new Line();
            line.X1 = 0;
            line.Y1 = 450;
            line.X2 = 1000;
            line.Y2 = 450;
            line.Stroke = new SolidColorBrush(Colors.Black);
        }


        private Stack<List<Line>> linesStack = new Stack<List<Line>>();
        private List<Line> lines;

        private void CanvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            lines = new List<Line>();
        }

        private double mouseX;
        private double mouseY;

        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (e.GetPosition(canvas).X > 0
                && e.GetPosition(canvas).Y > 0
                && e.GetPosition(canvas).X < canvas.ActualWidth
                && e.GetPosition(canvas).Y < canvas.ActualHeight)
            {

                if (e.LeftButton == MouseButtonState.Pressed && lines != null)
                {
                    Line line = new Line();
                    line.X1 = mouseX;
                    line.Y1 = mouseY;
                    line.X2 = e.GetPosition(canvas).X;
                    line.Y2 = e.GetPosition(canvas).Y;
                    line.Stroke = new SolidColorBrush(Colors.Black);

                    lines.Add(line);
                    canvas.Children.Add(line);
                }

                mouseX = e.GetPosition(canvas).X;
                mouseY = e.GetPosition(canvas).Y;
            }
        }

        private void CanvasMouseUp(object sender, MouseButtonEventArgs e)
        {
            linesStack.Push(lines);
        }
    }
}