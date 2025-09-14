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
    }
}
