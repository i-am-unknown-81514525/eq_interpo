using ui.components;
using eq_interpo.math;
using System.Collections.Generic;
using System.Linq;

namespace eq_interpo
{

    public class DataOutput : Container
    {
        public readonly DivDiff[][] data;
        public readonly PagingTable table;

        public Field? GetFieldByIdx(int idx)
        {
            List<IComponent> content = new List<IComponent>();
            DivDiff[] initial = data[0];
            if (initial.Length * 2 - 1 <= idx || idx < 0)
            { // amount of field used is (initial.Length * 2 - 1) and idx must be less than the field used, It shouldn't be an off by one error dw future me :)
                return null;
            }
            if (idx % 2 == 0)
            {
                content.Add(new TextLabel(initial[idx / 2].start_idx.ToString()));
            }
            else
            {
                content.Add(new Padding());
            }
            for (int i = 0; i < data.Length; i++)
            {
                int r_idx = idx - i;
                if (r_idx % 2 == 0)
                {
                    content.Add(new TextLabel(data[i][r_idx / 2].value.ToString()));
                }
                else
                {
                    content.Add(new Padding());
                }
            }
            return new Field(content);
        }

        public DataOutput(List<DivDiff[]> data)
        {
            this.data = data.ToArray();
            List<string> fields_name = new List<string>() { "x" };
            for (int d = 0; d < this.data.Length; d++)
            {
                string prefix = $"Δ{FieldParser.GetSuperScript(d)}";
                fields_name.Add((d != 0 ? prefix : "") + "f(x)");
            }
            table = new PagingTable(new Field(fields_name.Select(f => new TextLabel(f))));
            Add(table);
            for (int i = 0; i < data[0].Length * 2 - 1; i++)
            {
                table.Push((Field)GetFieldByIdx(i));
            }
        }
    }
}
