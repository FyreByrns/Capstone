namespace AbstractNodeConnection {
    public class PassthroughNode : Node {
        public PassthroughNode() {
            inputNodes = new Connection[1];
            outputNodes = new Connection[1];
        }
        public PassthroughNode(int num) {
            inputNodes = new Connection[num];
            outputNodes = new Connection[num];
        }

        public override object GetOutputValue(int index) {
            return GetInputValue(index);
        }
    }
}
