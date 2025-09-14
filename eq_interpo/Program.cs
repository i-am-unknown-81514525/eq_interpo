using ui.components;
using ui.components.chainExt;
using System.Linq;
using ui.math;
using eq_interpo.components;
using ui.fmt;
using System;
using System.Collections.Generic;
using eq_interpo.math;
using System.IO;

// f(x) = f(x_0) + (x-x_0)(x-x_1)/1!h*(delta)f(x_0) + ...

namespace eq_interpo
{
    public static class Program
    {
        public static Poly CalculatePolynomial(List<DivDiff[]> data)
        {
            DivDiff[] dd0 = data[0];
            DivDiff[] first = data.Select(x => x[0]).ToArray();

            List<Poly> result = new List<Poly>();
            foreach (DivDiff dd_each in first)
            {
                Fraction value = dd_each.value;
                if (value == 0) continue;
                Poly initial = new Poly(1);
                if (dd_each.start_idx == 0)
                {
                    ui.DEBUG.DebugStore.AppendLine($"dbg assert 1: {dd_each.start_idx} not 0 CalculatePolynomial");
                }
                for (int i = 0; i < dd_each.end_idx; i++) // dd_each.start_idx
                {
                    initial *= new Poly(-dd0[i].start, 1);
                }
                result.Add(value * initial);
                ui.DEBUG.DebugStore.AppendLine($"{value * initial}");
            }
            Poly rt_result = new Poly(0);
            foreach (Poly poly in result)
            {
                rt_result += poly;
            }
            return rt_result;
        }

        public static void ProcessMathDisplay(ContainerGroup group, Entry[] entries)
        {
            List<DivDiff[]> processed = FieldParser.Process(entries);
            group.out_table.Set(new DataOutput(processed));
            Poly result = CalculatePolynomial(processed);
            group.out_meta.Set(
                new Button(result.ToString())
                    .WithForeground<EmptyStore, Button>(ForegroundColorEnum.WHITE)
                    .WithBackground<EmptyStore, Button>(BackgroundColorEnum.BLACK)
                    .WithHandler((_, __) =>
                    {
                        ui.core.ConsoleHandler.ConsoleIntermediateHandler.WriteClipboard(result.AsLatex());
                        File.WriteAllText(".clipboard", result.AsLatex());
                    }
                )
            );
        }

        public static void Main(string[] _)
        {
            SingleLineInputField h_inputField = new SingleLineInputField();
            SingleLineInputField x0_inputField = new SingleLineInputField();
            Fraction h_value = 0;
            Fraction x0_value = 0;
            ComponentHolder<Switcher> switcher = new ComponentHolder<Switcher>();
            PagingTable table = new PagingTable(new Field(new[] { new TextLabel("x"), new TextLabel("f(x)"), new TextLabel("Is Active") }));
            ContainerGroup group = new ContainerGroup();
            switcher.inner = new Switcher() {
                new DataEntry(switcher, group),
                new VerticalGroupComponent() {
                    group.out_table,
                    (new HorizontalGroupComponent() {
                        new PageSwitcher(switcher, "Back", 0),
                        new PageSwitcher(switcher, "Next", 2)
                    }, 1)
                },
                new VerticalGroupComponent() {
                    group.out_meta,
                    (new PageSwitcher(switcher, "Back", 1), 1)
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
