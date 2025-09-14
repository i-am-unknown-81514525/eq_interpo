using ui.components;
using ui.components.chainExt;
using ui.fmt;
using ui.math;
using System.Collections.Generic;
using eq_interpo.math;

namespace eq_interpo.components
{
    public class DataEntry : Container
    {
        public readonly ComponentHolder<Switcher> switcher;
        public readonly PagingTable table;
        public readonly BoundedSpinner spinner;
        public readonly Logger logger = new Logger();
        public readonly ContainerGroup group;

        public static Field Constructor()
        {
            return new Field(
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
            );
        }

        public bool SubmitHandler()
        {
            bool valid = true;
            if (table.Count() == 0)
            {
                valid = false;
                logger.Push("Error: Have 0 entries");
            }
            Field[] fields = table.GetFields();
            List<Fraction> fracs = new List<Fraction>();
            foreach (Field field in fields)
            {
                for (int x = 0; x < 2; x++)
                {
                    SingleLineInputField input = (SingleLineInputField)field.comp[x];
                    if (Fraction.TryParse(input.content, out Fraction frac))
                    {
                        if (x == 0)
                        {
                            if (fracs.Contains(frac))
                            {
                                valid = false;
                                logger.Push("Error: Contain repeated x value");
                            }
                            else
                            {
                                fracs.Add(frac);
                            }
                        }
                    }
                    else
                    {
                        logger.Push("Error: Contain field that cannot be parsed as a fraction");
                        valid = false;
                    }
                }
            }
            if (valid)
            {
                Program.ProcessMathDisplay(group, FieldParser.Parse(table.GetFields()));
            }
            return valid;
        }

        public DataEntry(ComponentHolder<Switcher> switcher, ContainerGroup group)
        {
            this.switcher = switcher;
            this.group = group;
            Add(
                new VerticalGroupComponent() {
                    (
                        table = new PagingTable(
                            new Field(new[] {new TextLabel("x"), new TextLabel("f(x)"), new TextLabel("Is Active")}),
                            spinner = new BoundedSpinner("Entries", 1, 1, 1000)
                            .WithChangeHandler(
                                (v) => {
                                    while (v > table.Count()) {
                                        table.Push(Constructor());
                                    }
                                    while (v < table.Count()) {
                                        table.RemoveLast();
                                    }
                                }
                            ),
                            new PageSwitcher(switcher, "Submit", SubmitHandler, 1)
                        )
                    ),
                    (logger, 1),
                }
            );
            spinner.WithTriggerChange();
        }
    }
}
