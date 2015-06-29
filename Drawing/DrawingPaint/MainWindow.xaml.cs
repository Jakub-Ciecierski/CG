using Drawing;
using Drawing.GFX.Shapes;
using Drawing.Filling;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Drawing.GFX;

namespace DrawingPaint
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string FILE_EXTENTION = ".pt";

        /// <summary>
        ///     Checks if mouse is being draged
        /// </summary>
        bool isLeftDrag = false;
        bool isRightDrag = false;

        bool isMiddleDrag = false;
        const double middleDragFactor = 0.5;
        System.Windows.Point middleDragStartPoint;

        int gridWidth = 1000;
        int gridHeight = 750;

        const int MAX_WIDTH = 500;
        const int MAX_HEIGHT = 500;

        private const double zoomFactor = 0.1;
        private double zoomValue = 1.0;

        bool isMaximized = false;
        private Rect _restoreLocation;

        PaintBitmap paintBitmap;

        System.Windows.Point point1;
        System.Windows.Point point2;

        System.Windows.Point firstPoint;
        bool isFirstPoint = true;

        bool whichToDraw = true;

        Drawer drawer;

        List<Edge> polygon = new List<Edge>();

        bool doFloodFill = false;
        FloodFill floodFiller;

        /***************************/
        /******* 3D RENDERING ******/
        /***************************/

        Renderer renderer;

        Camera camera;

        bool startRendering = false;

        bool cameraStyle = true;

        bool isLeftClickHold = false;

        System.Windows.Point leftDragStart;

        DispatcherTimer gameLoop = new DispatcherTimer();

        int speed = 60;

        public MainWindow()
        {
            InitializeComponent();
            paintBitmap = new PaintBitmap(gridWidth, gridHeight, paintImage);

            floodFiller = new FloodFill(paintBitmap, paintBitmap.PutPixel);

            drawer = new Drawer(1, paintBitmap.PutPixel);

            //initGameLoopDisp();
        }

        private void initGameLoopDisp()
        {
            gameLoop = new DispatcherTimer();
            gameLoop.Tick += new EventHandler(gameLoop_Tick);
            gameLoop.Interval = new TimeSpan(0, 0, 0, 0, speed);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //init3DRendering();
        }

        /*******************************************************************/
        /************************* DRAG AND DRAW ***************************/
        /*******************************************************************/


        private void drawCircle(System.Windows.Point point1, System.Windows.Point point2)
        {
            int x2 = (int)point2.X;
            int x1 = (int)point1.X;
            int y2 = (int)point2.Y;
            int y1 = (int)point1.Y;

            int R = (int)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

            drawer.DrawCircle((int)point1.X, (int)point1.Y, R, paintBitmap.PutPixel);
        }

        private void drawLine(System.Windows.Point point1, System.Windows.Point point2)
        {
            int x1 = (int)point1.X;
            int y1 = (int)point1.Y;

            int x2 = (int)point2.X;
            int y2 = (int)point2.Y;

            drawer.DrawSymmetricLine(x1, y1, x2, y2, paintBitmap.PutPixel);

            // create line and assume it is part of the polygon
            int y_max = y1 >= y2 ? y1 : y2;
            int y_min = y1 < y2 ? y1 : y2;

            int x_max = x1 >= x2 ? x1 : x2;
            int x_min = x1 < x2 ? x1 : x2;

            //int dy = y_max - y_min;
            //int dx = x_max - x_min;

            /*
            bool steep = Math.Abs(y2 - y1) > Math.Abs(x2 - x1);
            if (steep)
            {
                int tmpX1 = x1;
                x1 = y1;
                y1 = tmpX1;

                int tmpX2 = x2;
                x2 = y2;
                y2 = tmpX2;
            }
            */

            if (y1 > y2)
            {
                int tmpX1 = x1;
                x1 = x2;
                x2 = tmpX1;

                int tmpY1 = y1;
                y1 = y2;
                y2 = tmpY1;
            }

            int dy = y2 - y1;
            int dx = x2 - x1;

            double m = (double)dy / dx;

            Edge edge = new Edge(y2, y1, m);

            edge.curr_x = x1;
            /*
            if (y_min == y1)
                edge.curr_x = x1;
            else
                edge.curr_x = x2;
            */
            polygon.Add(edge);
        }


        private void lineButton_Click(object sender, RoutedEventArgs e)
        {
            whichToDraw = true;
        }

        private void circleButton_Click(object sender, RoutedEventArgs e)
        {
            whichToDraw = false;
        }

        private void fillButton_Click(object sender, RoutedEventArgs e)
        {
            drawLine(firstPoint, point2);

            ScanlineFilling filler = new ScanlineFilling(paintBitmap.PutPixel);

            filler.Fill(polygon);

            polygon = new List<Edge>();
        }

        private void floodFillButton_Click(object sender, RoutedEventArgs e)
        {
            doFloodFill = !doFloodFill;
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox box = sender as ComboBox;
            if (box.Text != "")
                drawer.Thickness = System.Convert.ToInt32(box.Text);
        }

        private void automatonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!cameraStyle)
            {
                if (doFloodFill)
                {
                    System.Windows.Point point = e.GetPosition(e.Source as FrameworkElement);
                    int x = (int)point.X;
                    int y = (int)point.Y;

                    floodFiller.Fill(x, y, paintBitmap.GetColor(x, y), paintBitmap.BRUSH_COLOR);
                }
                else
                {
                    point1 = e.GetPosition(e.Source as FrameworkElement);
                    if (isFirstPoint)
                    {
                        firstPoint = point1;
                        isFirstPoint = false;
                    }
                }
            }
            else
            {
                isLeftClickHold = false;
            }
        }

        private void paintImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!cameraStyle)
            {

            }
            else
            {
                isLeftClickHold = true;
                leftDragStart = e.GetPosition(e.Source as FrameworkElement);
                Console.Write("******************** Mouse started dragging ********************\n");
            }
        }


        private void automatonImage_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!cameraStyle)
            {
                point2 = e.GetPosition(e.Source as FrameworkElement);

                if (point1 != null)
                {
                    if (whichToDraw)
                        drawLine(point1, point2);
                    else
                        drawCircle(point1, point2);

                    point1 = point2;
                    /*
                    new Thread(() =>
                    {
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
                        {
                            if (whichToDraw)
                                drawLine(point1, point2);
                            else
                                drawCircle(point1, point2);
                        }));
                    }).Start();
                    */
                }
            }
        }



        /*******************************************************************/
        /************************* DRAG MOUSE ******************************/
        /*******************************************************************/

        private void automatonImage_MouseMove(object sender, MouseEventArgs e)
        {
            System.Windows.Point point = e.GetPosition(e.Source as FrameworkElement);

            if (isMiddleDrag)
            {
                double deltaX;
                double deltaY;

                deltaX = middleDragStartPoint.X - point.X;
                deltaY = middleDragStartPoint.Y - point.Y;

                gridScollViewer.ScrollToHorizontalOffset(gridScollViewer.HorizontalOffset + deltaX * middleDragFactor);
                gridScollViewer.ScrollToVerticalOffset(gridScollViewer.VerticalOffset + deltaY * middleDragFactor);
            }

            // 3D transition
            if (isLeftClickHold)
            {
                double deltaX;
                double deltaY;

                deltaX = leftDragStart.X - point.X;
                deltaY = leftDragStart.Y - point.Y;

                if (camera != null)
                {
                    if (Math.Abs(deltaX) > 0.0)
                    {
                        if (deltaX >= 0)
                            camera.Alpha -= camera.CameraSensitivity;
                        else
                            camera.Alpha += camera.CameraSensitivity;
                    }
                    if (Math.Abs(deltaY) > 0.0)
                    {
                        if (deltaY >= 0)
                            camera.Beta += camera.CameraSensitivity;
                        else
                            camera.Beta -= camera.CameraSensitivity;
                    }
                }

                leftDragStart = e.GetPosition(e.Source as FrameworkElement);
            }
        }


        private void automatonImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                isMiddleDrag = true;
                middleDragStartPoint = e.GetPosition(e.Source as FrameworkElement);
            }
        }

        private void automatonImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                isMiddleDrag = false;
            }
        }

        private void image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double lastZoomValue = zoomValue;
            if (e.Delta > 0)
            {
                zoomValue += zoomFactor;
            }
            else
            {
                zoomValue -= zoomFactor;
            }
            /* // DISABLED, UNDEFINED FOR 3D
            if (!paintBitmap.ScaleImage(zoomValue))
                zoomValue = lastZoomValue;
            */
        }



        /*******************************************************************/
        /**************************** COMMON *******************************/
        /*******************************************************************/

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (!isMaximized)
                    MaximizeWindow();
                else
                    Restore();
            }
            else
            {
                DragMove();
            }
        }

        private void MaximizeWindow()
        {
            isMaximized = true;
            _restoreLocation = new Rect { Width = Width, Height = Height, X = Left, Y = Top };

            System.Windows.Forms.Screen currentScreen;
            currentScreen = System.Windows.Forms.Screen.FromPoint(System.Windows.Forms.Cursor.Position);

            Height = currentScreen.WorkingArea.Height + 3;
            Width = currentScreen.WorkingArea.Width + 3;

            Left = currentScreen.WorkingArea.X - 2;
            Top = currentScreen.WorkingArea.Y - 2;
        }

        private void Restore()
        {
            isMaximized = false;
            Height = _restoreLocation.Height;
            Width = _restoreLocation.Width;
            Left = _restoreLocation.X;
            Top = _restoreLocation.Y;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void maximizeButton_Click(object sender, RoutedEventArgs e)
        {
            //SystemCommands.MaximizeWindow(this);
            if (!isMaximized)
                MaximizeWindow();
            else
                Restore();
        }

        private void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void windowDropFileHandler(object sender, DragEventArgs e)
        {
            string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop, true);
            loadScene(filenames[0]);
        }

        /***************************************************/
        /**************** RENDERING 3D *********************/
        /***************************************************/

        private void loadScene(string filename)
        {
            List<Drawing.GFX.Shape> scene = SceneLoader.LoadScene(filename);
            init3DRendering(scene);
        }

        private void init3DRendering(List<Drawing.GFX.Shape> scene)
        {
            camera = new Camera(gridWidth, gridHeight);

            renderer = new Renderer(camera);

            foreach (Drawing.GFX.Shape shape in scene)
            {
                shape.drawer = drawer;
                renderer.AddShape(shape);
            }

            initGameLoopDisp();

            gameLoop.Start();
            startRendering = true;
        }

        private void gameLoop_Tick(object sender, EventArgs e)
        {
            if (startRendering)
            {
                renderWindow();
            }
        }

        private void project3D_Click(object sender, RoutedEventArgs e)
        {
            //init3DRendering();
        }

        private void paintImage_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void paintImage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W)
            {
                camera.Z -= camera.MovementSensitivity;
            }

            if (e.Key == Key.S)
            {
                camera.Z += camera.MovementSensitivity;
            }

            if (e.Key == Key.A)
            {
                camera.X -= camera.MovementSensitivity;
            }

            if (e.Key == Key.D)
            {
                camera.X += camera.MovementSensitivity;
            }

            if (e.Key == Key.NumPad4)
                camera.Alpha -= camera.CameraSensitivity / camera.MovementSensitivity;
            if (e.Key == Key.NumPad6)
                camera.Alpha += camera.CameraSensitivity / camera.MovementSensitivity;

            if (e.Key == Key.NumPad8)
                camera.Beta += camera.CameraSensitivity / camera.MovementSensitivity;
            if (e.Key == Key.NumPad2)
                camera.Beta -= camera.CameraSensitivity / camera.MovementSensitivity;

            renderWindow();
        }

        private void renderWindow()
        {
            paintBitmap.RefreshBitmap();

            camera.Update();
            renderer.Render();
        }


    }
}
