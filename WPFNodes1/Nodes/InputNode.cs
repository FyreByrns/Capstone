namespace AbstractNodeConnection {
    public class InputNode : Node {
        public object Value { get; set; }

        public InputNode() {
            inputNodes = new Connection[0];
            outputNodes = new Connection[1];
        }

        public override object GetOutputValue(int index) {
            if (index >= outputNodes.Length) return null;
            return Value;
        }
    }
}
