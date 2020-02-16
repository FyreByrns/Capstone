namespace AbstractNodeConnection {
    public class AddNode : Node {
        public AddNode(int inputs = 2) {
            inputNodes = new Connection[inputs];
            outputNodes = new Connection[1];
        }

        public override object GetOutputValue(int index) {
            int total = 0;

            for(int i = 0; i < NumberOfInputs; i++) {
                string tp = $"{GetInputValue(i)}";
                if (int.TryParse(tp, out int result)) {
                    total += result;
                }
                else return "err";
            }
            return total;
        }
    }
}
