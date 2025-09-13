using ui.components;
using ui.components.chainExt;

namespace eq_interpo.components
{
    public class InputTable : Container
    {
        public readonly FormattedTable table = new FormattedTable();
        public readonly Button back = new Button("Back");
        public readonly Button Start = new Button("Start");

        public InputTable() : base()
        {
            table.Resize((2, 2));
            table.WithComponentConstructor(() => new SingleLineInputField());
            table[0, 0] = new TextLabel("x");
            table[1, 0] = new TextLabel("f(x)");

        }
    }
}
