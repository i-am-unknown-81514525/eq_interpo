using eq_interpo.components;
using ui.math;
using ui.components;
using System.Collections.Generic;
using System.Linq;

namespace eq_interpo.math
{
    public struct Entry
    {
        public readonly Fraction x;
        public readonly Fraction fx;

        public Entry(Fraction x, Fraction fx)
        {
            this.x = x;
            this.fx = fx;
        }
    }

    public static class FieldParser
    {
        public static Entry[] Parse(Field[] fields)
        {
            List<Entry> entries = new List<Entry>();
            foreach (Field field in fields)
            {
                SingleLineInputField x_field = (SingleLineInputField)field.comp[0];
                SingleLineInputField fx_field = (SingleLineInputField)field.comp[1];
                entries.Add(
                    new Entry(
                        Fraction.Parse(x_field.content),
                        Fraction.Parse(fx_field.content)
                    )
                );
            }
            return entries.OrderBy(f => f.x).ToArray();
        }

        public static string GetSuperScript(int src)
        {
            string output = "";
            foreach (char c in src.ToString())
            {
                int v = int.Parse(c.ToString());
                output += Const.SUPER_SCRIPT_NUM[v];
            }
            return output;
        }

        public static List<DivDiff[]> Process(Entry[] entries)
        {
            List<DivDiff[]> output = new List<DivDiff[]>();
            DivDiff[] base_div = new DivDiff[entries.Length];
            for (int i = 0; i < entries.Length; i++)
            {
                base_div[i] = new DivDiff(0, i, i, entries[i].x, entries[i].x, entries[i].fx);
            }
            output.Add(base_div);
            for (int d = 1; d <= entries.Length - 1; d++)
            { // 2 value correspond to d1 only
                bool all_zero = true;
                int last = d - 1;
                DivDiff[] last_entries = output[last];
                DivDiff[] new_entries = new DivDiff[last_entries.Length - 1];
                for (int next = 1; next < last_entries.Length; next++)
                {
                    int curr = next - 1;
                    new_entries[curr] = last_entries[curr] + last_entries[next];
                    if (new_entries[curr].value != 0)
                    {
                        all_zero = false;
                    }
                }
                if (all_zero) break;
                output.Add(new_entries);
            }
            return output;
        }
    }
}
