using ui.components;
using ui.components.chainExt;
using ui.math;
using eq_interpo.components;
using ui.fmt;
using System;

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
            Switcher switcher = new Switcher();
            Logger logger = new Logger();
            PagingTable table = new PagingTable(new Field(new[] { new TextLabel("x"), new TextLabel("f(x)"), new TextLabel("Is Active") }));
            switcher = new Switcher() {
                new VerticalGroupComponent() {
                    (table = new PagingTable(
                        new Field(new[] {new TextLabel("x"), new TextLabel("f(x)"), new TextLabel("Is Active")}),
                        new Button("Add row")
                        .WithHandler((button, __) => {
                            table.Push(
                                new Field(
                                    new IComponent[] {
                                        new SingleLineInputField(),
                                        new SingleLineInputField(),
                                        new ToggleButton()
                                        .WithHandler<ToggleStore, ToggleButton>(
                                            (button_inner, ___) => {
                                                button_inner.store.isToggled = !button_inner.store.isToggled;
                                                if (button_inner.store.isToggled) {
                                                    button_inner.text = "☑";
                                                    button_inner.foreground = ForegroundColorEnum.GREEN;
                                                } else {
                                                    button_inner.text = "☐";
                                                    button_inner.foreground = ForegroundColorEnum.RED;
                                                }
                                        })
                                        .WithChange(
                                            (button_inner) => {
                                                button_inner.store.isToggled = true;
                                                button_inner.text = "☑";
                                                button_inner.foreground = ForegroundColorEnum.GREEN;
                                            }
                                        )
                                    }
                                )
                            );
                        })
                        .WithChange(
                            (comp) => comp.onClickHandler(comp, new ui.core.ConsoleLocation(0, 0))
                        )
                    ))
                }
            };
            new App(switcher).WithExitHandler<EmptyStore, App>((appObj) =>
            {
                Console.WriteLine(appObj.Debug_WriteStructure());
                Console.WriteLine(ui.DEBUG.DebugStore.ToString());
            }).Run();
        }
    }
}
