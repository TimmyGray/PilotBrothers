using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PilotBrothers.Model;
using PilotBrothers.ViewModel;
using System.Media;
using System.Windows.Media.Animation;

namespace PilotBrothers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationViewModel viewModel;
       
        public MainWindow()
        {
            viewModel = new ApplicationViewModel();//создаем контекст для работы с ключами

            InitializeComponent();
        }

        private void SizeSetClick(object sender, RoutedEventArgs e)
        {
            try
            {
                short size = Convert.ToInt16(SizeSetter.Text); //пытаемся конвертировать введенное значение в short,
                                                               //и ловим исключение, обрабатывая в соотвествии с
                                                               //выброшенным исключением
                if (size>=2&&size<=5)                        //ставить больше данного диапазона бессмысленно,
                                                             //т.к. игра становится крайне сложной
                {
                    viewModel.SafeKeys.Clear();             //отчищаем все данные(обьекты) от предыдущей игры
                    MainGrid.Children.Clear();
                    MainGrid.Columns = size;               //задаем поле(массив NxN)
                    MainGrid.Rows = size;

                    for (int i = 0; i < size; i++)      //заполняем поле ключами проходясь по каждой клетке
                    {

                        
                        for (int k = 0; k < size; k++)
                        {
                            Canvas canvas = new Canvas(); //задание характеристик для клеток
                            canvas.Height = 50;
                            canvas.Width = 50;
                            canvas.Background = new SolidColorBrush(Colors.BurlyWood);
                            canvas.Cursor = Cursors.Hand;
                            canvas.Tag = $"{k},{i}";                            //записываем в таг координаты, для связи с ключами
                            canvas.MouseLeftButtonDown += ChangeMethod;         //вешаем на контейнер метод для изменения положения ключей
                            SafeKey safekey = new SafeKey(k, i);                //создаем ключ с координатами текущей клетки
                            viewModel.SafeKeys.Add(safekey);                    //добавляем в коллекцию

                            Path path = new Path();                             //рисуем линию исходя из положения ключа
                            path.Stroke = new SolidColorBrush(Colors.Black);
                            path.Data = new LineGeometry();
                            canvas.Children.Add(path);
                            
                            if (safekey.Rotate==true)
                            {
                                Point startpoint = new Point(25, 10);
                                Point endpoint = new Point(25, 40);
                                path.Data = new LineGeometry(startpoint,endpoint);
                            }
                            else
                            {
                                Point startpoint = new Point(10, 25);
                                Point endpoint = new Point(40, 25);
                                path.Data = new LineGeometry(startpoint, endpoint);
                            }
                            
                            MainGrid.Children.Add(canvas);
                        }
                    }
                }
                else
                {
                    if (size > 5)
                    {
                        Exception exception = new Exception("Слишком большой сейф!");
                        throw exception;

                    }
                    if (size<2)
                    {
                        Exception exception = new Exception("Слишком странный сейф!");
                        throw exception;

                    }


                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show($"{ex.Message}");
            }
        }

        private void ChangeMethod(object sender, RoutedEventArgs e)
        {
            Canvas canvas = sender as Canvas;                   //по клику получаем текущую клетку
            string[] loc = canvas.Tag.ToString().Split(",");    //достаем из тага ее координаты
            int x = Convert.ToInt32(loc[0]);
            int y = Convert.ToInt32(loc[1]);
            bool open = viewModel.RotateKeys(x, y);             //выполняем основной метод по повороту ключей и проверяем открыт он или нет
            for (int i = 0; i < viewModel.SafeKeys.Count; i++)  //графическое отображение поворота ключей
            {
                Canvas rotatecanvas = MainGrid.Children[i] as Canvas;
                string[] rotateloc = rotatecanvas.Tag.ToString().Split(",");
                int rotatex = Convert.ToInt32(rotateloc[0]);
                int rotatey = Convert.ToInt32(rotateloc[1]);

                if (rotatex==x||rotatey==y)
                {

                    Path path = rotatecanvas.Children[0] as Path;

                    LineGeometry line = path.Data as LineGeometry;

                    if (viewModel.SafeKeys[i].Rotate==true)
                    {
                        Point newstartpoint = new Point(25, 10);
                        Point newendpoint = new Point(25, 40);
                        
                        Rotate(line,newstartpoint,newendpoint);

                    }
                    else
                    {
                        Point newstartpoint = new Point(10, 25);
                        Point newendpoint = new Point(40, 25);

                        Rotate(line,newstartpoint,newendpoint);
                    }
                }
            }
            if (open)
            {
                OpenBut.IsEnabled = true;
            }
        }

        private static void Rotate(LineGeometry line,Point newstartpoint,Point newendpoint)
        {


            PointAnimation rotatestart = new PointAnimation();      //создаем аницацию по плавному повороту линий
            rotatestart.From = line.StartPoint;
            rotatestart.To = newstartpoint;
            rotatestart.Duration = TimeSpan.FromSeconds(0.2);

            PointAnimation rotateend = new PointAnimation();
            rotateend.From = line.EndPoint;
            rotateend.To = newendpoint;
            rotateend.Duration = TimeSpan.FromSeconds(0.2);

            line.BeginAnimation(LineGeometry.StartPointProperty, rotatestart);
            line.BeginAnimation(LineGeometry.EndPointProperty, rotateend);
        }

        private void OpenBut_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Поздравляю! Сейф открыт, осталось вернуть слона!"); //поздравляем с открытием и отчищаем все поле
            MainGrid.Children.Clear();
            OpenBut.IsEnabled = false;
        }
    }
}
