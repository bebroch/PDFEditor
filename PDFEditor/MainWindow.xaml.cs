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
using System.IO;
using Point = System.Drawing.Point;
using Graphics = System.Drawing.Graphics;

namespace PDFEditor
{
    // Главный класс
    public partial class MainWindow : Window
    {
        private Bitmap[] pagesPDFInBitmap; // Картинки в Bitmap формате
        private PdfDocument document; // Сам документ (пока один)
        string path; // Пути до PDF файлов
        private const int DPI = 96; // Качество картинок
        private ChoosenEditor stateEdit; // Какая включена кнопка для редактирования (К примеру, если нажата кнопка с кистью, то будет состояние paint)
        private string fileName;

        public MainWindow()
        {
            InitializeComponent();
            stateEdit = ChoosenEditor.paint;
        }

        // Функция активируется, когда пользователь кидает файл в программу
        private void MainPanel_Drop(object sender, DragEventArgs e)
        {
            path = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            fileName = System.IO.Path.GetFileName(path);
            SavePdfToImage(path);
        }

        // Конвертируем PDF в изображение
        private BitmapSource ConvertPDFtoImage(int page)
        {
            // Выгружаем картинку страницы из файла
            Bitmap imageBmp = new Bitmap(document.PageToBitmap(page, DPI));
            pagesPDFInBitmap[page] = imageBmp; // Сохраняем лист в массив
            return ConvertToBitmapSource(imageBmp); // Конвертируем из Bitmap в BitmapSource;
        }

        // Сохраняем PDF файл и выводим его
        private void SavePdfToImage(string path)
        {
            // Создаём документ по путю
            document = new PdfDocument(path);
            // Так же указываем размеры массива, где лежат все листы, только уже в формате Bitmap
            pagesPDFInBitmap = new Bitmap[document.PageCount];
            // Отображаем сколько всего страниц в документе
            allPages.Content = document.PageCount;
            // Указываем начальную страницу какую страницу программа отрисует первой (по стандарту 1 страница)
            nowPage.Content = 1;
            // Активируем верхние кнопки, которые служат для редактирования 
            ActivateMenu();
            // Разбиваем PDF на листы и загружаем их в массив, у которого потом выводим 0 индекс (1 страницу документа)
            PictureBox.Source = ConvertPDFtoImage(0);

            PrintPagePdf();
            canvas.Width = PictureBox.Source.Width;
            canvas.Height = PictureBox.Source.Height;
            Title = path;
            canvas.Children.Clear();
        }

        // Вывод страниц, находящихся по соседству, текущей страницы (столбец слева снизу)
        private void PrintPagePdf()
        {
            for (int i = 0; i < document.PageCount; i++)
            {
                Image img = new Image();
                img.Margin = new Thickness(10);

                if (pagesPDFInBitmap[i] != null)
                {
                    img.Source = ConvertToBitmapSource(pagesPDFInBitmap[i]);
                }
                else
                {
                    img.Source = ConvertPDFtoImage(i);
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
            if (e.Key != Key.Right && e.Key != Key.Left && e.Key != Key.Z && Keyboard.Modifiers != ModifierKeys.Control)
            {
                e.Handled = false;
                return;
            }
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
                SavePage(true); // Сохраняем предыдущий лист

                if (key == Key.Right && pagesPDFInBitmap[DPCPlus] != null) // Если нажата стрелка вправо и страница документа конвертирована, то отрисовывается следующая страница
                {
                    PictureBox.Source = ConvertToBitmapSource(pagesPDFInBitmap[DPCPlus]);
                    docPageNow++;
                } else if (key == Key.Left && pagesPDFInBitmap[DPCMinus] != null) // Если же стрелка вправо, то отрисовывается предыдущая страница
                {
                    PictureBox.Source = ConvertToBitmapSource(pagesPDFInBitmap[DPCMinus]);
                    docPageNow--;
                }


                if (key == Key.Right && pagesPDFInBitmap[DPCPlus] == null) // Если же страница документа не конвертирована, то отрисовать её и вывести
                {
                    PictureBox.Source = ConvertPDFtoImage(DPCPlus);
                    docPageNow++;
                } else if (key == Key.Left && pagesPDFInBitmap[DPCMinus] == null) // Стрелка влево - предыдущая страница
                {
                    PictureBox.Source = ConvertPDFtoImage(DPCMinus);
                    docPageNow--;
                }


                if (docPageNow < 0) // Если текущая страница меньше нуля 
                    docPageNow = document.PageCount - 1; // То она становится последней страницей
                else if (docPageNow > document.PageCount - 1) // Если же текущая страница больше номера последней
                    docPageNow = 0; // То она становится первой

                nowPage.Content = (docPageNow + 1).ToString(); // Вывод Текущей страницы
            }

            // Отмена действия
            if (key == Key.Z && Keyboard.Modifiers == ModifierKeys.Control)
            {
                UndoAction();
            }
        }

        private void UndoAction()
        {
            if (stateEdit == ChoosenEditor.paint && linesStack.Count != 0)
            {
                foreach (Line item in linesStack.Peek())
                {
                    canvas.Children.Remove(item);
                }
                linesStack.Pop();
            }

            else if (stateEdit == ChoosenEditor.textBlock && textBoxes.Count != 0)
            {
                canvas.Children.Remove(textBoxes.Peek());
                textBoxes.Pop();
            }
        }

        // Конвертация из Bitmap в BitmapSource или ImageSource
        private BitmapSource ConvertToBitmapSource(Bitmap bitmap)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        enum ChoosenEditor
        {
            paint,
            textBlock,
            pictureBlock
        }
    }

    // Класс, где содержатся все кнопки из меню, которое висит сверху, кроме "MenuSave" и "Сохранить Как"
    public partial class MainWindow
    {
        // Открываем новый файл
        private void MenuOpen(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = "*.pdf";
            dlg.Filter = "PDF Files (*.pdf)|*.pdf";

            if (dlg.ShowDialog() == true)
            {
                path = dlg.FileName;
                SavePdfToImage(dlg.FileName);
            }
        }

        // Отменяем действие
        private void MenuCancel(object sender, RoutedEventArgs e)
        {
            UndoAction();
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
        private void MenuConvertToJPG(object sender, RoutedEventArgs e) => SaveBitmapInImage(".jpg");

        // Конвертировать PDF в PNG
        private void MenuConvertToPNG(object sender, RoutedEventArgs e) => SaveBitmapInImage(".png");

        // Конвертор
        private void SaveBitmapInImage(string extension)
        {
            using (System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SavePage(false);
                    for (int i = 0; i < pagesPDFInBitmap.Length - 1; i++)
                    {
                        pagesPDFInBitmap[i].Save(dialog.SelectedPath + $"\\{fileName}{i+1}{extension}");
                    }
                }
            }
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
            SaveBitmapInDocumment();
            document.SaveAs(path);
            MessageBox.Show("Файл успешно сохранён!");
        }

        // Сохраняем под новым именем 
        private void MenuSaveHow(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.DefaultExt = "*.pdf";
            sfd.Filter = "PDF Files (*.pdf)|*.pdf";

            if (sfd.ShowDialog() == true)
            {
                SaveBitmapInDocumment();
                document.SaveAs(sfd.FileName);
            }
        }

        private void SaveBitmapInDocumment()
        {
            SavePage(false);

            for (int i = 0; i <= document.PageCount - 1; i++)
            {
                if (pagesPDFInBitmap[i] != null)
                {
                    document.RemovePage(i);
                    document.InsertPdf(ImageToPdfConverter.ImageToPdf(pagesPDFInBitmap[i], IronPdf.Imaging.ImageBehavior.FitToPage), i);
                }
            }
            if (pagesPDFInBitmap[0].Width >= 800)
                document.Resize(IronPdf.Rendering.PdfPaperSize.A4Rotated);
            else
                document.Resize(IronPdf.Rendering.PdfPaperSize.A4);

        }
    }

    // Класс, который отвечает за канвас, то есть за нажатие на него
    public partial class MainWindow 
    {
        // Нажатие кнопки
        private void CanvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CheckCanvasBorder(e.GetPosition(canvas)))
                return;

            switch (stateEdit)
            {
                case ChoosenEditor.paint:
                    MousePressDown();
                    break;
                case ChoosenEditor.pictureBlock:
                    break;
                case ChoosenEditor.textBlock:
                    MouseClickOnCanvas(e.GetPosition(canvas));
                    break;
            }
        }

        // Отпускание кнопки
        private void CanvasMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (CheckCanvasBorder(e.GetPosition(canvas)))
                return;

            switch (stateEdit)
            {
                case ChoosenEditor.paint:
                    MousePressUp();
                    break;
                case ChoosenEditor.pictureBlock:
                    break;
                case ChoosenEditor.textBlock:
                    break;
            }
        }

        // Зажатие клавиши на canvas
        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (CheckCanvasBorder(e.GetPosition(canvas)))
                return;

            switch (stateEdit)
            {
                case ChoosenEditor.paint:
                    MousePressAndMove(e.GetPosition(canvas), e.LeftButton);
                    break;
                case ChoosenEditor.pictureBlock:
                    break;
                case ChoosenEditor.textBlock:
                    break;
            }
        }

        private bool CheckCanvasBorder(System.Windows.Point point)
        {
            if (point.X > 0 && point.Y > 0 && point.X < canvas.ActualWidth && point.Y < canvas.ActualHeight)
                return false;
            return true;
        }
    }

    // Класс, в котором реализовано рисование
    public partial class MainWindow
    {
        // Кнопка, которая выбирает кисть, что бы рисовать.
        private void PaintButt(object sender, RoutedEventArgs e)
        {
            stateEdit = ChoosenEditor.paint;
        }

        // Сохранение страницы при переключении
        private void SavePage(bool IsClear)
        {
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)canvas.Width, (int)canvas.Height, 96d, 96d, PixelFormats.Pbgra32);
            renderBitmap.Render(canvas);

            PngBitmapEncoder png = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create(renderBitmap));

            using (MemoryStream stream = new MemoryStream())
            {
                png.Save(stream);
                using (Bitmap bitmap = new Bitmap(stream))
                {
                    Graphics graphics = Graphics.FromImage(pagesPDFInBitmap[docPageNow]);
                    graphics.DrawImage(bitmap, new Point(0, 0));
                    graphics.Dispose();
                }
            }

            if (IsClear)
                canvas.Children.Clear();
        }

        private Stack<List<Line>> linesStack = new Stack<List<Line>>();
        private List<Line> lines;
        private double mouseX;
        private double mouseY;

        // Нажатие кнопки
        private void MousePressDown()
        {
            lines = new List<Line>();
        }

        // Отпускание кнопки
        private void MousePressUp()
        {
            linesStack.Push(lines);
        }

        // Происходит когда пользователь держит ЛКМ и водит по Grid-у
        private void MousePressAndMove(System.Windows.Point point, MouseButtonState mouseState)
        {
            if (point.X > 0
                && point.Y > 0
                && point.X < canvas.ActualWidth
                && point.Y < canvas.ActualHeight)
            {
                if (mouseState == MouseButtonState.Pressed && lines != null)
                {
                    Line line = new Line
                    {
                        X1 = mouseX,
                        Y1 = mouseY,
                        X2 = point.X,
                        Y2 = point.Y,
                        Stroke = new SolidColorBrush(Colors.Black)
                    };

                    lines.Add(line);
                    canvas.Children.Add(line);
                }

                mouseX = point.X;
                mouseY = point.Y;
            }
        }
    }

    // Добавление строки
    public partial class MainWindow
    {
        // Кнопка, которая добавляет строку 
        private void AddLineButt(object sender, RoutedEventArgs e)
        {
            stateEdit = ChoosenEditor.textBlock;
        }

        private TextBox textBox;
        private Stack<TextBox> textBoxes = new Stack<TextBox>();

        private void MouseClickOnCanvas(System.Windows.Point mouseClickPoint)
        {
            textBox = new TextBox();
            canvas.Children.Add(textBox);
            textBox.FontSize = 18;
            textBox.Width = 100;
            textBox.Background = Brushes.Transparent;
            textBox.BorderBrush = Brushes.Transparent;
            textBox.TextChanged += new TextChangedEventHandler(TextBoxChanged);
            textBox.Margin = new Thickness(mouseClickPoint.X, mouseClickPoint.Y, mouseClickPoint.X + 100, mouseClickPoint.Y + 100);
            textBox.Focus();
            textBoxes.Push(textBox);
        }

        private void TextBoxChanged(object sender, EventArgs e)
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
            stateEdit = ChoosenEditor.pictureBlock;
        }
    }
}