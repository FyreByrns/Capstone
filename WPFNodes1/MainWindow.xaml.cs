using AbstractNodeConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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

namespace WPFNodes1 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public List<Node> Nodes { get; set; }

        public MainWindow() {
            InitializeComponent();
            canvasBackground.Background = Brushes.DarkSlateGray;
            canvasBackground.MouseMove += Canvas_MouseMove;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e) {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            NodeContainer n = new NodeContainer(new InputNode() { Value = 10 });
            canvasBackground.Children.Add(n);
            Canvas.SetLeft(n, 0);
            Canvas.SetTop(n, 1);
        }
    }

    public class NodeContainer : Grid {
        public Node Node {
            get => _node;
            set {
                _node = value;
                Update();
            }
        }
        Node _node;

        Point Position { get; set; }
        Point dragOffset;
        bool dragging;

        public string NodeName { get; set; } = "Unnamed";
        public string NodeType => Node.GetType().Name;

        public List<Node.Connection> ConnectionsIn => Node.inputNodes.ToList();
        List<Button> _inButtons = new List<Button>();
        public List<Node.Connection> ConnectionsOut => Node.outputNodes.ToList();
        List<Button> _outButtons = new List<Button>();

        Grid main;
        Grid body;
        Label type;
        StackPanel inVisualize;
        StackPanel outVisualize;

        public NodeContainer() {
            Background = Brushes.DimGray;

            main = new Grid();
            RowDefinition mainTitle = new RowDefinition();
            RowDefinition mainBody = new RowDefinition();
            main.RowDefinitions.Add(mainTitle);
            main.RowDefinitions.Add(mainBody);

            body = new Grid();
            ColumnDefinition bodyInputs = new ColumnDefinition() { Width = new GridLength(20) };
            ColumnDefinition bodyBody = new ColumnDefinition() { };
            ColumnDefinition bodyOutputs = new ColumnDefinition() { Width = new GridLength(20) };
            main.Children.Add(body);
            body.ColumnDefinitions.Add(bodyInputs);
            body.ColumnDefinitions.Add(bodyBody);
            body.ColumnDefinitions.Add(bodyOutputs);
            inVisualize = new StackPanel();
            outVisualize = new StackPanel();
            body.Children.Add(inVisualize);
            body.Children.Add(outVisualize);
            SetColumn(inVisualize, 0);
            SetColumn(outVisualize, 2);
            SetRow(body, 1);

            StackPanel bar = new StackPanel();
            type = new Label() {
                FontFamily = new FontFamily("Consolas"),
                FontStyle = FontStyles.Italic,
                IsHitTestVisible = false,
                Content = "None"
            };
            bar.Children.Add(type);
            TextBox title = new TextBox() {
                Text = NodeName,
                FontFamily = new FontFamily("Consolas"),
                FontStyle = FontStyles.Normal,
            };
            title.TextChanged += Title_TextChanged;
            bar.Children.Add(title);
            main.Children.Add(bar);

            Children.Add(main);

            MouseDown += MDown;
            MouseMove += MMove;
            MouseUp += MUp;
        }
        public NodeContainer(Node node) : this() {
            Node = node;
        }

        void Update() {
            type.Content = Node.GetType().Name;
            foreach (Button b in _inButtons.Concat(_outButtons)) {
                ((StackPanel)b.Parent).Children.Remove(b);
            }

            foreach (Node.Connection c in ConnectionsIn) {
                Button bIn = new Button() { Content = $"{c.Value}", AllowDrop = true };
                bIn.Click += TryConnectIn;
                inVisualize.Children.Add(bIn);
                _inButtons.Add(bIn);
            }
            foreach (Node.Connection c in ConnectionsOut) {
                Button bOut = new Button() { Content = $"{c.Value}" };
                bOut.Click += TryConnectOut;
                outVisualize.Children.Add(bOut);
                SetColumn(bOut, 2);
                _outButtons.Add(bOut);
            }
        }

        private void TryConnectIn(object sender, RoutedEventArgs e) {
            if (sender is Button b) {
                NodeManager.EndDrag(this, ConnectionsIn[_inButtons.IndexOf(b)]);
            }
        }

        private void TryConnectOut(object sender, RoutedEventArgs e) {
            if (sender is Button b) {
                int index = _outButtons.IndexOf(b);
                Node.Connection connection;
                try { connection = ConnectionsOut[index]; }
                catch { connection = ConnectionsOut[0]; }
                connection.node = Node;
                NodeManager.StartDrag(connection);
            }
        }

        protected override void OnInitialized(EventArgs e) {
            base.OnInitialized(e);
            if (Node == null) Node = new SplitNode(4);
        }

        private void Title_TextChanged(object sender, TextChangedEventArgs e) {
            NodeName = (sender as TextBox).Text;
        }
        private void MDown(object sender, MouseButtonEventArgs e) {
            Position = new Point(Canvas.GetLeft(this), Canvas.GetTop(this));

            Point mousePos = Utils.GetMousePosition();
            dragOffset = (Point)(mousePos - Position);

            dragging = true;
        }
        private void MMove(object sender, MouseEventArgs e) {
            if (!dragging) return;

            Point mousePos = Utils.GetMousePosition();
            Position = (Point)(mousePos - dragOffset);

            Canvas.SetLeft(this, Position.X);
            Canvas.SetTop(this, Position.Y);
        }
        private void MUp(object sender, MouseButtonEventArgs e) { dragging = false; }

    }
    public static class NodeManager {
        public static void StartDrag(Node.Connection start) {
            
        }
        public static void EndDrag(NodeContainer container, Node.Connection end) {
            container.NodeName = end.node?.ToString();
        }
    }

    public static class Utils {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point {
            public Int32 X;
            public Int32 Y;
        };
        public static Point GetMousePosition() {
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }
    }
}
