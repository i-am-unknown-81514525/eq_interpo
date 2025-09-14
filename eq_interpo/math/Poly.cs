using ui.math;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ui.LatexExt;

namespace eq_interpo.math
{
    public struct Poly : ILatex
    {
        private readonly Fraction[] term;

        public Fraction this[int i]
        {
            get
            {
                if (i < 0) return term[i]; // could become better error later, for now just use the index out of range
                if (i < term.Length) return term[i]; // although same behaviour, making a clear logic distinction :)
                return new Fraction(0);
            }
        }

        public int GetGreatestOrder() => term.Length - 1;

        public Fraction[] GetInner() => term.ToArray();

        public Poly(params Fraction[] fracs)
        {
            this.term = fracs.ToArray();
        }

        public Poly(IEnumerable<Fraction> fracs)
        {
            this.term = fracs.ToArray();
        }

        public Poly(Poly src)
        {
            this.term = src.GetInner().ToArray();
        }

        public static Poly operator +(Poly left, Poly right)
        {
            bool left_max = left.GetGreatestOrder() >= right.GetGreatestOrder();
            Fraction[] base_v = left_max ? left.GetInner() : right.GetInner();
            Poly other = left_max ? right : left;
            for (int i = 0; i <= other.GetGreatestOrder(); i++)
            {
                base_v[i] += other[i];
            }
            return new Poly(base_v);
        }

        public static Poly operator -(Poly left, Poly right)
        {
            return left + new Poly(right.GetInner().Select(x => -x));
        }

        public static Poly operator *(Poly left, Poly right)
        {
            Fraction[] out_frac = new Fraction[left.GetGreatestOrder() + right.GetGreatestOrder() + 1];
            for (int i = 0; i < out_frac.Length; i++)
            {
                out_frac[i] = new Fraction(0);
            }
            for (int l_i = 0; l_i <= left.GetGreatestOrder(); l_i++)
            {
                for (int l_r = 0; l_r <= right.GetGreatestOrder(); l_r++)
                {
                    out_frac[l_i + l_r] += left[l_i] * right[l_r];
                }
            }
            return new Poly(out_frac);
        }

        public static Poly operator *(Poly left, Fraction right) => left * new Poly(right);
        public static Poly operator *(Fraction left, Poly right) => new Poly(left) * right;
        public static Poly operator +(Poly left, Fraction right) => left + new Poly(right);
        public static Poly operator +(Fraction left, Poly right) => new Poly(left) + right;
        public static Poly operator -(Poly left, Fraction right) => left - new Poly(right);
        public static Poly operator -(Fraction left, Poly right) => new Poly(left) - right;
        public static Poly operator -(Poly left) => new Fraction(0) - left;
        public static Poly operator +(Poly left) => left;

        public override string ToString()
        {
            var term = this.term;
            (int order, Fraction value)[] arr = Enumerable.Range(0, this.GetGreatestOrder() + 1).Select(i => (i, term[i])).OrderByDescending(d => d.i).ToArray();
            StringBuilder builder = new StringBuilder();
            bool initial = true;
            foreach ((int order, Fraction value) in arr)
            {
                if (value == 0 && (order != 0 || (order == 0 && !initial)))
                {
                    continue;
                }
                string sign = value >= 0 ? "+" : "";
                string postfix = $"x{FieldParser.GetSuperScript(order)}";
                if (initial && value >= 0)
                {
                    sign = "";
                }
                if (order == 1)
                {
                    postfix = "x";
                }
                if (order == 0)
                {
                    postfix = "";
                }
                builder.Append($"{sign}{value} {postfix}");
                initial = false;
            }
            return builder.ToString();
        }

        public string AsLatex()
        {
            var term = this.term;
            (int order, Fraction value)[] arr = Enumerable.Range(0, this.GetGreatestOrder() + 1).Select(i => (i, term[i])).OrderByDescending(d => d.i).ToArray();
            StringBuilder builder = new StringBuilder();
            bool initial = true;
            foreach ((int order, Fraction value) in arr)
            {
                if (value == 0 && (order != 0 || (order == 0 && !initial)))
                {
                    continue;
                }
                string sign = value >= 0 ? "+" : "";
                string postfix = $"x^{{{order}}}";
                if (initial && value >= 0)
                {
                    sign = "";
                }
                if (order == 1)
                {
                    postfix = "x";
                }
                if (order == 0)
                {
                    postfix = "";
                }
                if (value >= 0)
                    builder.Append($"{sign}{value.AsLatex().Trim('$')} {postfix}");
                else
                    builder.Append($"-{(-value).AsLatex().Trim('$')} {postfix}");
                initial = false;
            }
            return builder.ToString();
        }

        public Fraction Calculate(Fraction x)
        {
            Fraction base_v = new Fraction(0);
            for (int o = 0; o < term.Length; o++)
            {
                Fraction pow = new Fraction(1);
                for (int i = 0; i < o; i++)
                {
                    pow *= x;
                }
                base_v += pow * term[o];
            }
            return base_v;
        }
    }
}
