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
            Switcher switcher = new Switcher();
            Logger logger = new Logger();
            PagingTable table = new PagingTable(new Field(new[] { new TextLabel("x"), new TextLabel("f(x)"), new TextLabel("Is Active") }));
            switcher = new Switcher() {
                new VerticalGroupComponent() {
                    (
                        table = new PagingTable(
                            new Field(new[] {new TextLabel("x"), new TextLabel("f(x)"), new TextLabel("Is Active")}),
                            new BoundedSpinner("Entries", 1, 1, 1000)
                            .WithChangeHandler(
                                (v) => {
                                    while (v > table.Count()) {
                                        table.Push(
                                            new Field(
                                                new IComponent[] {
                                                    new SingleLineInputField().WithChange((comp) => comp.underline = false),
                                                    new SingleLineInputField().WithChange((comp) => comp.underline = false),
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
                                    }
                                    while (v < table.Count()) {
                                        table.RemoveLast();
                                    }
                                }
                            )
                            .WithTriggerChange(),
                            new Button("Submit")
                            .WithHandler(
                                (__, ___) => {
                                    bool valid = true;
                                    if (table.Count() == 0) valid = false;
                                    Field[] fields = table.GetFields();
                                    List<Fraction> fracs = new List<Fraction>();
                                    foreach (Field field in fields) {
                                        for (int x = 0; x < 2; x++) {
                                            SingleLineInputField input = (SingleLineInputField)field.comp[x];
                                            if (Fraction.TryParse(input.content, out Fraction frac)) {
                                                if (x==0) {
                                                    if (fracs.Contains(frac)) {
                                                        valid = false;
                                                    } else {
                                                        fracs.Add(frac);
                                                    }
                                                }
                                            } else {
                                                valid = false;
                                            }
                                        }
                                    }
                                    if (valid) {
                                        switcher.SwitchTo(1);
                                    }
                                }
                            )
                        )
                    )
                },
                new VerticalGroupComponent() {
                    new TextLabel("Valid input and TODO"),
                    new Button("Back").WithHandler((__, ___) => switcher.SwitchTo(0))
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
