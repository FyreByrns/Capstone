namespace AbstractNodeConnection {
    public class PassthroughNode : Node {
        public PassthroughNode() {
            inputNodes = new Connection[1];
            outputNodes = new Connection[1];
        }

        public override object GetOutputValue(int index) {
            return GetInputValue(0);
        }
    }
}
