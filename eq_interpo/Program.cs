using ui.components;
using ui.components.chainExt;
using ui.math;
using eq_interpo.components;

// f(x) = f(x_0) + (x-x_0)(x-x_1)/1!h*(delta)f(x_0) + ...

namespace eq_interpo
{
    public static class Program
    {
        public static void Main(string[] _)
        {
            SingleLineInputField h_inputField = new SingleLineInputField();
            SingleLineInputField x0_inputField = new SingleLineInputField();
            FormattedTable table = new FormattedTable();
            Fraction h_value = 0;
            Fraction x0_value = 0;
            Switcher switcher = new Switcher();
            Logger logger = new Logger();
            switcher = new Switcher() {
                new VerticalGroupComponent() {
                    new PagingTable
                }
            };
            new App(switcher).Run();
        }
    }
}
