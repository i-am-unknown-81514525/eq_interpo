using ui.components;
using System.Collections.Generic;
using System.Linq;
using System;
using ui.utils;

namespace eq_interpo.components
{
    public struct Field
    {
        public IComponent[] comp { get; set; }

        public Field(IEnumerable<IComponent> src)
        {
            comp = src.ToArray();
        }
    }

    internal class PagingTableInner : VirtualTable<FormattedTable>
    {
        public readonly int col;
        private readonly Field top;

        protected override FormattedTable InnerConstructor()
        {
            return new FormattedTable();
        }


        public PagingTableInner(Field top)
        {
            this.col = top.comp.Length;
            this.top = new Field(top.comp);
            ForceResize((col, 2));
        }

        public void PushFields(IEnumerable<Field> fields)
        {
            Field[] fieldsArr = fields.ToArray();
            ForceResize((col, 1 + fieldsArr.Length));
            for (int y = 1; y - 1 < fieldsArr.Length; y++)
            {
                int arr_y = y;
                Field field = fieldsArr[arr_y];
                if (field.comp.Length != inner.GetSize().x)
                {
                    throw new InvalidOperationException("Cannot use field with different size");
                }
                for (int x = 0; x < field.comp.Length; x++)
                {
                    this[x, y] = field.comp[x];
                }
            }
        }

        public override void InsertColumn(int idx, SplitAmount amount = null)
        {
            inner.InsertColumn(idx, amount);
        }
        public override void InsertRow(int idx, SplitAmount amount = null)
        {
            inner.InsertRow(idx, amount);
        }
        public override void RemoveColumn(int idx)
        {
            inner.RemoveColumn(idx);
        }
        public override void RemoveRow(int idx)
        {
            inner.RemoveRow(idx);
        }
    }

    public class PagingTable : Container
    {
        public BoundedSpinner spinner = new BoundedSpinner("Page", 1, 1, 1);

        private protected PagingTableInner inner;

        //Reactive of overlap with type int and default value: `0`, Trigger: SetHasUpdate();
        private int _overlap = 0;
        public int overlap
        {
            get => _overlap; set
            {
                if (value < 0) throw new InvalidOperationException("Cannot have negative overlap, overlap must be >= 0");
                _overlap = value;
                SetHasUpdate();
            }
        }

        private int pg_idx = 0; // the index of the first field of the page
        private int pg_end_idx = 0; // the index of the last field of the page
        private int virt_pg_idx = 0; // the virtual field space when resizing so it return to exact same page after a series of resize. Change on page change

        protected List<Field> fields = new List<Field>();

        public PagingTable(Field top)
        {
            inner = new PagingTableInner(top);
            Add(new VerticalGroupComponent()
            {
                inner,
                spinner
            });
            spinner.onChange = ChangePage;
        }

        public int GetPageRenderAmount()
        {
            int size = (int)GetAllocSize().y - 3;
            int act_size = size - overlap;
            if (act_size < 1) act_size = 1;
            return act_size;
        }

        public void ChangePage(int page)
        {
            pg_idx = virt_pg_idx = GetPageRenderAmount() * (page - 1);
            UpdateRender();
        }

        public void Push(Field item)
        {
            int curr_count = fields.Count;
            fields.Add(item);
            if (curr_count - 1 == pg_end_idx)
            {
                UpdateRender();
            }
        }

        public void UpdateRender()
        {
            Field[] result;
            (result, pg_idx, pg_end_idx) = RenderWith(virt_pg_idx);
            inner.PushFields(result);
            SetHasUpdate();
        }

        protected (Field[], int start, int end) RenderWith(int ref_idx)
        {
            int act_size = GetPageRenderAmount();
            int start_idx = (ref_idx / act_size) * act_size;
            int end_idx = start_idx + act_size - 1; // To make this inclusive idx
            if (end_idx >= fields.Count)
            {
                end_idx = fields.Count - 1;
            }
            int count = end_idx - start_idx + 1;
            return (fields.GetRange(start_idx, count).ToArray(), start_idx, end_idx);
        }

        protected override void OnResize()
        {
            base.OnResize();
            UpdateRender();
        }
    }
}
