using ui.math;
using System;

namespace eq_interpo
{
    public class DivDiff
    {
        public int level { get; }
        public int start_idx { get; }
        public int end_idx { get; }
        public Fraction start { get; }
        Fraction end { get; }
        public Fraction value { get; } // E.g. (delta)f(f_0)

        public DivDiff(int level, int start_idx, int end_idx, Fraction start, Fraction end, Fraction value)
        {
            this.level = level;
            this.start_idx = start_idx;
            this.end_idx = end_idx;
            this.start = start;
            this.end = end;
            this.value = value;

        }

        public static DivDiff operator +(DivDiff left, DivDiff right)
        {
            if (left.level != right.level)
            {
                throw new InvalidOperationException("They have different derivative level, cannot merge");
            }
            if (left.start_idx > right.start_idx)
            {
                (left, right) = (right, left);
            }
            if (left.start_idx + 1 != right.start_idx || left.end_idx + 1 != right.end_idx)
            {
                throw new InvalidOperationException("They have different idx of difference, cannot merge");
            }
            Fraction amount = (right.value - left.value) / (right.end - left.start);
            DivDiff new_divdiff = new DivDiff(left.level + 1, left.start_idx, right.end_idx, left.start, right.end, amount);
            return new_divdiff;
        }
    }
}
