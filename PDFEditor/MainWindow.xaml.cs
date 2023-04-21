using System;
using System.Collections.Generic;
using Bitmap = System.Drawing.Bitmap;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Controls;
using IronPdf;
using System.Drawing.Imaging;
using System.IO;

namespace PDFEditor
{
    // Главный класс
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

        // Конвертируем PDF в изображение
        private BitmapSource ConvertPDFtoImage(int page, bool isPrint)
        {
            // Выгружаем картинку страницы из файла
            Bitmap imageBmp = new Bitmap(document.PageToBitmap(page, DPI)); 
            pagesPDFInBitmap[page] = imageBmp; // Сохраняем лист в массив
            BitmapSource imgsource = Imaging.CreateBitmapSourceFromHBitmap(
                imageBmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            ); // Конвертируем из Bitmap в BitmapSource

            if (isPrint)
            {
                pagesPDFInImage[page] = imgsource; // Сохраняем лист в массив
                PictureBox.Source = imgsource; // Выводим картинку
            }
            return imgsource;
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
            ConvertPDFtoImage(0, true);

            PrintPagePdf();
        }

        // Вывод страниц, находящихся по соседству, текущей страницы (столбец слева снизу)
        private void PrintPagePdf()
        {
            for (int i = 0; i < document.PageCount; i++)
            {
                Image img = new Image();
                img.Margin = new Thickness(10);

                if (pagesPDFInImage[i] != null)
                {
                    img.Source = pagesPDFInImage[i];
                }
                else
                {
                    img.Source = ConvertPDFtoImage(i, false);
                }

                PrintPagePanel.Children.Add(img);
                Grid.SetRow(img, i);
                RowDefinition row = new RowDefinition();
                row.Height = GridLength.Auto;
                PrintPagePanel.RowDefinitions.Add(row);
            }
        }

        // Функция активирует кнопки сверху, после загрузки файла
        private void ActivateMenu()
        {
            saveMenuItem.IsEnabled        = true;
            saveHowMenuItem.IsEnabled     = true;
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
        private int docPageNow = 0; // Текующая страница

        // Функция, которая отлавливает нажатие клавиш и перелистывает в зависимости от нажатой кнопки
        private new void KeyDown(Key key)
        {
            if (document != null && (key == Key.Right || key == Key.Left))
            {
                DPCPlus = docPageNow + 1 <= document.PageCount - 1 ? docPageNow + 1 : 0; // Подсчёт следующей страницы, чтобы не выходила за рамки
                DPCMinus = docPageNow - 1 >= 0 ? docPageNow - 1 : document.PageCount - 1; // предыдущей страницы

                if (key == Key.Right && pagesPDFInImage[DPCPlus] != null) // Если нажата стрелка вправо и страница документа конвертирована
                { PictureBox.Source = pagesPDFInImage[DPCPlus]; docPageNow++; } // То отрисвывается следующая страница
                else if (key == Key.Left && pagesPDFInImage[DPCMinus] != null) // Если же стрелка вправо
                { PictureBox.Source = pagesPDFInImage[DPCMinus]; docPageNow--; } // То отрисовывается предыдущая страница

                if (key == Key.Right && pagesPDFInImage[DPCPlus] == null) // А если же страница документа не конвертирована
                { ConvertPDFtoImage(DPCPlus, true); docPageNow++; } // То отрисовать её и вывести
                else if (key == Key.Left && pagesPDFInImage[DPCMinus] == null) // Стрелка влево
                { ConvertPDFtoImage(DPCMinus, true); docPageNow--; } //Предыдущая страница

                if (docPageNow < 0) // Если текущая страница меньше нуля 
                    docPageNow = document.PageCount - 1; // То она становится последней страницей
                else if (docPageNow > document.PageCount - 1) // Если же текущая страница больше номера последней
                    docPageNow = 0; // То она становится первой

                nowPage.Content = (docPageNow + 1).ToString(); // Вывод Текущей страницы
                SavePage();
            }

            // Отмена действия
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
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.DefaultExt = "*.pdf";
            sfd.Filter = "PDF Files (*.pdf)|*.pdf";

            if (sfd.ShowDialog() == true)
            {
                document.SaveAs(sfd.FileName);
            }
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

    // Класс про сохранение файла
    public partial class MainWindow
    {
        // Конпка в верхней строке, которая сохраняет файл
        private void MenuSave(object sender, RoutedEventArgs e) => SaveFile();

        // Кнопка, которая сохраняет pdf документ
        private void SaveButt(object sender, RoutedEventArgs e) => SaveFile();

        // Функция для сохранения файла
        private void SaveFile()
        {

            for (int i = 0; i < document.PageCount - 1; i++)
            {
                if (pagesPDFInBitmap[i] != null)
                {
                    document.RemovePage(i);
                    document.InsertPdf(ImageToPdfConverter.ImageToPdf(pagesPDFInBitmap[i], IronPdf.Imaging.ImageBehavior.FitToPage), i);
                }
            }

            document.SaveAs(paths[0]);
            MessageBox.Show("Файл успешно сохранён!");
        }
    }

    // Класс, в котором реализовано рисование
    public partial class MainWindow
    {
        // Кнопка, которая выбирает кисть, что бы рисовать.
        private void PaintButt(object sender, RoutedEventArgs e)
        {
            
        }

        // Сохранение страницы при переключении
        private void SavePage()
        {
            //canvas System.Windows.Controls.Canvas
            //PictureBox System.Windows.Controls.Image

            //RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
            //    (int)canvas.ActualWidth, (int)canvas.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
            //renderBitmap.Render(canvas);
            //BitmapSource bitmapSource = BitmapFrame.Create(renderBitmap);
            //MemoryStream stream = new MemoryStream();
            //BitmapEncoder encoder = new BmpBitmapEncoder();
            //encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            //encoder.Save(stream);
            //Bitmap bitmap = new Bitmap(stream);
        }

        private Stack<List<Line>> linesStack = new Stack<List<Line>>();
        private List<Line> lines;
        private double mouseX;
        private double mouseY;

        // нажатие кнопки
        private void CanvasMouseDown(object sender, MouseButtonEventArgs e) => lines = new List<Line>(); 

        // отпускание кнопки
        private void CanvasMouseUp(object sender, MouseButtonEventArgs e) => linesStack.Push(lines); 

        // Происходит когда пользователь держит ЛКМ и водит по Grid-у
        private void CanvasMouseMove(object sender, MouseEventArgs e) 
        {
            if (e.GetPosition(canvas).X > 0
                && e.GetPosition(canvas).Y > 0
                && e.GetPosition(canvas).X < canvas.ActualWidth
                && e.GetPosition(canvas).Y < canvas.ActualHeight)
            {
                if (e.LeftButton == MouseButtonState.Pressed && lines != null)
                {
                    Line line = new Line
                    {
                        X1 = mouseX,
                        Y1 = mouseY,
                        X2 = e.GetPosition(canvas).X,
                        Y2 = e.GetPosition(canvas).Y,
                        Stroke = new SolidColorBrush(Colors.Black)
                    };

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