namespace AbstractNodeConnection {
    public class OutputNode : Node {
        public OutputNode() {
            inputNodes = new Connection[1];
            outputNodes = new Connection[0];
        }

        public override object GetOutputValue(int index) {
            return inputNodes[0].Value;
        }
    }
}
