using ui.components;
using ui.components.chainExt;
using ui.math;
using eq_interpo.components;
using ui.fmt;
using System;
using System.Collections.Generic;

// f(x) = f(x_0) + (x-x_0)(x-x_1)/1!h*(delta)f(x_0) + ...

namespace eq_interpo
{
    public static class Program
    {
        public static void Main(string[] _)
        {
            SingleLineInputField h_inputField = new SingleLineInputField();
            SingleLineInputField x0_inputField = new SingleLineInputField();
            Fraction h_value = 0;
            Fraction x0_value = 0;
            ComponentHolder<Switcher> switcher = new ComponentHolder<Switcher>();
            PagingTable table = new PagingTable(new Field(new[] { new TextLabel("x"), new TextLabel("f(x)"), new TextLabel("Is Active") }));
            switcher.inner = new Switcher() {
                new DataEntry(switcher),
                new VerticalGroupComponent() {
                    new TextLabel("Valid input and TODO"),
                    (new PageSwitcher(switcher, "Back", 0), 1)
                }
            };
            new App(switcher.inner).WithExitHandler<EmptyStore, App>((appObj) =>
            {
                Console.WriteLine(appObj.Debug_WriteStructure());
                Console.WriteLine(ui.DEBUG.DebugStore.ToString());
            }).Run();
        }
    }
}
