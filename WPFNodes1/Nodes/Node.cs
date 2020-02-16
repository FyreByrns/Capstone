namespace AbstractNodeConnection {
    // Each node contains a list of input nodes and a list of output nodes.
    // To determine the actual value of each output the node will query the relevant input nodes for values.
    // 
    // Each connection needs to know the index of connection with the other node
    // 
    public abstract class Node  {
        public int NumberOfInputs => inputNodes.Length;
        public int NumberOfOutputs => outputNodes.Length;
        public bool HasInput => NumberOfInputs != 0;
        public bool HasOutput => NumberOfOutputs != 0;

        public Connection[] inputNodes; // Connection.index is the connected node's output index
        public Connection[] outputNodes; // Connection.index is the connected node's input index

        public abstract object GetOutputValue(int index);
        public object GetInputValue(int index) {
            if (index >= NumberOfInputs) return null;
            return inputNodes[index].Value;
        }
        public virtual object[] GetAllOutputValues() {
            object[] toReturn = new object[NumberOfOutputs];
            for (int i = 0; i < NumberOfOutputs; i++)
                toReturn[i] = GetOutputValue(i);
            return toReturn;
        }

        public void ConnectInput(Node node, int indexTo, int indexFrom) {
            if (indexTo >= NumberOfInputs || indexFrom >= node.NumberOfOutputs) return;

            inputNodes[indexTo] = (node, indexFrom);
            node.outputNodes[indexFrom] = (this, indexTo);
        }
        public void ConnectOutput(Node node, int indexTo, int indexFrom) {
            if (indexFrom >= NumberOfOutputs || indexTo >= node.NumberOfInputs) return;

            outputNodes[indexFrom] = (node, indexTo);
            node.inputNodes[indexTo] = (this, indexFrom);
        }

        public struct Connection {
            public Node node;
            public int index;

            /// <summary>
            /// Only use on input connections.
            /// </summary>
            public object Value => node?.GetOutputValue(index);

            public Connection(Node n, int idx) { node = n; index = idx; }
            public static implicit operator Connection((Node n, int idx) a) => new Connection(a.n, a.idx);
        }
    }
}
