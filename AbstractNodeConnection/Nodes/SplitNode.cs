namespace AbstractNodeConnection {
    public class SplitNode : Node {
        public SplitNode(int times) {
            inputNodes = new Connection[1];
            outputNodes = new Connection[times];
        }

        public override object GetOutputValue(int index) {
            if (index >= NumberOfOutputs) return null;
            return GetInputValue(0);
        }
    }
}
