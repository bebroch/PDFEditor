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

namespace PDFEditor
{
    public partial class MainWindow : Window
    {
        private ImageSource[] pagesPDFInImage; // Картинки в ImageSource формате
        private Bitmap[] pagesPDFInBitmap; // Картинки в Bitmap формате
        private PdfDocument document; // Сам документ (пока один)
        string[] paths; // Пути до PDF файлов
        private const int DPI = 96; // Качество картинок

        public MainWindow()
        {
            InitializeComponent();
        }

        // Функция активируется, когда пользователь кидает файл в программу
        private void MainPanel_Drop(object sender, DragEventArgs e)
        {
            paths = (string[])e.Data.GetData(DataFormats.FileDrop);
            SavePdfToImage(paths[0]);
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
            BitmapSource imgsource = Imaging.CreateBitmapSourceFromHBitmap(
                imageBmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            ); // Конвертируем из Bitmap в BitmapSource

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
            e.Handled = true;
            KeyDown(e.Key);
        }

        private int DPCPlus = 0; // Следующая страница, если она выходит за рамки, то это 1 страница
        private int DPCMinus = 0; // Следующая страница, всё так же, как и с DPCPLus, только назад
        private int docPageCount = 0; // Текующая страница

        // Функция, которая отлавливает нажатие клавиш и перелистывает в зависимости от нажатой кнопки
        private new void KeyDown(Key key)
        {
            if (document != null && (key == Key.Right || key == Key.Left))
            {
                /* Хочу сохранять все рисунки, что сделал пользователь, после переключения каждой страницы, типо было не на canvas и Image, а всё вместе, на Image
                
                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)PictureBox.ActualWidth, (int)PictureBox.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                renderTargetBitmap.Render(PictureBox);

                BitmapSource bitmapSource = renderTargetBitmap;
                bitmapSource.
                bitmapSource.StreamSource.Write(PictureBox.Source);
                bitmapSource.EndInit();

                // Создать DrawingVisual и отрисовать линию

                DrawingVisual drawingVisual = new DrawingVisual();
                drawingVisual.Children.Add(lines);
                using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                {
                    drawingContext.DrawLine(lines[0]);
                    drawingContext.
                }

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

    // Класс, где содержатся все кнопки из меню, которое висит сверху, кроме "MenuSave"
    public partial class MainWindow
    {
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
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = "*.pdf";
            dlg.Filter = "PDF Files (*.pdf)|*.pdf";

            if (dlg.ShowDialog() == true)
            {
                SavePdfToImage(dlg.FileName);
            }
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

    // Класс про сохранение файла (не работает)
    public partial class MainWindow
    {
        // Конпка в верхней строке, которая сохраняет файл
        private void MenuSave(object sender, RoutedEventArgs e) => SaveFile();

        // Кнопка, которая сохраняет pdf документ
        private void SaveButt(object sender, RoutedEventArgs e) => SaveFile();

        // Функция для сохранения файла
        private void SaveFile()
        {
            CountLineSigmentChangedPages();
            SaveFileByLineSigment();

            document = ImageToPdfConverter.ImageToPdf(pagesPDFInBitmap, IronPdf.Imaging.ImageBehavior.FitToPage);
            document.SaveAs(paths[0]);
            MessageBox.Show("Файл успешно сохранён!");
        }

        // Сохраняем PDF файл и выводим его
        private void SavePdfToImage(string path)
        {
            // Создаём документ по путю
            document = new PdfDocument(path);
            // Указываем размеры массива, где лежат все листы, в формате картинки
            pagesPDFInImage = new ImageSource[document.PageCount];
            // Так же указываем размеры массива, где лежат все листы, только уже в формате Bitmap
            pagesPDFInBitmap = new Bitmap[document.PageCount];
            // Отображаем сколько всего страниц в документе
            allPages.Content = document.PageCount;
            // Указываем начальную страницу какую страницу программа отрисует первой (по стандарту 1 страница)
            nowPage.Content = 1;
            // Активируем верхние кнопки, которые служат для редактирования 
            ActivateMenu();
            // Разбиваем PDF на листы и загружаем их в массив, у которого потом выводим 0 индекс (1 страницу документа)
            ConvertPDFtoImage(0);
        }

        private List<Tuple<int, bool>> lineSigment;

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

    // Класс, в котором реализовано рисование
    public partial class MainWindow
    {

        // Кнопка, которая выбирает кисть, что бы рисовать.
        private void PaintButt(object sender, RoutedEventArgs e)
        {
            
        }

        private Stack<List<Line>> linesStack = new Stack<List<Line>>();
        private List<Line> lines;
        private double mouseX;
        private double mouseY;

        private void CanvasMouseDown(object sender, MouseButtonEventArgs e) => lines = new List<Line>(); // нажатие кнопки
        private void CanvasMouseUp(object sender, MouseButtonEventArgs e) => linesStack.Push(lines); // отпускание кнопки

        private void CanvasMouseMove(object sender, MouseEventArgs e) // Происходит когда пользователь держит ЛКМ и водит по Grid-у
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
    }

    // Добавление строки (не работает)
    public partial class MainWindow
    {
        // Кнопка, которая добавляет строку 
        private void AddLineButt(object sender, RoutedEventArgs e)
        {

        }

        // Там к TextBox нужно дописать MouseLeftButtonDown="myTextBoxMouseLeftButtonDown" что бы работало, наверное

        /*
         <TextBox Background="Transparent" FontSize="20" Grid.Row="2"
             Grid.Column="1" Margin="48,21,962,595" Grid.RowSpan="2"/>
         */

        /* Написано ИИ
        private bool isDragging = false;
        private Point lastPosition;

        private void myTextBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            lastPosition = e.GetPosition(myTextBox);
            myTextBox.CaptureMouse();
        }

        private void myTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point newPosition = e.GetPosition(this);
                double deltaX = newPosition.X - lastPosition.X;
                double deltaY = newPosition.Y - lastPosition.Y;
                myTextBox.Margin = new Thickness(myTextBox.Margin.Left + deltaX, myTextBox.Margin.Top + deltaY, 0, 0);
                lastPosition = newPosition;
            }
        }

        private void myTextBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            myTextBox.ReleaseMouseCapture();
        }*/

    }

    // Добавление картинки (не работает)
    public partial class MainWindow
    {
        // Кнопка, которая добавляет картинку 
        private void AddImageButt(object sender, RoutedEventArgs e)
        {

        }
    }
}