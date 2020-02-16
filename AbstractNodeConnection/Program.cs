using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractNodeConnection {
    class Program {
        static void Main(string[] args) {
            InputNode root = new InputNode {
                Value = "10"
            };
            InputNode root2 = new InputNode {
                Value = "20"
            };

            AddNode add = new AddNode(2);
            root.ConnectOutput(add, 0, 0);
            root2.ConnectOutput(add, 1, 0);

            SplitNode pass = new SplitNode(3);
            pass.ConnectInput(add, 0, 0);

            AddNode add2 = new AddNode(4);
            pass.ConnectOutput(add2, 0, 0);
            pass.ConnectOutput(add2, 1, 1);
            pass.ConnectOutput(add2, 3, 2);

            InputNode root3 = new InputNode() {
                Value = 3
            };
            root3.ConnectOutput(add2, 2, 0);

            OutputNode o1 = new OutputNode();
            o1.ConnectInput(pass, 0, 1);


            //DisplayNode(root, 0, 1);
            //DisplayNode(root2, 0, 4);
            //DisplayNode(add, 12, 1);

            //DisplayNode(pass, 25, 1);
            //DisplayNode(o1, 45, 1);
            DisplayNodeTree(root);

            Console.ReadLine();
        }

        static void DisplayNode(Node n, int x, int y) {
            Console.SetCursorPosition(x, y);
            Console.Write(n.GetType().Name);

            for (int i = 0; i < n.NumberOfInputs; i++) {
                Console.SetCursorPosition(x, y + 1 + i);
                Console.Write($"{n.GetInputValue(i)}>");
            }
            for (int i = 0; i < n.NumberOfOutputs; i++) {
                string td = $"{n.GetOutputValue(i)}>";
                Console.SetCursorPosition(x + $"{n.GetType().Name}".Length/* - td.Length*/, y + 1 + i);
                Console.Write(td);
            }
        }

        static void DisplayNodeTree(Node root) {
            int cx = 0;
            int cy = 0;
            int lastx = 0;
            int lasty = 0;
            Console.SetCursorPosition(0, 0);

            List<Node> processed = new List<Node>();
            Stack<Node> toProcess = new Stack<Node>();
            toProcess.Push(root);

            Node current;
            while ((current = toProcess.Pop()) != null) {
                int longest = 0;

                string name = current.GetType().Name;
                longest = name.Length > longest ? name.Length : longest;
                Console.SetCursorPosition(cx, cy);
                Console.Write(name);
                for (int i = 0; i < current.NumberOfInputs; i++) {
                    string display = $"{current.GetInputValue(i)}>";
                    longest = display.Length > longest ? display.Length : longest;
                    Console.SetCursorPosition(cx, cy + 1 + i);
                    Console.Write(display);

                    Node currentInput = current.inputNodes[i].node;
                    if (currentInput != null && !processed.Contains(currentInput)) {
                        int unprocessedY = lasty + 1;
                        DisplayNode(current.inputNodes[i].node, lastx, unprocessedY);
                        processed.Add(currentInput);
                    }
                }
                lastx = cx;
                cx += longest;

                for (int i = 0; i < current.NumberOfOutputs; i++) {
                    string display = $"{current.GetOutputValue(i)}>";
                    longest = display.Length > longest ? display.Length : longest;
                    //try { Console.SetCursorPosition(cx - display.Length, cy + 1 + i); }
                    /*catch { */
                    Console.SetCursorPosition(cx, cy + 1 + i);/* }*/
                    Console.Write(display);
                    toProcess.Push(current.outputNodes[i].node);
                }
                cx += longest + 1;
                lasty = current.NumberOfInputs + current.NumberOfOutputs + 1;

                processed.Add(current);
            }
        }
    }
}
